using System;
using System.Collections.Generic;
using Assets.Sanxiao;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.Game.Skill;
using Assets.Sanxiao.Test;
using Assets.Sanxiao.UI;
using UnityEditor;
using UnityEngine;
using RequestChallengeOk = Assets.Sanxiao.Communication.UpperPart.RequestChallengeOk;
using StartChallenge = Assets.Sanxiao.Communication.UpperPart.StartChallenge;

/// <summary>
/// 服务器模拟器
/// </summary>
public class SkillEffectSimulator : EditorWindow
{
    [MenuItem("Morln/SkillEffectSimulator")]
    public static void Init()
    {
        var editor = GetWindow<SkillEffectSimulator>();
        editor.title = "技能效果测试器";
    }

    void Update()
    {
        Repaint();
    }

    private static int _leftIndex, _rightIndex;
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("先进入Demo场景，然后点击按钮开始测试");
            if (GUILayout.Button("初始化"))
            {
                //GameData.LastChallengeMajorLevelID = cmd.MajorLevelId;
                //GameData.LastChallengeSubLevelID = cmd.SubLevelId;

                //为之后准备好SubLevelData
                var hasCorrectsubLevelData = false;
                var challengeLevelConfig =
                    ConfigManager.GetConfig(ConfigManager.ConfigType.ChallengeLevelConfig) as ChallengeLevelConfig;
                if (challengeLevelConfig != null)
                {
                    var majorLevelData =
                        challengeLevelConfig.MajorLevelList.Find(
                            x => x.MajorLevelId == GameData.LastChallengeMajorLevelID);
                    if (majorLevelData != null)
                    {
                        GameData.LastMajorLevelData = majorLevelData;
                        GameData.LastSubLevelData =
                            majorLevelData.SubLevelList.Find(x => x.SubLevelId == GameData.LastChallengeSubLevelID);
                        hasCorrectsubLevelData = true;
                    }
                }
                if (!hasCorrectsubLevelData)
                {
                    GameData.LastMajorLevelData = null;
                    GameData.LastSubLevelData = null;
                }

                GameData.LastChallengeID = "04F3E163-1286-4657-B743-FAAB42096444";
                GameData.FellowDataList = new List<TeamAdd> {TestData.TeamAdd0, TestData.TeamAdd1};
                GameData.RivalBossData = TestData.DefenseDataEmpty;
                GameData.FriendDataList = new List<TeamAdd> {TestData.TeamAdd0, TestData.TeamAdd1};

                if (CommonData.MyUser != null) CommonData.MyUserOld = CommonData.MyUser.GetDuplicate(); //备份旧的MyUser

                MusicManager.Instance.CrossFadeOut(); //音乐渐出

                CandyBAPool.Instance.PrepareAllCandys(); //预加载所有糖果
                MainRoot.Goto(MainRoot.UIStateName.Game);
                GameManager.Instance.ResetAndRefreshBeforeChallengeFighting(0);
                GameManager.Instance.StartChallengeRound();
            }
        }
        EditorGUILayout.EndHorizontal();
        Time.timeScale = EditorGUILayout.Slider("时间速度", Time.timeScale, 0, 1);
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("左", GUILayout.Width(20));
            _leftIndex = EditorGUILayout.IntSlider(_leftIndex, 0, 2, GUILayout.Width(140));
            EditorGUILayout.LabelField("右", GUILayout.Width(20));
            _rightIndex = EditorGUILayout.IntSlider(_rightIndex, 0, 2, GUILayout.Width(140));
        }
        EditorGUILayout.EndHorizontal();
        var skillEnums = Enum.GetValues(typeof(SkillEnum)) as SkillEnum[];
        foreach (var skillCode in skillEnums)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(skillCode.ToString(), GUILayout.Width(90));
                if (GUILayout.Button("己方施法"))
                {
                    GameManager.Instance.PlayUseSkill(new GameManager.UseSkillInfo(true, _leftIndex, skillCode,
                                                                                   new[] { 200, 50, 60 }));
                }
                if (GUILayout.Button("对方施法"))
                {
                    GameManager.Instance.PlayUseSkill(new GameManager.UseSkillInfo(false, _rightIndex, skillCode,
                                                                                   new[] { 200, 50, 60 }));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}