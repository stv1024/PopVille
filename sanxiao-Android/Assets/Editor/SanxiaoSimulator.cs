using Assets.Sanxiao;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.Game.Skill;
using Assets.Sanxiao.Test;
using Assets.Sanxiao.UI;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 服务器模拟器
/// </summary>
public class SanxiaoSimulator : EditorWindow
{
    [MenuItem("Morln/SanxiaoSimulator")]
    static void Init()
    {
        var editor = GetWindow<SanxiaoSimulator>();
        editor.title = "三消测试器";
    }

    void FixedUpdate()
    {
        Debug.Log("FixedUpdate@" + Time.time);
        OnGUI();
    }
    void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Time", Time.time.ToString());
        EditorGUILayout.BeginHorizontal();
        {
            Time.timeScale = EditorGUILayout.Slider("TimeScale", Time.timeScale, 0, 16);
            if (GUILayout.Button("1"))
            {
                Time.timeScale = 1;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("StartRealTimeRound"))
            {
                GameManager.Instance.StartRealTimeRound();
            }
            if (GUILayout.Button("UploadChallengeOk"))
            {
                GameData.LastChallengeMajorLevelID = 1;
                CommonData.MyUser.ExpCeil = 1000;
                CommonData.MyUser.Exp = 550;
                CommonData.MyUser.ExpFloor = 0;
                MainRoot.Goto(MainRoot.UIStateName.EndRound);
                Responder.Instance.Execute(TestData.UploadChallengeOk0);
                EndRoundUI.Instance.PlayEndRoundProcess(TestData.UploadChallengeOk0);
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add Hp Prob"))
            {
                GameData.LastSubLevelData.HealthBottleProbability = 1000;
                GameData.LastSubLevelData.HealthBottleCapacity = 50;
            }
            if (GUILayout.Button("Energy Bottle"))
            {
                GameManager.Instance.MyGrid.AddItemToQueue(new CandyInfo(Candy.CandyType.Item, 203));
            }
            if (GUILayout.Button("MoneySack"))
            {
                GameManager.Instance.MyGrid.AddItemToQueue(new CandyInfo(Candy.CandyType.Item, 204));
            }
            if (GUILayout.Button("Stone"))
            {
                GameManager.Instance.MyGrid.AddItemToQueue(new CandyInfo(Candy.CandyType.Stone, -1));
            }
            if (GUILayout.Button("Chest"))
            {
                GameManager.Instance.MyGrid.AddItemToQueue(new CandyInfo(Candy.CandyType.Chest, -1));
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("AllHints(10s)"))
            {
                GameManager.Instance.PlaySkillEffect_AllHints(10);
            }
            if (GUILayout.Button("HideGenre(20s)"))
            {
                GameManager.Instance.PlaySkillEffect_HideGenre(20);
            }
            if (GUILayout.Button("Shake(0.05, 5)"))
            {
                UIShake.ShakeAUI(GameUI.Instance.gameObject, 0.05f, 5);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    
}