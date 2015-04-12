using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

class AssetBundleBuilder : EditorWindow
{

    [MenuItem("Morln/AssetBundle Builder")]
    static void Init()
    {
        var window = GetWindow<AssetBundleBuilder>();
        window.title = "爆消农场Asset Bundle生成工具";
    }

    private static string _exportPath = Path.Combine(Application.dataPath, "StreamingAssets/");

    void OnGUI()
    {
        
        //targetPlatform = (BuildTarget)EditorGUILayout.EnumPopup("Target Platform", targetPlatform);
        //EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("导出位置 " + _exportPath);
        if (GUILayout.Button("浏览..."))
        {
            _exportPath = EditorUtility.OpenFolderPanel("选择导出位置", _exportPath, "");
        }
        //EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("生成用于" + EditorUserBuildSettings.activeBuildTarget + "的静态资源bundle"))
        {
            BuildFolderIntoBundle("/Resources/");
        }
        if (GUILayout.Button("生成用于" + EditorUserBuildSettings.activeBuildTarget + "的下载资源bundle"))
        {
            BuildFolderIntoBundle("/ResourcesForDownload/");
        }
        //EditorGUILayout.SelectableLabel(bldr.ToString());
    }

    static void GetAllDeepFilesInFolder(DirectoryInfo directory, List<string> filePathList)
    {
        filePathList.AddRange(directory.GetFiles().Select(x=>x.FullName));
        var dirs = directory.GetDirectories();
        foreach (var directoryInfo in dirs)
        {
            GetAllDeepFilesInFolder(directoryInfo, filePathList);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folderPath">相对于Application.dataPath</param>
    private void BuildFolderIntoBundle(string folderPath)
    {
        if (!Directory.Exists(_exportPath))
        {
            Debug.LogError("导出路径不存在");
            return;
        }
        var filePathList = new List<string>();
        GetAllDeepFilesInFolder(new DirectoryInfo(Application.dataPath + folderPath), filePathList);
        var relfilePathList = filePathList.Select(x => "Assets" + x.Substring(Application.dataPath.Length)).ToList();
        Debug.LogWarning("f:" + relfilePathList.Aggregate((c, x) => c + '\n' + x));
        var assets = relfilePathList.Select(x => AssetDatabase.LoadAssetAtPath(x, typeof(Object))).Where(x => x).ToArray();
        Debug.LogWarning("asset:" + assets.Select(x => x.name).Aggregate((c, x) => c + '\n' + x));

        string platformNode;
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                platformNode = "Android";
                break;
            case BuildTarget.iPhone:
                platformNode =  "IOS";
                break;
            default:
                platformNode = "Standalone";
                break;
        }
        var filename = string.Format("dlres_{0}_{1:00}.unity3d", platformNode, DateTime.Now.Month);
        var path = Path.Combine(_exportPath, filename);
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                BuildPipeline.BuildAssetBundle(assets[0], assets, path,
                    BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                    BuildTarget.Android);
                break;
            case BuildTarget.iPhone:
                BuildPipeline.BuildAssetBundle(assets[0], assets, path,
                    BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                    BuildTarget.iPhone);
                break;
            default:
                BuildPipeline.BuildAssetBundle(assets[0], assets, path,
                    BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                    BuildTarget.StandaloneWindows);
                break;
        }

        var bldr = new StringBuilder("生成完毕，共包含{");
        foreach (var t in assets)
        {
            bldr.AppendFormat("{0}:{1},", t.name, t.GetType());
        }
        bldr.Append("} 文件。路径为:").Append(path);
        Debug.Log(bldr.ToString());

        AssetDatabase.Refresh();
    }

    [MenuItem("My Menu/Clear Cache")]
    static void CleanCache()
    {
        Caching.CleanCache();
    }

    [MenuItem("My Menu/Unload Unused Assets")]
    static void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
    [MenuItem("My Menu/GC.Collect")]
    static void GCCollect()
    {
        System.GC.Collect();
    }
}