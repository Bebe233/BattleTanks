using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BEBE.Engine.Event;
using BEBE.Engine.Service.Net;
using BEBE.Engine.Service.Net.Utils;

namespace BEBE.Engine.Service.Net
{
    public abstract class NetService : BaseService
    {
        protected string ip_address = "127.0.0.1";
        protected int port = 9600;

        public NetService(string ip_address, int port)
        {
            register_events();
            this.ip_address = ip_address;
            this.port = port;
        }

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void DoUpdate();
        public abstract void Send(Packet packet);
    }

    public class TCPClientService : NetService
    {
        public int Id => m_channel.id;
        private Channel m_channel;

        public TCPClientService(string ip_address, int port) : base(ip_address, port)
        {
            m_channel = new Channel(ip_address, port);
        }

        public override async void Connect()
        {
            await m_channel?.ConnectAsync();
        }

        public override void Disconnect()
        {
            //向服务端发送断开通知
            m_channel?.Send(new Packet(new EventMsg(EventCode.ON_CLIENT_DISCONNECTED, m_channel.id)));
            m_channel?.Dispose();
            m_channel = null;
        }
        public override void Send(Packet packet)
        {
            m_channel?.Send(packet);
        }

        public override void DoUpdate()
        {
            m_channel?.Recv();
        }


        protected void EVENT_ON_SERVER_CONNECTED(object param)
        {
            EventMsg msg = (EventMsg)param;
            m_channel.id = msg.Id;
            Logging.Debug.Log($"EVENT_ON_SERVER_CONNECTED --> Your client id is {msg.Id} to server");
            m_channel?.Send(new Packet(new EventMsg(EventCode.ON_CLIENT_CONNECTED, msg.Id)));
            // ping();
        }

        private void ping()
        {
            m_channel?.Send(new Packet(new EventMsg(EventCode.PING, BytesHelpper.long2bytes(System.DateTime.Now.ToBinary()), m_channel.id)));
        }

        protected void EVENT_PING_RPC(object param)
        {
            EventMsg msg = (EventMsg)param;
            byte[] content = msg.Content;
            long databinary = BytesHelpper.bytes2long(content);
            System.DateTime date = System.DateTime.FromBinary(databinary);
            double milliseconds = (System.DateTime.Now - date).TotalMilliseconds;
            Logging.Debug.Log($"clinet {m_channel.id} ping is {milliseconds} ms ");
        }

        protected void EVENT_SEND_JOIN_REQUEST(object param)
        {
            Logging.Debug.Log($"EVENT_SEND_JOIN_REQUEST");
            m_channel?.Send(new Packet(new EventMsg(EventCode.ON_JOIN_REQUEST_RECV, m_channel.id)));
        }

    }

    public class TCPServerService : NetService
    {
        TcpListener m_listenr;

        public TCPServerService(string ip_address, int port) : base(ip_address, port)
        {
            m_listenr = new TcpListener(IPAddress.Parse(ip_address), port);
            m_listenr.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            m_listenr.Server.NoDelay = true; //关闭 Nagle 算法
            //Nagle 算法旨在通过导致套接字缓冲小数据包来减少网络流量，然后在某些情况下合并并将它们发送到一个数据包。
            //TCP 数据包由 40 字节的标头以及要发送的数据组成。 使用 TCP 发送小数据包时，TCP 标头产生的开销可能成为
            //网络流量的重要组成部分。 在负载繁重的网络上，由此开销导致的拥塞可能导致数据报和重新传输丢失，以及拥塞
            //导致的传播时间过长。 当新传出数据从用户到达时，Nagle 算法会抑制新 TCP 段的发送，如果以前在连接上传输
            //过的数据保持未确认状态。大多数网络应用程序应使用 Nagle 算法。
        }

        ConcurrentDictionary<int, Channel> m_channels = new ConcurrentDictionary<int, Channel>();
        bool toggle = false;
        public override void Connect()
        {
            toggle = true;
            m_listenr.Start();
            //新线程监听客户端连接请求
            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (toggle)
                {

                    TcpClient accept = await m_listenr.AcceptTcpClientAsync();
                    int id = IdGenerator.Get();
                    Logging.Debug.LogWarning($"SERVER --> accepting a new client {id} ...");
                    Channel channel = new Channel(id, accept);
                    m_channels.AddOrUpdate(id, channel, (id, channel) => channel);
                    //将id返回给client
                    channel.Send(new Packet(new EventMsg(Event.EventCode.ON_SERVER_CONNECTED, id)));
                    // channel.Send(new Packet(new StringMsg($"Hello client {id}!")));
                }
            });
        }

        public override void Disconnect()
        {
            toggle = false;
            foreach (var channel in m_channels.Values)
            {
                channel.Dispose();
            }
            m_channels.Clear();
            m_listenr.Stop();
        }

        public override void DoUpdate()
        {
            foreach (var channel in m_channels.Values)
            {
                channel.Recv();
            }
        }
        public override void Send(Packet packet)
        {
            foreach (var channel in m_channels.Values)
            {
                channel.Send(packet);
            }
        }
        protected void EVENT_ON_CLIENT_CONNECTED(object param)
        {
            EventMsg msg = (EventMsg)param;
            Logging.Debug.Log($"EVENT_ON_CLIENT_CONNECTED --> client {msg.Id} connected");
        }

        protected void EVENT_ON_CLIENT_DISCONNECTED(object param)
        {
            EventMsg msg = (EventMsg)param;
            Logging.Debug.Log($"EVENT_ON_CLIENT_DISCONNECTED --> client {msg.Id} diconnected");
            if (m_channels.TryRemove(msg.Id, out Channel channel))
            {
                channel.Dispose();
                channel = null;
            }
        }

        protected void EVENT_PING(object param)
        {
            EventMsg msg = (EventMsg)param;
            if (m_channels.TryGetValue(msg.Id, out Channel channel))
            {
                channel.Send(new Packet(new EventMsg(EventCode.PING_RPC, msg.Content, channel.id)));
            }
        }

    }
}