using BEBE.Framework.Service;
using BEBE.Game.Inputs;

namespace BEBE.Framework.Managers
{
    //指令管理类
    public class CmdMgr : IMgr
    {
        private ClientCmdService m_ccmdsvc;
        private ServerCmdService m_scmdsvc;
        private bool toggle = false;
        public override void Awake()
        {

            m_ccmdsvc = new ClientCmdService();
#if UNITY_EDITOR
            m_scmdsvc = new ServerCmdService();
#endif
        }

        public override void Start()
        {
            base.Start();
            toggle = true;
        }

        public override void FixedUpdate()
        {
            if (toggle)
                m_ccmdsvc?.RecordInput();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            toggle = false;
        }

        public TickInputsRollbackableCache InputsRollbackable => m_ccmdsvc.InputsRollbackable;
        public int TickSync => m_ccmdsvc.TickSync;
        public void SyncTick(int tick)
        {
            m_ccmdsvc.SyncTick(tick);
        }
    }
}