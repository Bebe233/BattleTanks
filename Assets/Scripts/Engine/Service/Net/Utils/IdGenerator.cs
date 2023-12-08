using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BEBE.Engine.Service.Net.Utils
{
    public class IdGenerator
    {
        private static int id = -1;

        public static int Get()
        {
            return ++id;
        }
    }
}