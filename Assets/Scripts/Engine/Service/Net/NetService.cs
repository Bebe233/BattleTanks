using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BEBE.Engine.Service.Net.Utils;
using System;

namespace BEBE.Engine.Service.Net
{
    public abstract class NetworkService : BaseService
    {
        public abstract void DoUpdate();
        public abstract void Send(Packet packet);
    }

    public abstract class ClientService : NetworkService
    {
        public int Id => m_channel.Id;
        protected Channel m_channel;
        private bool is_connecting = false;
        public event Action OnConnected, OnDisconnected;

        public ClientService(string ip_address, int port)
        {

        }

        public virtual async void Connect()
        {
            if (is_connecting) return;
            await m_channel?.ConnectAsync();
            is_connecting = true;
            OnConnected?.Invoke();
        }

        public virtual void Disconnect()
        {
            if (!is_connecting) return;
            is_connecting = false;
            OnDisconnected?.Invoke();
        }

        public override void Send(Packet packet)
        {
            if (is_connecting)
                m_channel?.Send(packet);
        }

        
        public override void DoUpdate()
        {
            if (is_connecting)
            {
                m_channel?.RecieveMsg();
            }
        }
    }

    public abstract class ServerService : NetworkService
    {
        private TcpListener m_listenr;

        public ServerService(string ip_address, int port)
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

        protected ConcurrentDictionary<int, Session> m_sessions = new ConcurrentDictionary<int, Session>();
        private bool toggle_listener = false;
        public event Action<int, TcpClient> OnClientAccepted;
        private IdGenerator id_gen = new IdGenerator();
        public virtual void StartListening()
        {
            toggle_listener = true;
            m_listenr.Start();
            //新线程监听客户端连接请求
            ThreadPool.QueueUserWorkItem(async state =>
            {
                while (toggle_listener)
                {

                    TcpClient acceptor = await m_listenr.AcceptTcpClientAsync();
                    int channel_id = id_gen.Get();
                    OnClientAccepted?.Invoke(channel_id, acceptor);
                    Logging.Debug.LogWarning($"SERVER --> accepting a new client {channel_id} ...");
                }
            });
        }

        public virtual void StopListening()
        {
            toggle_listener = false;
            foreach (var session in m_sessions.Values)
            {
                session.Dispose();
            }
            m_sessions.Clear();
            m_listenr.Stop();
        }

        public override void DoUpdate()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                foreach (var session in m_sessions.Values)
                {
                    session.RecieveMsg();
                }
            });
        }

        public override void Send(Packet packet)
        {
            foreach (var session in m_sessions.Values)
            {
                session.Send(packet);
            }
        }


    }
}