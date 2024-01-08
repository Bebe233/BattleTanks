using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    static readonly string asset_bundle_directory = "/AssetBundles/";
    [MenuItem("Utils/Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string full_path = Application.streamingAssetsPath + asset_bundle_directory;
        Debug.Log($"ab path --> {full_path}");
        if (!Directory.Exists(full_path)) Directory.CreateDirectory(full_path);
        BuildPipeline.BuildAssetBundles(full_path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
