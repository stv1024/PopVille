using System;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 自己写的Tooltip，Show的文本自行控制换行，不会自动换行。是个单例，确保只有一个并且开局时存在并激活，用不销毁和创建
/// </summary>
public class MorlnTooltip : MonoBehaviour
{
    public static MorlnTooltip Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        Hide();
    }

    private const int PaddingLeft = 25, PaddingTop = 25, PaddingRight = 25, PaddingBottom = 25;

    public UISprite Spr;
    public UILabel Lbl;

    private GameObject _Show(string text, UIWidget.Pivot pivot)
    {
        gameObject.SetActive(true);
        Lbl.pivot = pivot;
        switch (pivot)
        {
            case UIWidget.Pivot.TopLeft:
                Lbl.transform.localPosition = new Vector3(PaddingLeft, -PaddingTop);
                break;
            case UIWidget.Pivot.Top:
                Lbl.transform.localPosition = new Vector3(0, -PaddingTop);
                break;
            case UIWidget.Pivot.TopRight:
                break;
            case UIWidget.Pivot.Left:
                break;
            case UIWidget.Pivot.Center:
                Lbl.transform.localPosition = Vector3.zero;
                break;
            case UIWidget.Pivot.Right:
                break;
            case UIWidget.Pivot.BottomLeft:
                break;
            case UIWidget.Pivot.Bottom:
                break;
            case UIWidget.Pivot.BottomRight:
                break;
        }
        Lbl.text = text;
        Lbl.MakePixelPerfect();

        Spr.pivot = pivot;
        Spr.transform.localPosition = Vector3.zero;
        Spr.width = Lbl.width + PaddingLeft + PaddingRight;
        Spr.height = Lbl.height + PaddingTop + PaddingBottom;
        return gameObject;
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnOutsideTouched()
    {
        Hide();
    }

    public static void ShowAtMousePos(string text)
    {
        if (!Instance) return;
        var go = Instance._Show(text, UIWidget.Pivot.TopLeft);
        go.transform.localPosition = GetMousePos().SetV3Z(0);
    }

    public static void ShowCentered(string text)
    {
        if (!Instance) return;
        var go = Instance._Show(text, UIWidget.Pivot.Center);
        go.transform.localPosition = Vector3.zero;
    }

    public static void Show(string text, Vector2 pos, UIWidget.Pivot pivot = UIWidget.Pivot.TopLeft)
    {
        if (!Instance) return;
        var go = Instance._Show(text, pivot);
        go.transform.localPosition = pos.ToVector3();
    }

    public static bool IsShowing
    {
        get { return Instance && Instance.gameObject.activeSelf; }
    }

    public static void ForceHide()
    {
        if (!Instance) return;
        Instance.Hide();
    }

    static Vector3 GetMousePos()
    {
        Vector2 inPos;
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            inPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            inPos = Input.touches[0].position;
        }
        else
        {
            return Vector3.zero;
        }
        var ray = UICamera.currentCamera.ScreenPointToRay(inPos);
        return ray.origin;
    }
}