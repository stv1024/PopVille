using UnityEngine;

/// <summary>
/// 飘起的Label，基于NGUI
/// </summary>
public class FloatingLabel : MonoBehaviour
{
    private UILabel _uiLabel;
    //private TweenAlpha _tweenAlpha;
    //private TweenPosition _tweenPosition;

    void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        _uiLabel = GetComponentInChildren<UILabel>();
        //_tweenAlpha = GetComponentInChildren<TweenAlpha>();
        //_tweenPosition = GetComponentInChildren<TweenPosition>();
    }

    public void Reset(string text, float lifespan = 1)
    {
        if (!_uiLabel) Initialize();

        _uiLabel.text = text;

        if (lifespan > 0) Destroy(gameObject, lifespan);
    }
}