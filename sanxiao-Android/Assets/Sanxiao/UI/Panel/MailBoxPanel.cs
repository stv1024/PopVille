using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.UI.Panel.MailBox;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class MailBoxPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static MailBoxPanel _instance;

        public static MailBoxPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 MailBoxPanel instance now!");
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
                var go = Resources.Load("UI/MailBoxPanel") as GameObject;
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

        protected override void Initialize()
        {
            BtnUnread.UpdateColor(false, true);
            BtnRead.UpdateColor(true, true);
            TransitionToUnread();
        }

        public UIGrid Grid;
        public GameObject MailSlotTemplate;
        readonly List<MailSlot> _mailSlots = new List<MailSlot>();

        public GameObject[] SubPanels;
        public UIButton BtnUnread, BtnRead;
        private int _curView;

        public void TransitionToUnread()
        {
            _curView = 0;
            BtnUnread.isEnabled = false;
            BtnRead.isEnabled = true;
            if (CommonData.MailList == null)
            {
                Refresh(null);
            }
            else
            {
                var unreadList = CommonData.MailList.Where(x => !x.IsRead).ToList();
                Refresh(unreadList);
            }
        }
        public void TransitionToRead()
        {
            _curView = 1;
            BtnUnread.isEnabled = true;
            BtnRead.isEnabled = false;
            if (CommonData.MailList == null)
            {
                Refresh(null);
            }
            else
            {
                var readList = CommonData.MailList.Where(x => x.IsRead).ToList();
                Refresh(readList);
            }
        }
        public void Refresh(List<Mail> mailList)
        {
            if (CommonData.MailList == null)
            {
                foreach (var mailSlot in _mailSlots)
                {
                    mailSlot.gameObject.SetActive(false);
                }
            }
            else
            {
                while (_mailSlots.Count < mailList.Count)
                {
                    var mailSlot = PrefabHelper.InstantiateAndReset<MailSlot>(MailSlotTemplate, Grid.transform);
                    mailSlot.name = "Slot" + _mailSlots.Count;
                    _mailSlots.Add(mailSlot);
                }
                for (int i = 0; i < _mailSlots.Count; i++)
                {
                    if (i < mailList.Count)
                    {
                        _mailSlots[i].gameObject.SetActive(true);
                        _mailSlots[i].SetAndRefresh(mailList[i]);
                    }
                    else
                    {
                        _mailSlots[i].gameObject.SetActive(false);
                    }
                }
            }
            Grid.repositionNow = true;
            MailSlotTemplate.SetActive(false);
        }

        public void Execute(UserMailList cmd)
        {
            if (_curView == 0) TransitionToUnread();
            else TransitionToRead();
        }
    }
}