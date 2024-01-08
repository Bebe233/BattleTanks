using System.IO;
using System.Collections.Generic;
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
        const string ab_directory = "AssetBundles";
#if UNITY_EDITOR
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
#endif
        private Dictionary<string, AssetBundle> assetbundles = new Dictionary<string, AssetBundle>();
        public GameObject GetPrefabAsset(string path, string assetbundle)
        {
            string uri = Path.Combine(Application.streamingAssetsPath, ab_directory, assetbundle);
            Debug.Log($"uri --> {uri}");
            AssetBundle bundle;
            //先判断AssetBundle是否已经加载
            if (assetbundles.ContainsKey(assetbundle))
            {
                bundle = assetbundles[assetbundle];
            }
            else
            {
                //加载AssetBundle
                bundle = AssetBundle.LoadFromFile(uri);
                assetbundles[assetbundle] = bundle;
            }

            if (bundle != null)
            {
                string url = Path.Combine(src_directory, path + ".prefab");
                return bundle.LoadAsset<GameObject>(url);
            }
            else
            {
                return null;
            }
        }

        public void UnloadAssetBundle(string assetbundle)
        {
            if (assetbundles.ContainsKey(assetbundle))
            {
                assetbundles[assetbundle].Unload(true);
                assetbundles.Remove(assetbundle);
            }
        }
    }

}