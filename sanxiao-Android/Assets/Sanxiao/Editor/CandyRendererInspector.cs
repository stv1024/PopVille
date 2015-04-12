using Assets.Sanxiao.Game;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    [CustomEditor(typeof(CandyRenderer),true)]
    public class CandyRendererInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var cr = target as CandyRenderer;

            DrawDefaultInspector();

            if (GUILayout.Button("Blink"))
            {
                cr.Blink();
            }
            if (GUILayout.Button("Exchange"))
            {
                cr.Exchange();
            }
            if (GUILayout.Button("Fall"))
            {
                cr.HasSupport = false;
            }
            if (GUILayout.Button("Normal"))
            {
                cr.Reset();
            }
            if (GUILayout.Button("Pop"))
            {
                cr.Pop();
            }
            if (GUILayout.Button("Exchange with Tween"))
            {
                var go = cr.Candy.gameObject;
                TweenPosition.Begin(go, 0.45f, go.transform.localPosition + Vector3.right*75);
                cr.Exchange();
            }
        }
    }
}