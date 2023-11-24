using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Service.Cmd
{
    public abstract class CmdService : BaseService
    {
        protected NetService m_netservice;
        public CmdService(NetService netService)
        {
            register_events();
            m_netservice = netService;
        }
    }
}