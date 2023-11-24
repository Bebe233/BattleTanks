using BEBE.Engine.Service.Net;
using System.Collections.Generic;
namespace BEBE.Framework.Managers
{
    //网络管理类
    //客户端实例化 net_service
    //服务端实例化 net_server
    public class NetMgr : IMgr
    {
        private NetService m_Server;
        public NetService Server => m_Server;
        private TCPClientService m_client;
        public TCPClientService Client => m_client;
        private const int clients_num = 1;
        private const string ip = "127.0.0.1";
        private const int port = 9600;
        public override void Awake()
        {
            m_Server = new TCPServerService(ip, port);
            m_client = new TCPClientService(ip, port);
        }

        public override void Start()
        {
            m_Server?.Connect();
            m_client?.Connect();
        }

        public override void Update()
        {
            m_Server?.DoUpdate();
            m_client?.DoUpdate();
        }

        public override void OnDestroy()
        {
            m_Server?.Disconnect();
            m_client?.Disconnect();
        }
    }


}