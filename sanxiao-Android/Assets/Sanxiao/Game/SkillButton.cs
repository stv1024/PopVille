using System.Globalization;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 使用技能按钮
    /// </summary>
    public class SkillButton : MonoBehaviour
    {
        public GameManager GameManager;

        public UISprite SprBackground;
        public UISprite SprSkillIcon;
        /// <summary>
        /// 中文名，无图时显示
        /// </summary>
        public UILabel LblDisplayName;
        public UILabel LblSkillLevel;
        public UILabel LblCost;

        /// <summary>
        /// 记得设置
        /// </summary>
        public int Index;

        /// <summary>
        /// 如果没有该档次的技能(skillCode == 0)，则显示锁住
        /// </summary>
        public void Refresh(SkillEnum skillCode)
        {
            if (skillCode == 0)
            {
                gameObject.SetActive(false);

                SprSkillIcon.enabled = false;
                LblDisplayName.text = null;
                LblCost.text = null;
            }
            else
            {
                gameObject.SetActive(true);

                //图标
                var spriteName = string.Format("skillicon-{0}", skillCode);
                if (SprSkillIcon.atlas.GetSprite(spriteName) != null)
                {
                    SprSkillIcon.spriteName = spriteName;
                    SprSkillIcon.MakePixelPerfect();
                    SprSkillIcon.enabled = true;
                }
                else
                {
                    SprSkillIcon.enabled = false;

                    LblDisplayName.text = skillCode.ToString();
                    LblDisplayName.enabled = true;
                }

                //技能等级
                var userSkill = CommonData.MySkillList.Find(x => x.Code == (int)skillCode);
                int level;
                if (userSkill == null)
                {
                    Debug.LogError("这里怎么可能找不到userSkill:Code==" + skillCode);
                    level = 0;
                }
                else
                {
                    level = userSkill.Level;
                }
                //技能名称
                var skillIntroTextConfig =
                    ConfigManager.GetConfig(ConfigManager.ConfigType.SkillIntroTextConfig) as SkillIntroTextConfig;
                if (skillIntroTextConfig != null)
                {
                    var index = skillIntroTextConfig.SkillCodeList.FindIndex(x => x == (int)skillCode);
                    if (index >= 0 && index < skillIntroTextConfig.DisplayNameList.Count)
                    {
                        LblDisplayName.text = string.Format("{0}",
                                                            skillIntroTextConfig.DisplayNameList[index]);
                        LblDisplayName.enabled = true;

                        LblSkillLevel.text = string.Format("{0}级", level);
                    }
                    else
                    {
                        LblDisplayName.enabled = false;
                    }
                }
                else
                {
                    LblDisplayName.enabled = false;
                }


                //价格
                var useEnergy = SkillUtil.GetSkillUseEnergy(skillCode);
                if (LblCost != null) //免费技能可以固定显示FREE
                {
                    LblCost.text = useEnergy.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// 仅用于可随意设定内容的新手教程状态
        /// </summary>
        public void Refresh(SkillEnum skillCode, int skillLevel, string skillName, int useEnergy)
        {
            if (skillCode == 0)
            {
                gameObject.SetActive(false);

                SprSkillIcon.enabled = false;
                LblDisplayName.text = null;
                LblCost.text = null;
            }
            else
            {
                gameObject.SetActive(true);

                //图标
                var spriteName = string.Format("skillicon-{0}", skillCode);
                if (SprSkillIcon.atlas.GetSprite(spriteName) != null)
                {
                    SprSkillIcon.spriteName = spriteName;
                    SprSkillIcon.MakePixelPerfect();
                    SprSkillIcon.enabled = true;
                }
                else
                {
                    SprSkillIcon.enabled = false;
                    LblDisplayName.text = skillCode.ToString();
                    LblDisplayName.enabled = true;
                }

                //技能名称
                LblDisplayName.text = skillName;
                LblSkillLevel.text = string.Format("{0}级", skillLevel);

                //价格
                LblCost.text = useEnergy.ToString(CultureInfo.InvariantCulture);
            }
        }

        void OnClick()
        {
            GameManager.UseSelectedSkill(Index);
        }
    }
}