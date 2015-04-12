using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 按钮上的红色小数字，有强迫症吧
/// </summary>
public abstract class BaseNoticeButton : MonoBehaviour
{

    public Vector3 Position;

    protected NoticeLabel _noticeNumberLabel;

    protected void ShowIfHidden()
    {
        if (!_noticeNumberLabel)
        {
            _noticeNumberLabel = PrefabHelper.InstantiateAndReset<NoticeLabel>(NoticeLabel.Prefab, transform);
            _noticeNumberLabel.transform.localPosition = Position;
        }
    }

    public void Hide()
    {
        if (_noticeNumberLabel) Destroy(_noticeNumberLabel.gameObject);
    }
}