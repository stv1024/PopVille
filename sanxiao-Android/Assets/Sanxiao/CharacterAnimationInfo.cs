using Assets.Scripts;
using UnityEngine;

namespace Assets.Sanxiao
{
    /// <summary>
    /// 角色动画相关的一些参数，绑定在美术制作的角色Prefab上。动画师填写，程序使用
    /// </summary>
    public class CharacterAnimationInfo : MonoBehaviour
    {
        public Animator Animator;

        /// <summary>
        /// 放置武器的父物体。程序将武器Prefab放入该Parent，保留Prefab上存在的位置信息，即position,rotation,scale未必为0
        /// </summary>
        public SpriteRenderer Weapon;

        /// <summary>
        /// 放置盾牌的父物体。程序将盾牌Prefab放入该Parent，保留Prefab上存在的位置信息，即position,rotation,scale未必为0
        /// </summary>
        public SpriteRenderer Shield;

        /// <summary>
        /// 暂不使用
        /// </summary>
        public AudioClip AttackAudio;

        void Awake()
        {
            var c1 = MorlnResources.Load("Characters/Body");
            var controller = MorlnResources.Load<RuntimeAnimatorController>("Characters/Body");
            Animator.runtimeAnimatorController = controller;
        }
    }
}