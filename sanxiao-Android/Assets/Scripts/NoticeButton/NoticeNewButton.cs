using UnityEngine;

/// <summary>
/// 按钮上的红色新字小提示，有强迫症吧
/// </summary>
public class NoticeNewButton : BaseNoticeButton
{
    private bool _new;

    /// <summary>
    /// 改变小数字，大于0则显示，否则隐藏
    /// </summary>
    public bool New
    {
        get { return _new; }
        set
        {
            _new = value;
            if (_new)
            {
                if (!_noticeNumberLabel)
                {
                    ShowIfHidden();
                    _noticeNumberLabel.Text = "新";
                }
            }
            else
            {
                Hide();
            }
        }
    }
}