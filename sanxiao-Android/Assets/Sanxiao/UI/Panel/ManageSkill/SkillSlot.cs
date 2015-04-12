using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using UnityEngine;
using UpgradeSkill = Assets.Sanxiao.Communication.UpperPart.UpgradeSkill;

namespace Assets.Sanxiao.UI.Panel.ManageSkill
{
    /// <summary>
    /// 管理技能面板的技能条目
    /// </summary>
    public class SkillSlot : MonoBehaviour
    {
        public UISprite SprSkillIcon;

        public UILabel LblSkillLevel, LblDisplayName, LblCostPerUse;

        public UILabel LblIntro;
        public UILabel LblOnButton, LblLockLevelLimit;
        public GameObject GrpUnlocked;
        public GameObject GrpLocked;


        public MorlnUIButtonScale BtnUpgrade;

        private Skill _skill;

        private int _level;

        public void SetAndRefresh(Skill skill)
        {
            _skill = skill;

            var mySkillInfo = CommonData.MySkillList.Find(x => x.Code == _skill.SkillCode);
            _level = mySkillInfo == null ? 0 : mySkillInfo.Level;

            if (_level > 0)
            {
                LblSkillLevel.text = string.Format("{0}段", _level);
                SprSkillIcon.color = Color.white;
            }
            else
            {
                LblSkillLevel.text = "未解锁";
                SprSkillIcon.color = Color.white*0.6f;
            }

            if (_level < _skill.UnlockLevelList.Count)
            {
                var canUpgradeNow = _skill.UnlockLevelList[_level] <= CommonData.MyUser.Level;
                if (canUpgradeNow)
                {
                    GrpUnlocked.SetActive(true);
                    GrpLocked.SetActive(false);
                    //LblOnButton.text = string.Format("升级到{0}级", _level + 1);
                    LblOnButton.text = string.Format("升级");
                    //LblOnButton.text = _skill.UpgradeCostList[_level].GetCurrencyLabelWithIcon(true);
                }
                else
                {
                    GrpUnlocked.SetActive(false);
                    GrpLocked.SetActive(true);
                    LblLockLevelLimit.text = "" + _skill.UnlockLevelList[_level] + "级" + (_level == 0 ? "解锁" : "升级");
                    //LblLockLevelLimit.text = _level == 0
                    //                       ? string.Format("解锁需要等级{0}", _skill.UnlockLevelList[_level])
                    //                       : "升级需要等级" + _skill.UnlockLevelList[_level];
                }
                BtnUpgrade.isEnabled = true;
            }
            else
            {
                LblOnButton.text = "已达到顶级";
                BtnUpgrade.isEnabled = false;
            }

            var energyPerUse = SkillUtil.GetSkillUseEnergy(_skill);
            LblCostPerUse.text = energyPerUse + "攻击";
            var spriteName = string.Format("skillicon-{0}", (SkillEnum) skill.SkillCode);
            if (SprSkillIcon.atlas.GetSprite(spriteName) != null)
            {
                SprSkillIcon.spriteName = spriteName;
                SprSkillIcon.enabled = true;
                SprSkillIcon.MakePixelPerfect();
            }
            else
            {
                SprSkillIcon.enabled = false;
            }

            var skillIntroTextConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.SkillIntroTextConfig) as SkillIntroTextConfig;
            if (skillIntroTextConfig != null && skillIntroTextConfig.SkillCodeList != null)
            {

                var index = skillIntroTextConfig.SkillCodeList.FindIndex(x => x == skill.SkillCode);
                if (index >= 0)
                {
                    if (index < skillIntroTextConfig.DisplayNameList.Count)
                    {
                        LblDisplayName.text = skillIntroTextConfig.DisplayNameList[index];
                    }
                    if (index < skillIntroTextConfig.IntroList.Count)
                    {
                        LblIntro.text = skillIntroTextConfig.IntroList[index];
                    }
                }
            }
        }

        void OnUpgradeClick()
        {
            var canUpgradeNow = _skill.UnlockLevelList[_level] <= CommonData.MyUser.Level;

            UMengPlugin.UMengEvent(EventId.SKILL_UPGRADE,
                                   new Dictionary<string, object>
                                       {
                                           {"code", _skill.SkillCode},
                                           {"level", _level},
                                           {"can_upgrade", canUpgradeNow}
                                       });

            if (canUpgradeNow)
            {
                var cost = _skill.UpgradeCostList[_level];
                var afford = cost.DoIAfford();
                var costLabel = string.Format(afford ? "{0}" : "[FF1010]{0}[-]", cost.GetCurrencyLabelWithIcon());
                AlertDialog.Load("确认用" + costLabel + "升级这个技能吗", "不了", null, "升级",
                                 () =>
                                     {
                                         if (afford)
                                         {
                                             Requester.Instance.Send(new UpgradeSkill(_skill.SkillCode));
                                         }
                                         else
                                         {
                                             if (cost.Type == (int) CurrencyType.Coin)
                                             {
                                                 MorlnFloatingToast.Create("您的金币不足了");
                                             }
                                             else if (cost.Type == (int) CurrencyType.Diamond)
                                             {
                                                 MorlnFloatingToast.Create("您的钻石不足了");
                                             }
                                             else
                                             {
                                                 MorlnFloatingToast.Create("您的货币不足了");
                                             }
                                         }
                                     });
                //Requester.Instance.Send(new UpgradeSkill(_skill.SkillCode));
            }
            else
            {
                MorlnFloatingToast.Create("你的玩家等级需要达到" + _skill.UnlockLevelList[_level] + "级");
            }
        }

        private void OnDetailClick()
        {
            UMengPlugin.UMengEvent(EventId.SKILL_DETAIL,
                                   new Dictionary<string, object> {{"code", _skill.SkillCode}, {"level", _level}});

            //这个复杂了，每个技能的介绍还不一样
            var skillLevelDetailTextConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillLevelDetailTextConfig) as SkillLevelDetailTextConfig;
            if (skillLevelDetailTextConfig != null)
            {
                string textToShow;
                var skillLevelDetail = skillLevelDetailTextConfig.DetailList.Find(x => x.SkillCode == _skill.SkillCode);
                if (skillLevelDetail == null) return;
                if (_level - 1 >= skillLevelDetail.LevelDetailList.Count) return;//配置长度不够，不管了
                if (_level == 0)
                {
                    var textNextLevel = skillLevelDetail.LevelDetailList[_level];
                    textToShow = string.Format("1级\n{0}", textNextLevel);
                }
                else
                {
                    var textCurLevel = skillLevelDetail.LevelDetailList[_level - 1];
                    if (_level < skillLevelDetail.LevelDetailList.Count)
                    {
                        var textNextLevel = skillLevelDetail.LevelDetailList[_level];
                        textToShow = string.Format("{0}级\n{1}\n\n下一级\n{2}", _level, textCurLevel, textNextLevel);
                    }
                    else
                    {
                        textToShow = string.Format("{0}级\n{1}\n\n已经顶级", _level, textCurLevel);
                    }
                }
                MorlnTooltip.Show(textToShow, SprSkillIcon.transform.position);
            }
        }
    }
}