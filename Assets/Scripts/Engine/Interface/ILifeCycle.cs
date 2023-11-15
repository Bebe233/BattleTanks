namespace BEBE.Engine.Interface
{
    public interface ILifeCycle
    {
        void Awake();

        void Start();

        void Update();

        void OnDestroy();
    }
}