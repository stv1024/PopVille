using Fairwood.Math;
using UnityEngine;

public class NoticeLabel : MonoBehaviour
{
    public UISprite SlcPad;
    public UILabel Label;

    /// <summary>
    /// 不允许换行
    /// </summary>
    public string Text
    {
        get { return Label.text; }
        set
        {
            Label.text = value;
            ResizePad();
        }
    }

    void ResizePad()
    {
        if (!SlcPad) return;
        var lblWidth = Label.enabled ? Label.width*Label.transform.localScale.x : 0;
        SlcPad.width = (int) Mathf.Max(26, lblWidth + 15);//最小26是圆形
    }


    private static GameObject _prefab;
    public static GameObject Prefab
    {
        get
        {
            if (!_prefab)
            {
                _prefab = Resources.Load("UI/Notice Label") as GameObject;
            }
            return _prefab;
        }
    }
}