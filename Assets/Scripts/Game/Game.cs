using BEBE.Framework.Managers;
using BEBE.Framework.Utils;
using UnityEngine;

public class Game : Singleton<Game>
{
    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        BEBE.Engine.Logging.Debug.Log("Enter game");
        //加载开始页面
        // LoadSceneStartGame();
    }

    protected UIMgr uiMgr => MgrsContainer.GetMgr<UIMgr>();
    protected SrcMgr srcMgr => MgrsContainer.GetMgr<SrcMgr>();
    public void LoadSceneStartGame()
    {
        // uiMgr.LoadCanvasUI<GameStartUIView>();
        MgrsContainer.AddMgr<FrameMgr>()?.Awake();
        MgrsContainer.GetMgr<FrameMgr>()?.Start();
        MgrsContainer.AddMgr<CmdMgr>()?.Awake();
        MgrsContainer.GetMgr<CmdMgr>()?.Start();
        MgrsContainer.AddMgr<EntityMgr>()?.Awake();
        MgrsContainer.GetMgr<EntityMgr>()?.Start();

    }

    public void LoadSceneLevel(int selectionIndex)
    {
        uiMgr.ClearStack();
        // selectionIndex 
        switch (selectionIndex)
        {
            case 0: // 1 player 
                break;
            case 1: // 2 players
                break;
        }
        Debug.Log($"LoadSceneLevel Mode {selectionIndex}");
        //加载地图
        GameObject map_1 = srcMgr.GetPrefabAsset("maps/map_1");
        GameObject.Instantiate(map_1, GameObject.Find("Canvas").transform);
        //加载角色

        //开始游戏

    }
}
