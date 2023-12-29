using BEBE.Engine.Service.Net;
namespace BEBE.Engine.Service.Cmd
{
    public abstract class CmdService : BaseService
    {
        protected NetworkService m_netservice;
        public CmdService(NetworkService netService)
        {
            register_events();
            m_netservice = netService;
        }
    }
}