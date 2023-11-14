using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BEBE.Framework.Managers;
using BEBE.Framework.Utils;
using BEBE.Framework.Logging;
/// <summary>
/// 游戏入口
/// </summary>
public class GameLaucher : SingletonGameobject<GameLaucher>
{
    public class MgrsContainer : Singleton<MgrsContainer>
    {
        protected List<IMgr> mgrs = new List<IMgr>();
        public List<IMgr> AllMgrs => mgrs;
        public T GetMgr<T>() where T : IMgr
        {
            return (T)mgrs.Where(x => x is T).Single();
        }

        public T AddMgr<T>() where T : IMgr, new()
        {
            if (mgrs.Any(x => x.GetType() is T)) return default(T);
            T mgr = new T();
            mgrs.Add(mgr);
            BEBE.Framework.Logging.Debug.Log($"{mgr.GetType().ToString()} Added!");
            return mgr;
        }
    }

    private MgrsContainer container;
    public MgrsContainer Container => container;

    private void Awake()
    {
        container = GameLaucher.MgrsContainer.Instance;
        BEBE.Framework.Logging.Debug.prefix = " Frame Sync Test | " + System.DateTime.Now + " | ";
        BEBE.Framework.Logging.Debug.TraceModeOn();
        // 加载管理器
        Container.AddMgr<SrcMgr>()?.Awake();
        Container.AddMgr<UIMgr>()?.Awake();
        Container.AddMgr<NetMgr>()?.Awake();
    }

    void Start()
    {
        foreach (var mgr in Container.AllMgrs)
        {
            mgr?.Start();
        }

        // 启动游戏
        new Game().EnterGame();
    }

    private void Update()
    {
        foreach (var mgr in Container.AllMgrs)
        {
            mgr?.Update();
        }
    }

    private void OnDestroy()
    {
        foreach (var mgr in Container.AllMgrs)
        {
            mgr?.OnDestroy();
        }

        BEBE.Framework.Logging.Debug.FlushTrace();
    }
}
