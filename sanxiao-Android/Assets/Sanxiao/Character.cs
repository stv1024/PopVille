
using System.Collections.Generic;
using Assets.Sanxiao.Data;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao
{
    /// <summary>
    /// 角色：鸡、狗、猫。
    /// 用途：MenuUI,MatchUI,EquipPanel,GameUI
    /// </summary>
    public class Character : MonoBehaviour
    {
        public const int MaxCharacterKindCount = 16;

        public Animator CharacterAnimator
        {
            get { return AnimationInfo ? AnimationInfo.Animator : null; }
        }

        public CharacterAnimationInfo AnimationInfo;


        /// <summary>
        /// 需要变成的角色Code，设置完后调用Refresh()，如果跟旧的不一样才更换BA
        /// </summary>
        public int CharacterCode;
        /// <summary>
        /// 当前显示的BA，用于判断Refresh()是否使用旧的还是更换新的
        /// </summary>
        private int _curShowCode;

        /// <summary>
        /// 刷新角色形象为当前Code，若刚才的形象不对，则施放旧的，创建新的，都是从Pool里操作，开销不大。重复设置相同Code无开销。
        /// 刷新前一定要设置好Code
        /// </summary>
        public void Refresh()
        {
            Refresh(1f);
        }
        /// <summary>
        /// 刷新角色形象为当前Code，若刚才的形象不对，则施放旧的，创建新的，都是从Pool里操作，开销不大。重复设置相同Code无开销。
        /// 刷新前一定要设置好Code
        /// 可以设置大小哦，1是原大小
        /// </summary>
        public void Refresh(float scale)
        {
            if (_curShowCode != CharacterCode)
            {
                if (AnimationInfo)
                {
                    CharacterBAPool.Enqueue(_curShowCode, AnimationInfo.gameObject);
                }

                AnimationInfo = null;
                _curShowCode = CharacterCode;
            }

            if (AnimationInfo == null)
            {
                var go = CharacterBAPool.Dequeue(CharacterCode);
                if (go != null)
                {
                    go.transform.parent = transform;
                    go.transform.ResetTransform();
                    go.transform.localScale = new Vector3(scale, scale, 1);
                    AnimationInfo = go.GetComponent<CharacterAnimationInfo>();
                }
            }
        }

        public float ArtContentScale
        {
            get { return AnimationInfo ? AnimationInfo.transform.localScale.x : 1; }
            set { if (AnimationInfo) AnimationInfo.transform.localScale = new Vector3(value, value, 1); }
        }

        /// <summary>
        /// 施放ArtContent到Pool，从而隐藏角色形象，重复调用无副作用
        /// </summary>
        public void ClearToEmpty()
        {
            if (AnimationInfo)
            {
                Destroy(AnimationInfo.gameObject);
                //CharacterBAPool.Enqueue(CharacterCode, AnimationInfo.gameObject);
                AnimationInfo = null;
            }
        }

        public void Attack(int fromIndex, int toIndex)
        {
            if (CharacterAnimator != null)
            {
                CharacterAnimator.SetTrigger(string.Format("Attack{0}-{1}", fromIndex, toIndex));
            }
            if (AnimationInfo)
            {
                //Invoke("PlayAttackAudio", AnimationInfo.TimeFromPlayAttackToRivalBeAttacked*0.6f);
                //Invoke("RivalPlayBeAttackPhysically", AnimationInfo.TimeFromPlayAttackToRivalBeAttacked);
            }
        }

        private void PlayAttackAudio()
        {
            if (AnimationInfo)
            {
                AudioManager.PlayOneShot(AnimationInfo.AttackAudio); //攻击音效
            }
        }

        public void BeAttacked()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("BeAttacked");
        }

        public void BeMagicallyAttacked()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("BeAttacked");//TODO:改成魔法攻击
        }

        public void BeMagicallyBuff()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("BeMagicallyBuff");//魔法增益
        }

        public void Die()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("Die");
        }

        public void Cheer()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("Victory");
        }

        public void Cry()
        {
            if (CharacterAnimator != null) CharacterAnimator.SetTrigger("Failure");
        }

        /// <summary>
        /// 动画归位
        /// </summary>
        public void Reset()
        {
            if (CharacterAnimator != null)
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    for (int j = 0; j < GameData.TeamMaxNumber; j++)
                    {
                        CharacterAnimator.ResetTrigger(string.Format("Attack{0}-{1}", i, j));
                    }
                }
                CharacterAnimator.ResetTrigger("BeAttacked");
                CharacterAnimator.ResetTrigger("BeMagicallyBuff");
                CharacterAnimator.ResetTrigger("Die");
                CharacterAnimator.ResetTrigger("Failure");
                CharacterAnimator.ResetTrigger("Victory");
                CharacterAnimator.SetTrigger("Reset");
            }
        }
        
        /// <summary>
        /// 穿装备。
        /// </summary>
        /// <param name="type">0：头盔，1：盔甲，2：剑，3：盾，4：鞋子。</param>
        /// <param name="equipCode">null则脱掉</param>
        public void WearEquip(int type, int? equipCode)
        {
            if (!AnimationInfo) return;
            var equipSprite = equipCode != null ? MorlnDownloadResources.Load<Sprite>("ResourcesForDownload/Equip/EquipSprite/Equip" + equipCode) : null;
            SpriteRenderer sr;
            switch (type)
            {
                case 2:
                    sr = AnimationInfo.Weapon;
                    break;
                case 3:
                    sr = AnimationInfo.Shield;
                    break;
                default:
                    return;
            }
            if (equipSprite)
            {
                if (sr) sr.sprite = equipSprite;
            }
            else
            {
                if (sr) sr.sprite = null;
            }
        }

        public void WearEquip(List<int> equipCodes)
        {
            foreach (var equipCode in equipCodes)
            {
                var type = EquipUtil.GetEquipType(equipCode);
                WearEquip(type, equipCode);
            }
        }

        public void TakeOffAllEquip()
        {
            if (!AnimationInfo) return;
            if (AnimationInfo.Weapon) AnimationInfo.Weapon.sprite = null;
            if (AnimationInfo.Shield) AnimationInfo.Shield.sprite = null;
        }
    }
}