using System;
using System.Collections;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;
using BindOAuthInfo = Assets.Sanxiao.Communication.UpperPart.BindOAuthInfo;
using BindOAuthInfoOk = Assets.Sanxiao.Communication.UpperPart.BindOAuthInfoOk;

namespace Assets.Sanxiao.UI.Panel
{
    public class BindSnsPanel : BaseTempSingletonPanel, WeiboAuthListener
    {
        #region 单例面板通用

        private static BindSnsPanel _instance;

        public static BindSnsPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 BindSnsPanel instance now!");
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
                var go = Resources.Load("UI/BindSnsPanel") as GameObject;
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

        /// <summary>
        /// 如果是附着在支援面板上的，就填上
        /// </summary>
        public ReinforcePanel ReinforcePanel;

        public GameObject ButtonTemplate;
        public UIGrid GridButton;

        public void OnResult(WeiboAuth.AuthType authType, string code)
        {
            switch (authType)
            {
                case WeiboAuth.AuthType.SinaWeibo:
                    SinaWeibo.Code = code;
                    break;
                case WeiboAuth.AuthType.TecentWeibo:
                    break;
                case WeiboAuth.AuthType.Renren:
                    break;
            }
        }
        public void OnResult(WeiboAuth.AuthType authType, long uid, long expiresIn, string accessToken)
        {
            switch (authType)
            {
                case WeiboAuth.AuthType.SinaWeibo:
                    break;
                case WeiboAuth.AuthType.TecentWeibo:
                    break;
                case WeiboAuth.AuthType.Renren:
                    break;
            }
        }

        public void OnSinaWeiboClick()
        {
            WeiboAuth.AddWeiboAuthListener(this);
            StartCoroutine(SinaWeibo.Bind());
        }
        public static class SinaWeibo
        {
            private const int TYPE = 0;
            public static string Code = null;
            public static string Uid = null;
            public static IEnumerator Bind()
            {
                Code = null;

                //调用SDK
                try
                {
                    var oAuthParamConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.OAuthParamConfig) as OAuthParamConfig;
                    if (oAuthParamConfig != null)
                    {
                        var oAuthParam = oAuthParamConfig.ParamList.Find(x => x.Type == TYPE);
                        if (oAuthParam != null)
                        {
                            WeiboAuth.Auth(oAuthParam.AppKey, oAuthParam.RedirectUrl, WeiboAuth.AuthType.SinaWeibo,
                                           WeiboAuth.AuthResponseType.Code);
                        }
                        else
                        {
                            Debug.LogError("没有微博的OAuth参数，无法绑定");
                        }
                    }
                    else
                    {
                        Debug.LogError("没有OAuthParamConfig，无法绑定社交账号");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                if (Application.platform == RuntimePlatform.WindowsEditor ||
                    Application.platform == RuntimePlatform.OSXEditor)
                {
                    Application.OpenURL("https://api.weibo.com/oauth2/authorize?client_id=3007735796&redirect_uri=http://www.iguandan.com");
                }

                while (true)
                {
                    yield return new WaitForEndOfFrame();
                    if (Code != null) break;
                }

                Requester.Instance.Send(new BindOAuthInfo(TYPE, Code, MainController.DeviceUID) { Uid = Uid });
            }
        }

        public void Execute(BindOAuthInfoOk cmd)
        {
            MorlnFloatingToast.Create("绑定成功");
            if (ReinforcePanel.Instance == null)//我是独立面板时才刷新。如果附着于支援面板，则会立即被它干掉
            {
                OnConfirmClick();
            }
        }

    }
}