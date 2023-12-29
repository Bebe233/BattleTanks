using BEBE.Engine.Managers;

namespace BEBE.Engine.Service
{
    public abstract class BaseService
    {
        public BaseService()
        {
            register_events();
        }
        
        protected void register_events()
        {
            Dispatchor.Register(this, "EVENT_");
        }
    }
}