using System.Linq;
using Assets.Sanxiao;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 服务器模拟器
/// </summary>
public class CommonDataWatcher : EditorWindow
{
    [MenuItem("Morln/CommonDataWatcher")]
    static void Init()
    {
        var editor = GetWindow<CommonDataWatcher>();
        editor.title = "CommonData数据监视器";
    }

    static bool _showTextConfig;

    static bool _showMyUser;
    static bool _showMyUserOld;
    static bool _showRivalUser;
    static bool _showRivalUserOld;

    private static bool _showMyCharacterList;

    private void OnGUI()
    {
        _showTextConfig = EditorGUILayout.Foldout(_showTextConfig, "TextConfig");
        if (_showTextConfig) ShowTextConfig();

        _showMyUser = EditorGUILayout.Foldout(_showMyUser, "MyUser");
        if (_showMyUser) ShowUser(CommonData.MyUser);
        _showMyUserOld = EditorGUILayout.Foldout(_showMyUserOld, "MyUserOld");
        if (_showMyUserOld) ShowUser(CommonData.MyUserOld);
        _showRivalUser = EditorGUILayout.Foldout(_showRivalUser, "RivalUser");
        if (_showRivalUser) ShowUser(CommonData.RivalUser);
        _showRivalUserOld = EditorGUILayout.Foldout(_showRivalUserOld, "RivalUserOld");
        if (_showRivalUserOld) ShowUser(CommonData.RivalUserOld);

        _showMyCharacterList = EditorGUILayout.Foldout(_showMyCharacterList, "MyCharacterList");
        if (_showMyCharacterList)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical();
                {
                    foreach (var userCharacter in CommonData.MyCharacterList)
                    {
                        //EditorGUILayout.BeginHorizontal();
                        //{
                        //    userCharacter.CharacterCode
                        //}
                        //EditorGUILayout.EndHorizontal();
                        EditorGUILayout.LabelField(userCharacter.CharacterCode + ":" +
                                                   userCharacter.WearEquipList.Select(x => x.ToString())
                                                                .Aggregate((c, x) => c + "," + x));
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LabelField("HeartData");
        EditorGUILayout.LabelField("Count", CommonData.HeartData.Count.ToString());
        EditorGUILayout.LabelField("NextHeartRealTime", CommonData.HeartData.NextHeartRealTime.ToString());
    }

    static void ShowUser(User curUser)
    {
        EditorGUILayout.LabelField("UserId", curUser == null ? "null" : curUser.UserId);
        EditorGUILayout.LabelField("Nickname", curUser == null ? "null" : curUser.Nickname);
        EditorGUILayout.LabelField("Level", curUser == null ? "null" : curUser.Level.ToString());
    }

    static bool _showSkillIntro, _showWaitHintText;
    static void ShowTextConfig()
    {
        _showSkillIntro = EditorGUILayout.Foldout(_showSkillIntro, "SkillIntroText");
        //if (_showSkillIntro && ConfigManager.SkillIntroText != null)
        //{
        //    EditorGUILayout.LabelField("Hash", ConfigManager.SkillIntroText.Hash);
        //    if (ConfigManager.SkillIntroText.SkillCodeList.Count > 0) EditorGUILayout.LabelField(ConfigManager.SkillIntroText.SkillCodeList.Select(x => x.ToString()).Aggregate("", (c, x) => c + x + ","));
        //    EditorGUILayout.LabelField(ConfigManager.SkillIntroText.DisplayNameList.Aggregate("", (c, x) => c + x + ","));
        //    EditorGUILayout.LabelField(ConfigManager.SkillIntroText.IntroList.Aggregate("", (c, x) => c + x + ","));
        //}

        //_showWaitHintText = EditorGUILayout.Foldout(_showWaitHintText, "WaitHintText");
        //if (_showWaitHintText && ConfigManager.WaitHintText != null)
        //{
        //    EditorGUILayout.LabelField("Hash", ConfigManager.WaitHintText.Hash);
        //    EditorGUILayout.LabelField(ConfigManager.WaitHintText.HintList.Aggregate((c, x) => c + x + ","));
        //}
    }
}