using System.Collections.Generic;
using System.Linq;

namespace BEBE.Framework.Managers
{
    public static class MgrsContainer
    {
        private static List<IMgr> mgrs = new List<IMgr>();
        public static List<IMgr> AllMgrs => mgrs;
        public static T GetMgr<T>() where T : IMgr
        {
            return (T)mgrs.Where(x => x is T).Single();
        }

        public static T AddMgr<T>() where T : IMgr, new()
        {
            if (mgrs.Any(x => x.GetType() is T)) return default(T);
            T mgr = new T();
            mgrs.Add(mgr);
            BEBE.Engine.Logging.Debug.Log($"{mgr.GetType().ToString()} Added!");
            return mgr;
        }
    }
}