using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.UpperPart;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    /// <summary>
    /// 选择昵称面板
    /// </summary>
    public class SelectNicknamePanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static SelectNicknamePanel _instance;

        public static SelectNicknamePanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 SelectNicknamePanel instance now!");
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
                var go = Resources.Load("UI/SelectNicknamePanel") as GameObject;
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

        public static Queue<string> NicknameQueue = new Queue<string>();
        /// <summary>
        /// TODO:修改成功后这里设为true
        /// </summary>
        public static bool NicknameSelectOk = false;
        public UIInput IptNickname;
        public static int RandomTimes;
        protected override void Initialize()
        {
            RandomNickname();
            RandomTimes = 0;
        }
        public void RandomNickname()
        {
            RandomTimes++;
            if (NicknameQueue.Count > 0)
            {
                var newNickname = NicknameQueue.Dequeue();
                IptNickname.value = newNickname;
            }
            else
            {
                IptNickname.value = "";
            }
            if (NicknameQueue.Count < 3)
            {
                Requester.Instance.Send(new RequestNickname());//获取随机昵称
            }
        }
        public void Confirm()
        {
            var curNickname = IptNickname.value;
            if (string.IsNullOrEmpty(curNickname))
            {
                MorlnTooltip.Show("昵称不能空",
                                  IptNickname.transform.position +
                                  new Vector3(102, 10));
                return;
            }
            Requester.Instance.Send(new EditUserInfo {NewNickname = curNickname});
        }
        public void FinishEditIuput()
        {
            UMengPlugin.UMengEvent(EventId.NICKNAME_EDIT,
                                   new Dictionary<string, object> {{"name_length", IptNickname.text.Length}});
        }
    }
}