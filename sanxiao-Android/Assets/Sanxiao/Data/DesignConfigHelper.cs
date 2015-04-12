using Assets.Sanxiao.Communication.Proto;
using UnityEngine;

namespace Assets.Sanxiao.Data
{
    public class DesignConfigHelper
    {
        //public List<DesignSkillConfigItem> ConfigList;

        public static SkillParameter GetSkillParameterItem(SkillEnum skillCode)
        {
            var skillParameterConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillParameterConfig) as SkillParameterConfig;
            if (skillParameterConfig != null)
            {
                return skillParameterConfig.SkillParameterList.Find(x => x.SkillCode == (int) skillCode);
            }
            else
            {
                Debug.LogError("没有SkillParameterConfig");
                return null;
            }
        }

        public static int GetRandomPhysicalDamage(int average, float deviation)
        {
            return Mathf.RoundToInt(Random.Range(1 - deviation, 1 + deviation)*average);
        }
    }
}