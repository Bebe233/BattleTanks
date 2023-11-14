using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BEBE.Framework.Interface
{
    public interface ILifeCycle
    {
        void Awake();

        void Start();

        void Update();

        void OnDestroy();
    }
}