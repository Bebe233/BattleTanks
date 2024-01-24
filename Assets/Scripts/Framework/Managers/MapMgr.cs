//加载地图
using BEBE.Game.Map;
using UnityEngine;
namespace BEBE.Framework.Managers
{
    public class MapMgr : IMgr
    {
        public GameObject CurrentMap => current_map;
        private GameObject current_map;
        private SrcMgr srcMgr => MgrsContainer.GetMgr<SrcMgr>();
        public void LoadMap(string path_map)
        {
            GameObject prefab;
#if UNITY_EDITOR
            prefab = srcMgr.GetPrefabAsset(path_map);
#elif UNITY_STANDALONE
            prefab= srcMgr.GetPrefabAsset(path_map,"maps");
#endif
            current_map = GameObject.Instantiate(prefab);
            current_map.AddComponent<Map>();
        }
    }
}