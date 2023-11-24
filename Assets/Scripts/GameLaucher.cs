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
        MgrsContainer.Start();

        // 启动游戏
        new Game().EnterGame();
    }

    private void Update()
    {
        try
        {
            MgrsContainer.Update();
            do_fixed_update();
        }
        catch
        {

        }
    }

    private void do_fixed_update()
    {
        timer += Time.deltaTime;
        if (timer >= InverseFrameRate)
        {
            timer -= InverseFrameRate;
            MgrsContainer.FixedUpdate();
        }
    }

    private void OnDestroy()
    {
        MgrsContainer.OnDestroy();

        BEBE.Engine.Logging.Debug.FlushTrace();
    }
}
