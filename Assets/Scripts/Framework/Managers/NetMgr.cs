using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BEBE.Framework.Service;
using BEBE.Framework.Utils;
using UnityEngine;
namespace BEBE.Framework.Managers
{
    //网络管理类
    //客户端实例化 net_service
    //服务端实例化 net_server
    public class NetMgr : IMgr
    {
        private NetService m_netService, m_netService2, m_netServer;

        public override void Awake()
        {
            m_netServer = new TCPServerService();
            m_netServer.Init("127.0.0.1", 9600);
            m_netService = new TCPClientService();
            m_netService.Init("127.0.0.1", 9600);
            m_netService2 = new TCPClientService();
            m_netService2.Init("127.0.0.1", 9600);
        }

        public override void Start()
        {
            m_netServer?.Connect();
            m_netService?.Connect();
            m_netService2?.Connect();
        }

        public override void OnDestroy()
        {
            m_netServer?.Disconnect();
            m_netService?.Disconnect();
            m_netService2?.Disconnect();
        }
    }


}