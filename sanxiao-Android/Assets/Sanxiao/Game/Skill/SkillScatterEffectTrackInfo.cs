using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 附着在技能散落特效的轨迹Prefab上，动画师填写。
    /// </summary>
    public class SkillScatterEffectTrackInfo : MonoBehaviour
    {
        /// <summary>
        /// 沿着轨迹运动的跟踪点
        /// </summary>
        public Transform TrackPoint;
        /// <summary>
        /// 寿命，结束后销毁
        /// </summary>
        public float Lifespan;

        private void Awake()
        {
            if (Lifespan > 20) Lifespan = 20;
            else if (Lifespan < 0) Lifespan = 0;
            Destroy(gameObject, Lifespan);//自动销毁
        }
    }
}