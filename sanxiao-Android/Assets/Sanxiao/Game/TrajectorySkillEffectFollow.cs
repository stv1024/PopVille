using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 技能弹道特效
    /// </summary>
    public class TrajectorySkillEffectFollow : MonoBehaviour
    {
        public Transform Target;

        public bool FollowPosition = true;
        public bool FollowRotation = true;

        void Update()
        {
            if (!Target) return;

            if (FollowPosition)
            {
                transform.position = Target.position;
            }
            if (FollowRotation)
            {
                transform.rotation = Target.rotation;
            }
        }
    }
}