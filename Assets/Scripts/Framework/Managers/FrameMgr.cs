
using BEBE.Engine.Math;
using UnityEngine;

namespace BEBE.Framework.Managers
{
    public class FrameMgr : IMgr
    {
        private int tick;
        public int Tick => tick;
        private bool toggle = false;

        public override void Start()
        {
            toggle = true;
            tick = 0;
        }

        public override void FixedUpdate()
        {
            if (toggle)
            {
                ++tick;
                //BEBE.Engine.Logging.Debug.Log($"Frame {frame}");
            }
        }

        public override void OnDestroy()
        {
            toggle = false;
        }
    }
}