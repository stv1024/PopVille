using Assets.Sanxiao.Game;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    [CustomEditor(typeof(AttackAvatar))]
    public class AttackAvatarInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var candy = target as AttackAvatar;
            if (!candy) return;
            DrawDefaultInspector();
            EditorGUILayout.LabelField("Prefab里应该没有角色内容");
            EditorGUILayout.LabelField("点击下面的按钮测试");
            if (GUILayout.Button("鸡"))
            {
                var prefab = Resources.Load<GameObject>("ResourcesForDownload/Character/Character90001");
                candy.Initialize(prefab, false);
            }
            if (GUILayout.Button("猪"))
            {
                var prefab = Resources.Load<GameObject>("ResourcesForDownload/Character/Character90002");
                candy.Initialize(prefab, true);
            }
        }
    }
}