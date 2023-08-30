using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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
