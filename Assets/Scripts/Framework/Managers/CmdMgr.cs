using BEBE.Engine.Math;
using BEBE.Engine.Service.Cmd;
using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;
using BEBE.Framework.Service;
using UnityEngine;

namespace BEBE.Framework.Managers
{
    //指令管理类
    public class CmdMgr : IMgr
    {
        private ClientCmdService m_ccmdsvc;
        private ServerCmdService m_scmdsvc;
        private NetworkService m_client => MgrsContainer.GetMgr<NetMgr>().Client;
        private NetworkService m_server => MgrsContainer.GetMgr<NetMgr>().Server;

        public override void Awake()
        {
            m_ccmdsvc = new ClientCmdService(m_client);
            m_scmdsvc = new ServerCmdService(m_server);
        }

        public override void FixedUpdate()
        {
            m_ccmdsvc.RecordInput();
        }

        public PlayerInputs GetPlayerInputs() => m_ccmdsvc.Inputs;
       
    }
}