using Assets.Sanxiao.Communication;
using Assets.Sanxiao.UI.Panel;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI
{
    public class EntranceUI : BaseUI
    {
        #region 单例UI通用

        private static EntranceUI _instance;

        public static EntranceUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 EntranceUI instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        protected override void Release()
        {
            
        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/EntranceUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static EntranceUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<EntranceUI>(prefab, MainRoot.UIParent);//创建并成为单例
            Instance.OnStage();
            return Instance;
        }

        public override void OffStage()
        {
            Instance = null;//成为孤立
            base.OffStage();
        }

        public static void ExitStage()
        {
            if (Instance)
            {
                Instance.OffStage();
            }
        }

        #endregion

        public UISlider PrgLoading;
        public UIPlayTween PlayTween;

        public void DidLoginOk()
        {
            PlayTween.Play(true);
            SetProgressValue(1);
            PrgLoading.gameObject.SetActive(false);
        }

        void Update()
        {
            //TODO：下载进度条
        }

        private void OnEnterGameClick()
        {
            if (CommonData.FirstTimeGuide)//进入新手教程状态
            {
                FreshmanGuide.StartGuide();
                CommonData.FirstTimeGuide = false;
            }
            else
            {
                MainRoot.Goto(MainRoot.UIStateName.Menu);
            }
        }

        void OnOutLinkClick()
        {
            UMengPlugin.UMengShare("快来陪我玩“爆消农场”吧~", "", "http://www.163.com");
        }

        void On112Click()
        {
            Debug.Log("On112Click");
            UnityClient.Instance.InitAndConnect("112.124.55.102", 61016, Responder.Instance, NetworkManager.Instance);
        }
        void On192Click()
        {
            UnityClient.Instance.InitAndConnect("192.168.0.123", 61016, Responder.Instance, NetworkManager.Instance);
        }

        public void SetProgressValue(float value)
        {
            PrgLoading.value = value;
        }

        public void OnBindSnsClick()
        {
            BindSnsPanel.Load();
        }
    }
}