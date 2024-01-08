namespace BEBE.Engine.Service
{
    public abstract class BaseService
    {
        public BaseService()
        {
            register_events();
        }

        protected abstract void register_events();
    }
}