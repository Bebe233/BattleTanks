using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
/// <summary>
/// 游戏入口
/// </summary>
public class GameLaucher : SingletonGameobject<GameLaucher>
{
    public class MgrsContainer : Singleton<MgrsContainer>
    {
        protected List<IMgr> mgrs = new List<IMgr>();
        public T GetMgr<T>() where T : IMgr
        {
            return (T)mgrs.Where(x => x is T).Single();
        }

        public T AddMgr<T>() where T : IMgr, new()
        {
            if (mgrs.Any(x => x.GetType() is T)) return default(T);
            T mgr = new T();
            mgrs.Add(mgr);
            Debug.Log($"{mgr.GetType().ToString()} Added!");
            return mgr;
        }
    }

    public MgrsContainer Container;

    private void Awake()
    {
        Container = GameLaucher.MgrsContainer.Instance;
        // 加载管理器
        Container.AddMgr<SrcMgr>()?.Start();
        Container.AddMgr<UIMgr>()?.Start();
    }

    void Start()
    {
        // 启动游戏
        new Game().EnterGame();
    }

}
