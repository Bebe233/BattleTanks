using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEBE.Engine.Math;
using UnityEngine;
namespace BEBE.Framework.Utils
{
    public class IntervalExecuteHelper
    {
        public int FrameRate { get; private set; }
        private LFloat InverseFrameRate;
        private float timer = 0;

        public IntervalExecuteHelper(int frame_rate)
        {
            FrameRate = frame_rate;
            InverseFrameRate = 1.ToLFloat() / FrameRate;
        }

        public void Invoke(Action handler)
        {
            timer += Time.deltaTime;
            if (timer >= InverseFrameRate)
            {
                timer -= InverseFrameRate;
                handler?.Invoke();
            }
        }
    }
}