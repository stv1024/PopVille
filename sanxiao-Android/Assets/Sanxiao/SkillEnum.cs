using System;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;

namespace Assets.Sanxiao
{
    public enum SkillEnum
    {
        CreateStripe = 101,
        CreateBomb = 102,
        CreateColorful = 103,
        AllHint = 104,
        HideGenre = 105,
        GoldenFinger = 106,
        /// <summary>
        /// 废弃
        /// </summary>
        RecoverHealth = 107,
        Stain = 201,
        Ice = 202,
        Bird = 203,
        Shake = 204,
        Stone = 205,
        Lock = 206,
        Brick = 207,
        /// <summary>
        /// 废弃
        /// </summary>
        ExtraDamage = 208,
        ReduceEnergy = 209,
        Grayscale = 210,
        Smog = 211,
    }

    public static class SkillEnumExtension
    {
        public static Skill GetSkill(this SkillEnum skillCode)
        {
            try
            {
                var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
                if (skillConfig != null)
                {
                    return skillConfig.SkillList.Find(x => x.SkillCode == (int) skillCode);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}