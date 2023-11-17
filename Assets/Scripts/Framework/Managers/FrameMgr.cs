
using BEBE.Engine.Math;
using UnityEngine;

namespace BEBE.Framework.Managers
{
    public class FrameMgr : IMgr
    {
        private long frame;
        public long Frame => frame;
        private bool toggle = false;

        public override void Start()
        {
            toggle = true;
            frame = 0;
        }

        public override void DoFixedUpdate()
        {
            if (toggle)
            {
                frame++;
                //BEBE.Engine.Logging.Debug.Log($"Frame {frame}");
            }
        }

        public override void OnDestroy()
        {
            toggle = false;
        }
    }
}