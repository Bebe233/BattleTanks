using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BEBE.Framework.Event;
using BEBE.Framework.Managers;
using UnityEngine;

namespace BEBE.Framework.Service
{
    public abstract class BaseService
    {
        protected void register_events()
        {
            GameLaucher.Instance.Container.GetMgr<DispatchMgr>().Register(this, "EVENT_");
        }
    }
}