using System.Linq;
using Assets.Sanxiao;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 服务器模拟器
/// </summary>
public class GameDataWatcher : EditorWindow
{
    [MenuItem("Morln/GameDataWatcher")]
    static void Init()
    {
        var editor = GetWindow<GameDataWatcher>();
        editor.title = "GameData数据监视器";
    }

    void Update()
    {
        Repaint();
    }

    private void OnGUI()
    {
        EditorGUILayout.Foldout(true, "OurHealthList");
        for (int i = 0; i < GameData.OurHealthList.Length; i++)
        {
            GameData.OurHealthList[i] = EditorGUILayout.IntField(i.ToString(), GameData.OurHealthList[i]);
        }
        EditorGUILayout.Foldout(true, "RivalHealthList");
        for (int i = 0; i < GameData.RivalHealthList.Length; i++)
        {
            GameData.RivalHealthList[i] = EditorGUILayout.IntField(i.ToString(), GameData.RivalHealthList[i]);
        }
        EditorGUILayout.Foldout(true, "OurEnergyList");
        for (int i = 0; i < GameData.OurEnergyList.Length; i++)
        {
            GameData.OurEnergyList[i] = EditorGUILayout.IntField(i.ToString(), GameData.OurEnergyList[i]);
        }
        EditorGUILayout.LabelField("OurEnergyList",
                                   GameData.OurEnergyList.Select(x => x.ToString()).Aggregate((c, x) => c + "," + x));
        EditorGUILayout.LabelField("RivalEnergyList",
                                   GameData.RivalEnergyList.Select(x => x.ToString()).Aggregate((c, x) => c + "," + x));
    }
}