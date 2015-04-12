using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 攻击时三消区域遮罩，有个头像
    /// </summary>
    public class AttackAvatar : MonoBehaviour
    {
        public Transform CharacterParent;
        public Transform FlipTra;

        /// <summary>
        /// 立即调用。传入Prefab而不是新实例
        /// </summary>
        /// <param name="characterGo"></param>
        /// <param name="flip"></param>
        public void Initialize(GameObject characterGo, bool flip)
        {
            characterGo.transform.ResetTransform(CharacterParent);
            characterGo.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            var cai = characterGo.GetComponent<CharacterAnimationInfo>();
            cai.transform.SetSortingLayer("Foreground");
            cai.Animator.enabled = false;
            if (flip) FlipTra.localScale = FlipTra.localScale.SetV3X(-1);
            //cai.Animator.SetTrigger("");
        }
    }
}