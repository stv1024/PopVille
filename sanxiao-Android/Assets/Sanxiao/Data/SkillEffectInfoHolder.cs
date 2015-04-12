using System.Linq;
using UnityEngine;

namespace Assets.Sanxiao.Data
{
    /// <summary>
    /// 放在Project里，仅为了存数值。动画师设置数值
    /// </summary>
    public class SkillEffectInfoHolder : MonoBehaviour
    {
        /// <summary>
        /// 一个技能从开始到结束的最长时间限制-策划
        /// </summary>
        public const float MaxSkillEffectLength = 3;

        /// <summary>
        /// 动画师填写从开始播放到对方受击的时间
        /// </summary>
        public float TimeFromPlayAttackToRivalBeAttacked = 0.6f;
        /// <summary>
        /// 动画师填写受击动画的长度
        /// </summary>
        public float BeAttackedLength = 0.5f;
        /// <summary>
        /// 播放弹道特效时刻
        /// </summary>
        public float PlayTrajectoryEffectTime;

        public SkillEnum[] SkillCodeList;

        /// <summary>
        /// 魔法受击时刻
        /// </summary>
        public float[] BeMagicallyAttackTimeList;

        /// <summary>
        /// 弹道特效长度
        /// </summary>
        public float[] TrajectoryAnimationLengthList;

        /// <summary>
        /// 播放三消效果时刻
        /// </summary>
        public float[] PlaySanxiaoEffectTimeList;

        public Vector2[] ScatterPointList;

        /// <summary>
        /// 检查数据是否完整
        /// </summary>
        public bool CheckInfoCompleteAndValid()
        {
            if (SkillCodeList == null || BeMagicallyAttackTimeList == null || TrajectoryAnimationLengthList == null ||
                PlaySanxiaoEffectTimeList == null)
            {
                Debug.LogError("未初始化，错误45");
                return false;
            }
            if (SkillCodeList.Length != BeMagicallyAttackTimeList.Length ||
                SkillCodeList.Length != TrajectoryAnimationLengthList.Length ||
                SkillCodeList.Length != PlaySanxiaoEffectTimeList.Length ||
                SkillCodeList.Length != ScatterPointList.Length)
            {
                Debug.LogError("配置长度不全相等，错误50");
                return false;
            }
            return true;
        }

        /// <summary>
        /// -1表示不要播放动画
        /// </summary>
        /// <param name="skillCode"></param>
        /// <returns></returns>
        public float GetBeMagicallyAttackTime(SkillEnum skillCode)
        {
            var index = SkillCodeList.ToList().FindIndex(x=>x == skillCode);
            if (index < 0) return -1;
            return BeMagicallyAttackTimeList[index];
        }
        public float GetTrajectoryAnimationLength(SkillEnum skillCode)
        {
            var index = SkillCodeList.ToList().FindIndex(x => x == skillCode);
            if (index < 0) return 0.1f;
            return TrajectoryAnimationLengthList[index];
        }
        public float GetPlaySanxiaoEffectTime(SkillEnum skillCode)
        {
            var index = SkillCodeList.ToList().FindIndex(x => x == skillCode);
            if (index < 0) return 0.1f;
            return PlaySanxiaoEffectTimeList[index];
        }
        public Vector2 GetScatterPoint(SkillEnum skillCode)
        {
            var index = SkillCodeList.ToList().FindIndex(x => x == skillCode);
            if (index < 0) return new Vector2(-172, 50);
            return ScatterPointList[index];
        }
    }
}