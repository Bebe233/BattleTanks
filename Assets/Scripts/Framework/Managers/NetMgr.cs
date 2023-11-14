using BEBE.Framework.Service.Net;
using System.Collections.Generic;
namespace BEBE.Framework.Managers
{
    //网络管理类
    //客户端实例化 net_service
    //服务端实例化 net_server
    public class NetMgr : IMgr
    {
        private NetService m_netServer;
        private List<NetService> services = new List<NetService>();
        public override void Awake()
        {
            m_netServer = new TCPServerService();
            m_netServer.Init("127.0.0.1", 9600);

            for (int i = 0; i < 10; i++)
            {
                var m_netService = new TCPClientService();
                m_netService.Init("127.0.0.1", 9600);
                services.Add(m_netService);
            }
        }

        public override void Start()
        {
            m_netServer?.Connect();
            for (int i = 0; i < 10; i++)
            {
                services[i].Connect();
            }
        }

        public override void Update()
        {
            base.Update();
            m_netServer?.DoUpdate();
            for (int i = 0; i < 10; i++)
            {
                services[i].DoUpdate();
            }
        }

        public override void OnDestroy()
        {
            m_netServer?.Disconnect();
            for (int i = 0; i < 10; i++)
            {
                services[i].Disconnect();
            }
        }
    }


}