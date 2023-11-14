using BEBE.Framework.Managers;
using BEBE.Framework.Math.BaseType;
using BEBE.Framework.Math;
using BEBE.Framework.Utils;
using UnityEngine;

public class Game : Singleton<Game>
{
    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame()
    {
        BEBE.Framework.Logging.Debug.Log("Enter game");
        //加载开始页面
        LoadSceneStartGame();

    }

    protected UIMgr uiMgr => GameLaucher.Instance.Container.GetMgr<UIMgr>();
    protected SrcMgr srcMgr => GameLaucher.Instance.Container.GetMgr<SrcMgr>();
    protected void LoadSceneStartGame()
    {
        // uiMgr.LoadCanvasUI<GameStartUIView>();
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

    }
}