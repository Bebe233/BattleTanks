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
        private CmdService m_service;
        private TCPClientService m_client => MgrsContainer.GetMgr<NetMgr>().Client;
        public override void Awake()
        {
            m_service = new ClientCmdService(m_client);
            m_binput = new PlayerInput();
        }

        public override void Update()
        {
            get_command();
            m_service.SendCmd(m_binput);
        }

        PlayerInput m_binput;
        public PlayerInput BInput => m_binput;
        protected void get_command()
        {
            //获取移动的指令
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            //装载
            m_binput.x = x.ToLFloat();
            m_binput.y = y.ToLFloat();

        }
    }
}