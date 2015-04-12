using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao
{
    [CustomEditor(typeof(Character))]
    public class CharacterInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var character = target as Character;
            if (!character) return;
            DrawDefaultInspector();

            if (GUILayout.Button("Attack"))
            {
                character.Attack(0, 1);
                //character.Attack(Random.Range(0, 3), Random.Range(0, 3));
            }
            if (GUILayout.Button("BeMagicallyAttacked"))
            {
                character.BeMagicallyAttacked();
            }
            if (GUILayout.Button("BeAttacked"))
            {
                character.BeAttacked();
            }
            if (GUILayout.Button("Die"))
            {
                character.Die();
            }
            if (GUILayout.Button("Reset"))
            {
                character.Reset();
            }
        }
    }
}