using System.Collections.Generic;
using System.Linq;
using BEBE.Engine.Math;
using BEBE.Framework.Managers;
using BEBE.Framework.Utils;
using UnityEngine;
/// <summary>
/// 游戏入口
/// </summary>
public class GameLaucher : SingletonGameobject<GameLaucher>
{
    public const int FrameRate = 30;
    public LFloat InverseFrameRate = 1.ToLFloat() / FrameRate;
    private float timer;
    private void Awake()
    {
        // 设置Logger
        BEBE.Engine.Logging.Debug.SetLogHandler(BEBE.Engine.Logging.Logger.UnityLogHandler);
        BEBE.Engine.Logging.Debug.prefix = " Frame Sync Test | " + System.DateTime.Now + " | ";
        BEBE.Engine.Logging.Debug.TraceModeOn();
        // 加载管理器
        MgrsContainer.AddMgr<SrcMgr>()?.Awake();
        MgrsContainer.AddMgr<UIMgr>()?.Awake();
        MgrsContainer.AddMgr<NetMgr>()?.Awake();

    }

    void Start()
    {
        foreach (var mgr in MgrsContainer.AllMgrs)
        {
            mgr?.Start();
        }

        // 启动游戏
        new Game().EnterGame();
    }

    private void Update()
    {
        foreach (var mgr in MgrsContainer.AllMgrs)
        {
            mgr?.Update();
        }

        do_fixed_update();
    }

    private void do_fixed_update()
    {
        timer += Time.deltaTime;
        if (timer >= InverseFrameRate)
        {
            timer -= InverseFrameRate;
            foreach (var mgr in MgrsContainer.AllMgrs)
            {
                mgr?.DoFixedUpdate();
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var mgr in MgrsContainer.AllMgrs)
        {
            mgr?.OnDestroy();
        }

        BEBE.Engine.Logging.Debug.FlushTrace();
    }
}
