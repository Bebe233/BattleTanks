using System.Collections;
using System.Collections.Generic;
using Bebe.Framework.Interface;
using UnityEngine;
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