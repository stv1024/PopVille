using UnityEngine;

/// <summary>
/// 按钮上的红色文字小提示，有强迫症吧
/// </summary>
public class NoticeTextButton : BaseNoticeButton
{
    private string _text;

    /// <summary>
    /// null则隐藏，""则只有小圆点，"***"啥都可以
    /// </summary>
    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;
            if (_text != null)
            {
                if (!_noticeNumberLabel)
                {
                    ShowIfHidden();
                    _noticeNumberLabel.Text = _text;
                }
            }
            else
            {
                Hide();
            }
        }
    }
}