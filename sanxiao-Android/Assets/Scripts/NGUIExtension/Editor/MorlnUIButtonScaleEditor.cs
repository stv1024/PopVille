//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using Fairwood.Math;

/// <summary>
/// Writen by Stephen Zhou @ 2013-12-23
/// </summary>
[CustomEditor(typeof(MorlnUIButtonScale))]
public class MorlnUIButtonScaleEditor : UIWidgetContainerEditor
{
	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(80f);
        MorlnUIButtonScale button = target as MorlnUIButtonScale;

		GUILayout.Space(6f);

		GUI.changed = false;
		Transform tt = (Transform)EditorGUILayout.ObjectField("Target", button.tweenTarget, typeof(Transform), true);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Button Change", button);
			button.tweenTarget = tt;
			UnityEditor.EditorUtility.SetDirty(button);
		}

		if (tt != null)
		{
			UIWidget w = tt.GetComponent<UIWidget>();

			if (w != null)
			{
				GUI.changed = false;
				Color c = EditorGUILayout.ColorField("Normal", w.color);

				if (GUI.changed)
				{
					NGUIEditorTools.RegisterUndo("Button Change", w);
					w.color = c;
					UnityEditor.EditorUtility.SetDirty(w);
				}
			}
		}

		GUI.changed = false;
		Vector3 hover = EditorGUILayout.Vector2Field("Hover", button.hover).ToVector3(1);
		Vector3 pressed = EditorGUILayout.Vector2Field("Pressed", button.pressed).ToVector3(1);

		GUILayout.BeginHorizontal();
		float duration = EditorGUILayout.FloatField("Duration", button.duration, GUILayout.Width(120f));
		GUILayout.Label("seconds");
		GUILayout.EndHorizontal();

		GUILayout.Space(3f);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Button Change", button);
			button.hover = hover;
			button.pressed = pressed;
		    button.duration = duration;
			button.duration = duration;
			UnityEditor.EditorUtility.SetDirty(button);
		}
		NGUIEditorTools.DrawEvents("On Click", button, button.onClick);
	}
}
