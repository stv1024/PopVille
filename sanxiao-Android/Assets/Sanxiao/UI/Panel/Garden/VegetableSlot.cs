using System;
using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Fairwood.Math;
using UnityEngine;
using SpeedUpVegetableUpgrade = Assets.Sanxiao.Communication.UpperPart.SpeedUpVegetableUpgrade;
using UpgradeVegetable = Assets.Sanxiao.Communication.UpperPart.UpgradeVegetable;
using UserVegetable = Assets.Sanxiao.Communication.Proto.UserVegetable;

namespace Assets.Sanxiao.UI.Panel.Garden
{
    /// <summary>
    /// 管理技能面板的技能条目
    /// </summary>
    public class VegetableSlot : MonoBehaviour
    {
        public Transform VegetableArtContentContainer;
        public UILabel LblLevel;
        public UILabel LblDescription;
        public GameObject Shadow;
        public GameObject VegetableArtContent;

        public GameObject BtnUpgrade, BtnPrompt;

        private UserVegetable _userVegetable;

        public void SetAndRefresh(UserVegetable userVegetable)
        {
            _userVegetable = userVegetable;

            Destroy(VegetableArtContent);

            Shadow.SetActive(true);
            VegetableArtContent =
                CandyBAPool.Dequeue(new CandyInfo(Candy.CandyType.Normal,
                                                  VegetableUtil.VegetableCodeToPrefabIndex(_userVegetable.VegetableCode)));
            VegetableArtContent.transform.ResetTransform(VegetableArtContentContainer);

            LblLevel.text = string.Format("{0}级", _userVegetable.CurrentLevel);

            var vegetableConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.VegetableConfig) as VegetableConfig;
            if (vegetableConfig != null)
            {
                var vegetable = vegetableConfig.VegetableList.Find(x => x.VegetableCode == _userVegetable.VegetableCode);
                if (vegetable != null)
                {
                    LblDescription.text = "能量 " + vegetable.LevelEnergyList[_userVegetable.CurrentLevel - 1];
                }
            }

            var matureInfo = CommonData.MyVegetableMatureInfoList.Find(x => x.Code == _userVegetable.VegetableCode);
            BtnUpgrade.SetActive(_userVegetable.CurrentLevel < _userVegetable.CurrentUpgradeLimit && matureInfo == null);
            if (matureInfo == null)
            {
                _dateTimeMature = null;
                BtnPrompt.SetActive(false);
            }
            else
            {
                _dateTimeMature = DateTime.Now +
                                  TimeSpan.FromSeconds(Mathf.Max(0, matureInfo.MatureTime - Time.realtimeSinceStartup));
                BtnPrompt.SetActive(true);
            }
        }

        private DateTime? _dateTimeMature;
        private int _lastTotalSeconds;
        void Update()
        {
            if (_dateTimeMature != null)
            {
                var ts = Mathf.CeilToInt((float) ((DateTime) _dateTimeMature - DateTime.Now).TotalSeconds);
                if (ts != _lastTotalSeconds)
                {
                    _lastTotalSeconds = ts;
                    if (ts >= 3600)//有小时数
                    {
                        LblDescription.text = string.Format("成熟需{0:0}:{1:00}:{2:00}", ts / 3600, ts % 3600 / 60, ts % 60);
                    }
                    else
                    {
                        LblDescription.text = string.Format("成熟需{0:0}:{1:00}", ts % 3600 / 60, ts % 60);
                    }
                }
            }
        }

        public void OnUpgradeClick()
        {
            UMengPlugin.UMengEvent(EventId.VEGETABLE_UPGRADE, new Dictionary<string, object> { { "code", _userVegetable.VegetableCode }, { "level", _userVegetable.CurrentLevel } });

            Requester.Instance.Send(new UpgradeVegetable(_userVegetable.VegetableCode));
        }

        public void OnDetailClick()
        {
            UMengPlugin.UMengEvent(EventId.VEGETABLE_EXPLAIN, new Dictionary<string, object> { { "code", _userVegetable.VegetableCode }, { "level", _userVegetable.CurrentLevel } });

            //这个复杂了，每个技能的介绍或许还不一样
            var vegetableIntroTextConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.VegetableIntroTextConfig) as VegetableIntroTextConfig;
            if (vegetableIntroTextConfig == null)
            {
                Debug.LogError("没有VegetableIntroTextConfig");
                return;
            }
            var vegetableIntro =
                vegetableIntroTextConfig.IntroList.Find(x => x.VegetableCode == _userVegetable.VegetableCode);
            if (vegetableIntro == null)
            {
                Debug.LogError("没有vegetableIntro.Code:" + _userVegetable.VegetableCode);
                return;
            }
            MorlnTooltip.Show(vegetableIntro.IntroContent, MainRoot.InverseTransformPoint(VegetableArtContentContainer.position));
        }

        public void Prompt()
        {
            UMengPlugin.UMengEvent(EventId.VEGETABLE_ACCELERATE, new Dictionary<string, object> { { "code", _userVegetable.VegetableCode }, { "level", _userVegetable.CurrentLevel } });

            if (_userVegetable != null)
            {
                AlertDialog.Load("确定使用" + new Currency(1, 1).GetCurrencyLabelWithIcon() + "立即完成培育吗", "确认",
                                 () => Requester.Instance.Send(
                                     new SpeedUpVegetableUpgrade(_userVegetable.VegetableCode)), "不了", null,
                                 true);
            }
        }

        public Animation UpgradeAnim;
        public void PlayUpgradeEffect()
        {
            UpgradeAnim.Play();
        }
    }
}