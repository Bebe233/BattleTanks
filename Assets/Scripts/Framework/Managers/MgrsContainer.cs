using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using BEBE.Engine.Interface;

namespace BEBE.Framework.Managers
{
    public static class MgrsContainer
    {
        private static ConcurrentDictionary<Type, IMgr> mgrs = new ConcurrentDictionary<Type, IMgr>();
        private static ConcurrentQueue<IMgr> mgrs_queue = new ConcurrentQueue<IMgr>();

        public static T GetMgr<T>() where T : IMgr, new()
        {
            if (mgrs.TryGetValue(typeof(T), out IMgr mgr))
            {
                return (T)mgr;
            }
            else
            {
                return null;
            }
        }

        public static T AddMgr<T>() where T : IMgr, new()
        {
            if (mgrs.ContainsKey(typeof(T))) return GetMgr<T>();
            T mgr = new T();

            if (mgrs.TryAdd(typeof(T), mgr))
            {
                mgrs_queue.Enqueue(mgr);
                BEBE.Engine.Logging.Debug.Log($"{mgr.GetType().ToString()} added!");
                return mgr;
            }
            else
            {
                BEBE.Engine.Logging.Debug.Log($"{mgr.GetType().ToString()} failed to add!");
                return default(T);
            }
        }

        public static void Awake()
        {
            foreach (var mgr in mgrs_queue)
            {
                mgr?.Awake();
            }
        }

        public static void Start()
        {
            foreach (var mgr in mgrs_queue)
            {
                mgr?.Start();
            }
        }

        public static void Update()
        {
            foreach (var mgr in mgrs_queue)
            {
                mgr?.Update();
            }
        }

        public static void FixedUpdate()
        {
            foreach (var mgr in mgrs_queue)
            {
                mgr?.FixedUpdate();
            }
        }

        public static void OnDestroy()
        {
            foreach (var mgr in mgrs_queue)
            {
                mgr?.OnDestroy();
            }
        }

    }
}