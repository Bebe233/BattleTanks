using BEBE.Engine.Service.Net;
using BEBE.Framework.Component;
using System.Collections.Generic;
namespace BEBE.Engine.Service.Cmd
{
    public abstract class CmdService : BaseService
    {
        protected const int NUM_CHARACTOR = 10;
        protected NetService m_netservice;
        public CmdService(NetService netService)
        {
            register_events();
            m_netservice = netService;
        }

        public abstract void SendCmd(Cmd binput);
    }
}