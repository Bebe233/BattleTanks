using BEBE.Framework.Interface;

namespace BEBE.Framework.Managers
{
    public abstract class IMgr : ILifeCycle
    {
        public virtual void Awake()
        {
        }
        
        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnDestroy()
        {
        }
        
    }

}