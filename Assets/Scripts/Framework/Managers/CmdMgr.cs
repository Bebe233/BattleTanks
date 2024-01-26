using BEBE.Framework.Module;
using BEBE.Framework.Service;
using BEBE.Framework.Utils;
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

        private IntervalExecuteHelper intervalExe = new IntervalExecuteHelper(Constant.LOGIC_FRAME_RATE);
        public override void Update()
        {
            intervalExe?.Invoke(invoke);
        }

        private void invoke()
        {
            m_ccmdsvc.push_cmd();
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