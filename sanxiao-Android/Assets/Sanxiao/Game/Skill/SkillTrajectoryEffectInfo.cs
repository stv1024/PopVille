using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 技能弹道特效信息.统一是右边角色施放的技能-动画师填写，程序使用
    /// </summary>
    public class SkillTrajectoryEffectInfo : MonoBehaviour
    {
        /// <summary>
        /// 动画师填写，寿命，单位:秒，创建后自动销毁自己
        /// </summary>
        public float Lifespan;
        /// <summary>
        /// 动画师填写，散落特效起点。如果不填则没有散落特效
        /// </summary>
        public Transform ScatterEffectStartPoint;

        void Awake()
        {
            Destroy(gameObject, Lifespan);
        }

        /// <summary>
        /// 左边角色施放技能时，调用这个Flip来反向
        /// </summary>
        public void FlipDirection()
        {
            transform.localScale = transform.localScale.SetV3X(-1);
            var pss = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in pss)
            {
                ps.transform.RotateAround(ps.transform.position, Vector3.up, 180);
            }
        }
    }
}