//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's rotation. Angles are absolute (360° is not 0°)
/// by Stephen Zhou @ 2013-12-31
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class MorlnTweenRotation : UITweener
{
	public Vector3 from;
	public Vector3 to;

	Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }
	public Quaternion rotation { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }

	protected override void OnUpdate (float factor, bool isFinished)
	{
	    cachedTransform.eulerAngles = Vector3.Lerp(from, to, factor);
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public MorlnTweenRotation Begin(GameObject go, float duration, Vector3 rot)
	{
        MorlnTweenRotation comp = UITweener.Begin<MorlnTweenRotation>(go, duration);
		comp.from = comp.rotation.eulerAngles;
		comp.to = rot;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
