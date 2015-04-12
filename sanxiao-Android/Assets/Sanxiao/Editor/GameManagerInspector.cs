using System.Linq;
using Assets.Sanxiao.Game;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var gameManager = target as GameManager;
            if (!gameManager) return;

            string str;

            str = gameManager._mySuspendingSkillList.Count > 0
                      ? gameManager._mySuspendingSkillList.Select(x => x.SkillCode.ToString())
                                   .Aggregate((c, x) => c + x + ",")
                      : "";
            EditorGUILayout.LabelField("_mySuspendingSkillList", str);

            str = gameManager._myOverdueSkillList.Count > 0
                      ? gameManager._myOverdueSkillList.Select(x => x.SkillCode.ToString()).Aggregate((c, x) => c + x + ","): "";
            EditorGUILayout.LabelField("_myOverdueSkillList", str);

            str = gameManager._playingSkillList.Count > 0
                      ? gameManager._playingSkillList.Select(x => x.SkillCode.ToString()).Aggregate((c, x) => c + x + ",") : "";
            EditorGUILayout.LabelField("_myPlayingSkillList", str);

            DrawDefaultInspector();

            
        }
    }
}