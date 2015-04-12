using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.UI.Match
{
    /// <summary>
    /// 敌方信息界面的技能图标
    /// </summary>
    public class SkillSlot : MonoBehaviour
    {
        public UISprite Spr;
        private SkillEnum _skillCode;
        private int _level;

        public void SetAndRefresh(SkillEnum skillCode, int skillLevel)
        {
            _skillCode = skillCode;
            _level = skillLevel;
            Spr.gameObject.SetActive(true);
            var spriteName = string.Format("skillicon-{0}", _skillCode);
            if (Spr.atlas.GetSprite(spriteName) != null)
            {
                Spr.spriteName = spriteName;
                Spr.enabled = true;
            }
            else
            {
                Spr.enabled = false;
            }
        }

        void OnClick()
        {
            //这个复杂了，每个技能的介绍还不一样
            var skillLevelDetailTextConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillLevelDetailTextConfig) as SkillLevelDetailTextConfig;
            if (skillLevelDetailTextConfig != null)
            {
                var skillLevelDetail =
                    skillLevelDetailTextConfig.DetailList.Find(x => (SkillEnum) x.SkillCode == _skillCode);
                if (skillLevelDetail == null) return;
                if (_level - 1 >= skillLevelDetail.LevelDetailList.Count) return; //配置长度不够，不管了

                var textCurLevel = skillLevelDetail.LevelDetailList[_level - 1];
                var textToShow = string.Format("{0}级\n{1}", _level, textCurLevel);

                MorlnTooltip.Show(textToShow, transform.position);
            }
        }

    }
}