using BEBE.Framework.Utils;
using BEBE.Framework.Managers;
using BEBE.Framework.Module;
using UnityEngine;
/// <summary>
/// 游戏入口
/// </summary>
public class GameLaucher : SingletonGameobject<GameLaucher>
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        // 设置Logger
        BEBE.Engine.Logging.Debug.SetLogHandler(BEBE.Engine.Logging.Logger.UnityLogHandler);
        BEBE.Engine.Logging.Debug.prefix = " Frame Sync Test | " + System.DateTime.Now + " | ";
        BEBE.Engine.Logging.Debug.TraceModeOn();
        // 加载管理器
        MgrsContainer.AddMgr<SrcMgr>();
        MgrsContainer.AddMgr<UIMgr>();
        MgrsContainer.AddMgr<NetMgr>();
        MgrsContainer.AddMgr<RoomMgr>();
        MgrsContainer.AddMgr<CommonStatusMgr>();
        MgrsContainer.AddMgr<SceneMgr>();
        MgrsContainer.Awake();
    }

    void Start()
    {
        MgrsContainer.Start();
        // 启动游戏
        new Game().EnterGame();
    }

    private IntervalExecuteHelper intervalExe = new IntervalExecuteHelper(Constant.TARGET_FRAME_RATE);

    private void Update()
    {
        try
        {
            MgrsContainer.Update();

            intervalExe.Invoke(MgrsContainer.FixedUpdate);
        }
        catch (System.Exception e)
        {
            BEBE.Engine.Logging.Debug.LogException(e);
        }
    }

    private void OnDestroy()
    {
        MgrsContainer.OnDestroy();

        BEBE.Engine.Logging.Debug.FlushTrace();
    }
}
