using BEBE.Engine.Managers;

namespace BEBE.Engine.Service
{
    public abstract class BaseService
    {
        protected void register_events()
        {
            Dispatchor.Register(this, "EVENT_");
        }
    }
}