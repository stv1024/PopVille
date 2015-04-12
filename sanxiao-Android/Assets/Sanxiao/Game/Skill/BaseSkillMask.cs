using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 技能遮罩基类
    /// </summary>
    public abstract class BaseSkillMask : MonoBehaviour
    {
        public virtual bool CanTouchThroughThisMask(Vector2 localPos)
        {
            return true;
        }

        public abstract void Show(params int[] parameters);
    }
}