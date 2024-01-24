using BEBE.Engine.Service.Net;
using BEBE.Framework.Event;
using BEBE.Framework.Module;

namespace BEBE.Engine.Service.Cmd
{
    public class CmdService : BaseService
    {
        public CmdService()
        {
            register_events();
        }

        protected override void register_events()
        {
            Dispatchor.Register(this, Constant.EVENT_PREFIX);
        }
    }
}