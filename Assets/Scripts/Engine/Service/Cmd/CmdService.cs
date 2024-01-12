using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using BEBE.Framework.Module;

namespace BEBE.Engine.Service.Cmd
{
    public class CmdService : BaseService
    {
        protected NetworkService m_netservice;
        public CmdService(NetworkService netService)
        {
            register_events();
            m_netservice = netService;
        }

        protected override void register_events()
        {
            Dispatchor.Register(this, Constant.EVENT_PREFIX);
        }
    }
}