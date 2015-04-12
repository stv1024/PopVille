using UnityEngine;

/// <summary>
/// 按钮上的红色小数字，有强迫症吧
/// </summary>
public class NoticeNumberButton : BaseNoticeButton
{
    ///// <summary>
    ///// 空则不保存
    ///// </summary>
    //public string NoticeKey;

    //private string PrefKey
    //{
    //    get { return "NoticeNumber:" + NoticeKey; }
    //}

    private void Start()
    {
        //if (!string.IsNullOrEmpty(NoticeKey))
        //{
        //    Number = PlayerPrefs.GetInt(PrefKey, 0);
        //}
    }

    private int _number;

    /// <summary>
    /// 改变小数字，大于0则显示，否则隐藏
    /// </summary>
    public int Number
    {
        get { return _number; }
        set
        {
            _number = value;
            //if (!string.IsNullOrEmpty(NoticeKey)){ PlayerPrefs.SetInt(PrefKey, _number);}
            if (_number > 0)
            {
                ShowIfHidden();
                _noticeNumberLabel.Text = _number.ToString();
            }
            else
            {
                Hide();
            }
        }
    }
}