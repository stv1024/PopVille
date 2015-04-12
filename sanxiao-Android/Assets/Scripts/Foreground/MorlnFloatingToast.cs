using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 自己写的Tooltip，Show的文本自行控制换行，不会自动换行。是个单例，确保只有一个并且开局时存在并激活，用不销毁和创建
/// </summary>
public class MorlnFloatingToast : MonoBehaviour
{
    public UISprite Spr;
    public UILabel Lbl;
    void Awake()
    {
        Destroy(gameObject, 5);
    }
    public void SetText(string text)
    {
        Lbl.text = text;
        var labelSize = Lbl.printedSize;
        labelSize.Scale(Lbl.transform.localScale);
        Spr.width = (int) (labelSize.x + 60);
        Spr.height = (int) (labelSize.y + 60);
    }

    /// <summary>
    /// 与直接调用MorlnFloatingToastContainer.Create(text)一样
    /// </summary>
    /// <param name="text"></param>
    public static void Create(string text)
    {
        MorlnFloatingToastContainer.Create(text);
    }
}