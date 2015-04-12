using Assets.Sanxiao;
using Assets.Sanxiao.Communication;
using UnityEditor;
using UnityEngine;
using BindOAuthInfo = Assets.Sanxiao.Communication.UpperPart.BindOAuthInfo;

/// <summary>
/// 服务器模拟器
/// </summary>
public class ClientSimulator : EditorWindow
{
    [MenuItem("Morln/ClientSimulator")]
    static void Init()
    {
        var editor = GetWindow<ClientSimulator>();
        editor.title = "客户端模拟器";
    }

    private static string _sinaWeiboCode;
    void OnGUI()
    {
        EditorGUILayout.Foldout(true, "新浪微博");
        EditorGUILayout.BeginHorizontal();
        {
            _sinaWeiboCode = EditorGUILayout.TextField("Code", _sinaWeiboCode);
            if (GUILayout.Button("BindOAuthInfo"))
            {
                Requester.Instance.Send(new BindOAuthInfo(0, _sinaWeiboCode, MainController.DeviceUID));
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}