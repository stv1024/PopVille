using Assets.Sanxiao.Game;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    [CustomEditor(typeof(Candy))]
    public class CandyInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var candy = target as Candy;
            if (!candy) return;
            DrawDefaultInspector();

            var type = (Candy.CandyType)EditorGUILayout.EnumPopup("Type", candy.MyInfo.Type);
            if (type != candy.MyType) candy.Set(type, candy.Genre);
            var genre = EditorGUILayout.IntField("Genre", candy.MyInfo.Genre);
            if (genre != candy.Genre) candy.Set(candy.MyType, genre);


            EditorGUILayout.Toggle("Fired", candy.Fired);
            candy.MyInfo.FiredCandyExtraData = EditorGUILayout.IntField("FiredCandyExtraData", candy.MyInfo.FiredCandyExtraData);

            EditorGUILayout.LabelField("CurIJ", candy.CurIJ.ToString());
            EditorGUILayout.LabelField("FromIJ", candy.FromIJ.ToString());

            if (GUILayout.Button("Refresh"))
            {
                candy.Refresh();
            }
        }
    }
}