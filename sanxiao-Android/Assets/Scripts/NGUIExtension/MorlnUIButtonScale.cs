//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Writen by Stephen Zhou @ 2013-12-23。改进的ButtonScale，按钮不用变色时，取代UIButton。还有onClick事件列表，很好用。自带编辑器。
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Scale(Better)")]
public class MorlnUIButtonScale : MonoBehaviour
{
    /// <summary>
    /// Current button that sent out the onClick event.
    /// </summary>

    static public MorlnUIButtonScale current;

	public Transform tweenTarget;
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1f);
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1f);
	public float duration = 0.2f;


    /// <summary>
    /// Click event listener.
    /// </summary>

    public List<EventDelegate> onClick = new List<EventDelegate>();

	Vector3 mScale;
	bool mStarted = false;
	bool mHighlighted = false;

	void Start ()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (tweenTarget == null) tweenTarget = transform;
			mScale = tweenTarget.localScale;
		}
	}

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnDisable ()
	{
		if (mStarted && tweenTarget != null)
		{
			TweenScale tc = tweenTarget.GetComponent<TweenScale>();

			if (tc != null)
			{
				tc.scale = mScale;
				tc.enabled = false;
			}
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (!mStarted) Start();
		    AnimationCurve oldCurve = null;
		    var oldTS = tweenTarget.GetComponent<TweenScale>();
		    if (oldTS) oldCurve = oldTS.animationCurve;
			var newTS = TweenScale.Begin(tweenTarget.gameObject, duration, isPressed ? Vector3.Scale(mScale, pressed) :
				(UICamera.IsHighlighted(gameObject) ? Vector3.Scale(mScale, hover) : mScale));
		    if (oldCurve != null) newTS.animationCurve = oldCurve;
		}
	}

	void OnHover (bool isOver)
	{
		if (enabled)
		{
            if (!mStarted) Start();
            AnimationCurve oldCurve = null;
            var oldTS = tweenTarget.GetComponent<TweenScale>();
            if (oldTS) oldCurve = oldTS.animationCurve;
            var newTS = TweenScale.Begin(tweenTarget.gameObject, duration, isOver ? Vector3.Scale(mScale, hover) : mScale);
            if (oldCurve != null) newTS.animationCurve = oldCurve;
			mHighlighted = isOver;
		}
	}

    /// <summary>
    /// Call the listener function.
    /// </summary>

    void OnClick()
    {
        if (isEnabled)
        {
            current = this;
            EventDelegate.Execute(onClick);
            current = null;
        }
    }

    /// <summary>
    /// Whether the button should be enabled.
    /// </summary>

    public bool isEnabled
    {
        get
        {
            if (!enabled) return false;
            Collider col = collider;
            return col && col.enabled;
        }
        set
        {
            Collider col = collider;
            if (col != null) col.enabled = value;
            enabled = value;
        }
    }
}
