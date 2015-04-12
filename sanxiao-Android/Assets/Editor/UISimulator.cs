using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 服务器模拟器
/// </summary>
public class UISimulator : EditorWindow
{
    [MenuItem("Morln/UISimulator")]
    static void Init()
    {
        var editor = GetWindow<UISimulator>();
        editor.title = "UI模拟器";
    }

    private static int _skillLevel = 1;
    public AudioClip Clip;
    void OnGUI()
    {
        if (GUILayout.Button("StartLoading"))
        {
            LoadingMask.StartLoading();
        }
        if (GUILayout.Button("EndLoading"))
        {
            LoadingMask.EndLoading();
        }
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("EnterMajorLevel"))
            {
                PushLevelUI.Instance.EnterMajorLevel(new MajorLevelUnlockInfo(2, true));
            }
            if (GUILayout.Button("EndChallengeRound"))
            {
                GameManager.Instance.StartCoroutine(GameManager.Instance.EndChallengeRoundCoroutine());
                GameManager.Instance.EndChallengeRound(true);
            }
        }
        EditorGUILayout.EndHorizontal();

        Clip = EditorGUILayout.ObjectField("Button Audio", Clip, typeof(AudioClip), false) as AudioClip;
        if (GUILayout.Button("ChangeAllButtonAudio"))
        {
            if (!Clip)
            {
                Debug.LogError("没Clip");return;
            }
            var allGo = Resources.FindObjectsOfTypeAll<GameObject>();
            
            var count = 0;
            foreach (var g in allGo)
            {
                var allPs = g.GetComponentsInChildren<UIPlaySound>();
                foreach (var uiPlaySound in allPs)
                {
                    if (uiPlaySound.GetComponent<UIButtonColor>() || uiPlaySound.GetComponent<MorlnUIButtonScale>() ||
                        uiPlaySound.GetComponent<UIButtonScale>())
                    {
                        uiPlaySound.audioClip = Clip;
                        EditorUtility.SetDirty(uiPlaySound);
                        count++;
                    }
                }
            }
            Debug.Log("Finish Op:cou:" + count);
        }
    }
}