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
        public const int FrameRate = 30;
        private static LFloat InverseFrameRate = 1.ToLFloat() / FrameRate;
        private static float timer = 0;
        public static void Invoke(Action handler)
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