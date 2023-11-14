using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BEBE.Framework.Event;

namespace BEBE.Framework.Service.Net
{
    public abstract class NetService : BaseService
    {
        protected string ip_address;
        protected int port;

        public virtual void Init(string ip_address, int port)
        {
            register_events();
            this.ip_address = ip_address;
            this.port = port;
        }

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void DoUpdate();
    }

    public class TCPClientService : NetService
    {
        byte id;
        private Channel m_channel;
        public override void Init(string ip_address, int port)
        {
            base.Init(ip_address, port);

            m_channel = new Channel(this, ip_address, port);
        }

        public override async void Connect()
        {
            await m_channel?.ConnectAsync();
        }

        public override void Disconnect()
        {
            //向服务端发送断开通知
            m_channel?.Send(new Packet(EventCode.ON_CLIENT_DISCONNECTED, id));
            m_channel?.Dispose();
            m_channel = null;
        }

        public override void DoUpdate()
        {
            m_channel?.Recv();
        }

        protected void EVENT_ON_CONNECTED_SERVER(object param)
        {
            id = (byte)param;
            Logging.Debug.Log($"EVENT_ON_CONNECTED_SERVER --> Your client id {id} from server");
        }
    }

    public class TCPServerService : NetService
    {
        TcpListener m_listenr;
        public override void Init(string ip_address, int port)
        {
            base.Init(ip_address, port);
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
                byte id = 0;
                while (toggle)
                {
                    TcpClient accept = await m_listenr.AcceptTcpClientAsync();
                    Logging.Debug.LogWarning($"SERVER --> new client {id} connected !");
                    Channel channel = new Channel(this, accept);
                    m_channels.AddOrUpdate(id, channel, (id, channel) => channel);
                    //将id返回给client
                    channel.Send(new Packet(Event.EventCode.ON_CONNECTED_SERVER, id));
                    id++;
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

        protected void EVENT_ON_CLIENT_DISCONNECTED(object param)
        {
            byte id = (byte)param;
            Logging.Debug.Log($"EVENT_ON_CLIENT_DISCONNECTED --> client {id} diconnected from server");
            if (m_channels.TryRemove(id, out Channel channel))
            {
                channel.Dispose();
                channel = null;
            }
        }
    }
}