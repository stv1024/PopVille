using Assets.Sanxiao.Data;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class SettingsPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static SettingsPanel _instance;

        public static SettingsPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 SettingsPanel instance now!");
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
                var go = Resources.Load("UI/SettingsPanel") as GameObject;
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

        public UILabel LblSoftwareVersion, LblResourceVersion, LblDeviceID;
        protected override void Initialize()
        {
            TogMusic.value = SystemSettings.MusicOn;
            TogAudio.value = SystemSettings.AudioOn;

            LblSoftwareVersion.text = "软件版本：" + ClientInfoHolder.Instance.SoftwareVersion;
            LblResourceVersion.text = "资源版本：0.0121";
            LblDeviceID.text = "设备ID：" + MainController.DeviceUID;
        }

        public void OnSuggestClick()
        {
            //友盟投诉建议SDK
            UMengPlugin.UMengFeedback();
        }

        public void OnWeiboClick()
        {
            //TODO:WebView.
        }

        public void OnUpdateClick()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.OpenURL("itms-services://?action=download-manifest&url=http://bcs.duapp.com/sanxiao/testiap.plist");
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL("http://bcs.duapp.com/sanxiao/androidtest.apk");
            }
        }

        public void OnReLoginClick()
        {
            if (_hasChanged) SystemSettings.SaveSettingsToHD();
            OnConfirmClick();
            MainController.Instance.ReLogin();
        }

        public void OnClearDownloadContentClick()
        {
            if (_hasChanged) SystemSettings.SaveSettingsToHD();
            OnConfirmClick();
            ConfigManager.DeleteAll();
            MorlnFloatingToast.Create("清除成功");
            MainController.Instance.ReLogin();
        }

        void OnExitClick()
        {
            if (_hasChanged) SystemSettings.SaveSettingsToHD();
            OnConfirmClick();
            MainRoot.Goto(MainRoot.UIStateName.Entrance);
        }

        private bool _hasChanged;
        public UIToggle TogMusic;
        public void OnMusicChange()
        {
            if (MusicManager.MusicOn == TogMusic.value) return;
            MusicManager.MusicOn = TogMusic.value;
            _hasChanged = true;
        }
        public UIToggle TogAudio;
        public void OnAudioChange()
        {
            if (AudioManager.AudioOn == TogAudio.value) return;
            AudioManager.AudioOn = TogAudio.value;
            _hasChanged = true;
        }
    }
}