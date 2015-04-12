using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;
using Random = UnityEngine.Random;
using UseSkill = Assets.Sanxiao.Communication.UpperPart.UseSkill;

namespace Assets.Sanxiao
{
    public static class SkillUtil
    {
        /// <summary>
        /// 技能是否是正向（对自己施放）的，是返回true，负向（对敌施放）返回false
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static bool IsSkillAffectSelf(this SkillEnum skill)
        {
            return ((int) skill)/100 == 1;
        }

        /// <summary>
        /// 获得我拥有的某个档次grade的随机技能。如果该grade没有技能，会报Error，并返回0
        /// </summary>
        /// <returns></returns>
        public static SkillEnum GetRandomMySkill()
        {
            var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
            if (skillConfig != null)
            {
                var curGradeSkillList =  skillConfig.SkillList.Where(x =>
                    {
                        var userSkill = CommonData.MySkillList.Find(y => y.Code == x.SkillCode);
                        return userSkill != null && userSkill.Level >= 1;
                    }).ToList();
             
                if (curGradeSkillList.Count == 0)
                {
                    return 0;
                }
                var ranInd = Random.Range(0, curGradeSkillList.Count);
                return (SkillEnum) curGradeSkillList[ranInd].SkillCode;
            }
            return 0;
        }
        /// <summary>
        /// 不重复地选出3个我可以施放的技能
        /// </summary>
        /// <returns></returns>
        public static List<SkillEnum> Random3OfMySkill()
        {
            var skillCodes = new List<SkillEnum>();
            var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
            if (skillConfig != null)
            {
                var curAvailableSkillList = skillConfig.SkillList.Where(x =>
                    {
                        var userSkill = CommonData.MySkillList.Find(y => y.Code == x.SkillCode);
                        var doIHave = userSkill != null && userSkill.Level >= 1;
                        if (!doIHave) return false;
                        if (userSkill.Level - 1 < x.PhysicalDamageList.Count)
                        {
                            return GameData.MyEnergyCapacity >= x.PhysicalDamageList[userSkill.Level - 1];
                        }
                        return false;
                    }).ToList();

                for (int i = 0; i < 3; i++)
                {
                    if (curAvailableSkillList.Count == 0)
                    {
                        break;
                    }
                    var ranInd = Random.Range(0, curAvailableSkillList.Count);
                    skillCodes.Add((SkillEnum) curAvailableSkillList[ranInd].SkillCode);
                    curAvailableSkillList.RemoveAt(ranInd);
                }
            }
            return skillCodes;
        }

        public static Skill GetSkill(SkillEnum skillCode)
        {
            var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
            if (skillConfig != null)
            {
                var skill = skillConfig.SkillList.Find(x => x.SkillCode == (int)skillCode);
                if (skill == null)
                {
                    Debug.LogError("配置里不存在该技能的数据skillCode:" + skillCode);
                    return null;
                }
                return skill;
            }
            Debug.LogError("怎能没有SkillConfig");
            return null;
        }

        public static string GetSkillDisplayName(SkillEnum skillCode)
        {
            var skillIntroTextConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.SkillIntroTextConfig) as SkillIntroTextConfig;
            if (skillIntroTextConfig != null)
            {
                var index = skillIntroTextConfig.SkillCodeList.FindIndex(x => x == (int) skillCode);
                if (index < 0)
                {
                    Debug.LogError("配置里不存在该技能的数据skillCode:" + skillCode);
                    return null;
                }
                if (index < skillIntroTextConfig.DisplayNameList.Count)
                {
                    return skillIntroTextConfig.DisplayNameList[index];
                }
                else
                {
                    Debug.LogError("skillIntroTextConfig.DisplayNameList长度不对.Count:" +
                                   skillIntroTextConfig.DisplayNameList.Count);
                }
            }
            Debug.LogError("怎能没有SkillIntroTextConfig");
            return null;
        }

        /// <summary>
        /// 若错误返回2000
        /// </summary>
        /// <param name="skillCode"></param>
        /// <returns></returns>
        public static int GetSkillUseEnergy(SkillEnum skillCode)
        {
            var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
            if (skillConfig != null)
            {
                var skill = skillConfig.SkillList.Find(x => x.SkillCode == (int)skillCode);
                if (skill == null)
                {
                    Debug.LogError("配置里不存在该技能的数据skillCode:" + skillCode);
                    return 2000;
                }
                return GetSkillUseEnergy(skill);
            }
            Debug.LogError("怎能没有SkillConfig");
            return 2000;
        }
        /// <summary>
        /// 错误返回2000
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static int GetSkillUseEnergy(Skill skill)
        {
            if (skill == null)
            {
                Debug.LogError("skill == null怎么可以");
                return 2000;
            }
            var level = GetMySkillLevel((SkillEnum)skill.SkillCode);
            if (level > 0 && level <= skill.PhysicalDamageList.Count)
            {
                return skill.PhysicalDamageList[level - 1];
            }
            else
            {
                Debug.LogError("有错误77");
                return 2000;
            }
        }

        /// <summary>
        /// 我施放技能时，根据当前状态和信息，生成UseSkill消息，供后面的PreShow和发送命令使用。UseSkill包含效果和逻辑的所有信息。
        /// </summary>
        /// <param name="skillCode"></param>
        /// <returns></returns>
        public static UseSkill GetUseSkillCmd(SkillEnum skillCode)
        {
            var skill = skillCode.GetSkill();
            if (skill == null) Debug.LogError("找不到Skill:Code==" + skillCode);

            var level = GetMySkillLevel(skillCode);
            if (level == 0) Debug.LogError("我的技能还未解锁skillCode:" + skillCode);

            var physicalDamage = skill == null ? 0 : skill.PhysicalDamageList[level - 1];
            switch (skillCode)
            {
                case SkillEnum.ExtraDamage:
                    {
                        var config = DesignConfigHelper.GetSkillParameterItem(skillCode);
                        if (config == null || config.ConstantList.Count < 2) break;
                        var extraDamage = Mathf.FloorToInt(level*config.ConstantList[1] + config.ConstantList[0]);
                        physicalDamage += extraDamage;
                    }
                    break;
            }

            var cmd = new UseSkill((int)skillCode, physicalDamage);
            return cmd;
        }

        /// <summary>
        /// 出错返回0，不会Exception
        /// </summary>
        /// <param name="skillCode"></param>
        /// <returns></returns>
        public static int GetMySkillLevel(SkillEnum skillCode)
        {
            var userSkill = CommonData.MySkillList.Find(x => x.Code == (int)skillCode);
            return userSkill == null ? 0 : userSkill.Level;
        }
        //public static string GetSkillName(int code)
        //{
        //    switch (code)
        //    {
        //        case 101:
        //            return "CreateStripe";
        //        case 102:
        //            return "CreateStripe";
        //        case 103:
        //            return "CreateColorful";
        //        case 104:
        //            return "AllHint";
        //        case 105:
        //            return "HideGenre";
        //        case 106:
        //            return "GoldenFinger";
        //        case 107:
        //            return "RecoverHealth";
        //        case 201:
        //            return "Stain";
        //        case 202:
        //            return "Ice";
        //        case 203:
        //            return "Bird";
        //        case 204:
        //            return "Shake";
        //        case 205:
        //            return "Stone";
        //        case 206:
        //            return "Lock";
        //        case 207:
        //            return "Brick";
        //        case 208:
        //            return "ExtraDamage";
        //        case 209:
        //            return "ReduceEnergy";
        //        case 210:
        //            return "Grayscale";
        //    }
        //    return null;
        //}
    }
}