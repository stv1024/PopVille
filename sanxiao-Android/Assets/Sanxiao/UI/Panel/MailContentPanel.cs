using System.Linq;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using UnityEngine;
using RequestMailGift = Assets.Sanxiao.Communication.UpperPart.RequestMailGift;

namespace Assets.Sanxiao.UI.Panel
{
    public class MailContentPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static MailContentPanel _instance;

        public static MailContentPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 MailContentPanel instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/MailContentPanel") as GameObject;
                return go;
            }
        }

        public static void Load()
        {
            if (Instance)
            {
                MainRoot.FocusPanel(Instance);
            }
            else
            {
                if (!Prefab) return;
                MainRoot.ShowPanel(Prefab);
            }
            if (Instance) Instance.Initialize();
        }

        public static void UnloadInterface()
        {
            if (Instance) Instance.OnConfirmClick();
        }

        #endregion

        private Mail _mail;
        public UILabel LblTitle, LblFrom, LblContent;
        public void Refresh(Mail mail)
        {
            _mail = mail;
            LblTitle.text = _mail.HasTitle ? _mail.Title : null;
            LblFrom.text = _mail.FromNickname;
            LblContent.text = _mail.HasContent ? _mail.Content : null;

            //TODO:奖品
        }

        public void ClaimGift()
        {
            Requester.Instance.Send(new RequestMailGift(_mail.MailId){SnList = _mail.GiftList.Select(x=>x.Sn).ToList()});
        }
    }
}