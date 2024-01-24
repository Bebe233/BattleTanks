using BEBE.Engine.Service.Net;
using BEBE.Framework.Service.Net;
using BEBE.Framework.Utils;

namespace BEBE.Framework.Managers
{
    //网络管理类
    //客户端实例化 net_service
    //服务端实例化 net_server
    public class NetMgr : IMgr
    {
        private UServerService m_Server;
        public UServerService Server => m_Server;
        private UClientService m_client;
        public UClientService Client => m_client;
        private const int clients_num = 1;
        private const string ip = "127.0.0.1";
        private const int port = 9600;
        public override void Awake()
        {
#if UNITY_EDITOR
            m_Server = new UServerService(ip, port);
#endif
            m_client = new UClientService(ip, port);
        }

        public override void Start()
        {
            m_Server?.StartListening();
            m_client?.Connect();
        }
        public override void Update()
        {
            base.Update();
            m_Server?.DoUpdate();
            m_client?.DoUpdate();
        }

        public override void OnDestroy()
        {
            m_client?.Disconnect();
            m_Server?.StopListening();
        }
    }


}