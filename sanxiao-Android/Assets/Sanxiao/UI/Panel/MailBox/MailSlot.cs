using System;
using System.Linq;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel.MailBox
{
    /// <summary>
    /// 邮件标题
    /// </summary>
    public class MailSlot : MonoBehaviour
    {
        public UILabel LblTitle, LblFrom, LblTime;
        public UILabel LblState;
        public UISprite SprNew;

        private Mail _mail;

        public void SetAndRefresh(Mail mail)
        {
            _mail = mail;
            LblTitle.text = _mail.HasTitle ? _mail.Title : null;
            LblFrom.text = _mail.FromNickname;
            LblTime.text = GetCountDownTimeLabel(DateTime.Now - new DateTime(_mail.Timestamp));
            if (!_mail.IsRead)
            {
                LblState.text = "未读";
                LblState.color = new Color32(111, 255, 67, 255);
                SprNew.enabled = true;
            }
            else
            {
                if (_mail.GiftList.Any(x => !x.IsObtained))
                {
                    LblState.text = "未领奖";
                    LblState.color = new Color32(255, 146, 73, 255);
                }
                else
                {
                    LblState.text = "已读";
                    LblState.color = new Color32(255, 146, 73, 255);
                }
                SprNew.enabled = false;
            }
        }

        static string GetCountDownTimeLabel(TimeSpan timeSpan)
        {
            if (timeSpan.TotalMinutes < 1)
            {
                return string.Format("{0}秒前", timeSpan.Seconds);
            }
            if (timeSpan.TotalHours < 1)
            {
                return string.Format("{0}分前", timeSpan.Minutes);
            }
            if (timeSpan.TotalDays < 1)
            {
                return string.Format("{0}小时前", timeSpan.Hours);
            }
            if (timeSpan.TotalDays < 1)
            {
                return string.Format("{0}小时前", timeSpan.Hours);
            }
            return null;
        }

        void OnClick()
        {
            //显示邮件内容
            MailContentPanel.Load();
            MailContentPanel.Instance.Refresh(_mail);

            if (!_mail.IsRead)
            {
                //Requester.Instance.Send(new ); TODO:设置为已读
            }
        }
    }
}