using BEBE.Framework.Utils;
using BEBE.Framework.Managers;
/// <summary>
/// 游戏入口
/// </summary>
public class GameLaucher : SingletonGameobject<GameLaucher>
{
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

            IntervalExecuteHelper.Invoke(MgrsContainer.FixedUpdate);
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
