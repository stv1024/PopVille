//----------------------------------------------
//  Edited by Stephen Zhou @2013-12-9
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Similar to UIImageButton, but only care disable state.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button")]
public class MorlnUIImageButton : MonoBehaviour
{
    public UISprite target;
    public string normalSprite;
    public string disabledSprite;

    public bool isEnabled
    {
        get
        {
            Collider col = GetComponent<Collider>();
            return col && col.enabled;
        }
        set
        {
            Collider col = GetComponent<Collider>();
            if (!col) return;

            if (col.enabled != value)
            {
                col.enabled = value;
                UpdateImage();
            }
        }
    }

    void OnEnable()
    {
        if (target == null) target = GetComponentInChildren<UISprite>();
        UpdateImage();
    }

    void UpdateImage()
    {
        if (target != null)
        {
            if (isEnabled)
            {
                target.spriteName = normalSprite;
            }
            else
            {
                target.spriteName = disabledSprite;
            }
            target.MakePixelPerfect();
        }
    }
}
