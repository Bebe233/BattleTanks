using System.IO;
using UnityEditor;
using UnityEngine;

namespace BEBE.Framework.Managers
{
    //资源加载类
    //在EditorMode下通过AssetDataBase加载
    //在PlatformMode下通过AssetBundle加载
    public class SrcMgr : IMgr
    {
        const string src_directory = "Assets/AssetsPackage/prefabs";
        public GameObject GetPrefabAsset(string path)
        {
            string url = Path.Combine(src_directory, path + ".prefab");
            Debug.Log($"load path : {url} ");
            GameObject res = AssetDatabase.LoadAssetAtPath<GameObject>(url);
            if (res == null)
            {
                Debug.LogError("failed to get prefab asset!");
            }
            return res;
        }
    }

}