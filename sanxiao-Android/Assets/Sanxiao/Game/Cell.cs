using System;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 单个格子
    /// </summary>
    public class Cell : MonoBehaviour
    {
        public enum State
        {
            /// <summary>
            /// 不可用
            /// </summary>
            Null = 0,
            /// <summary>
            /// 空，正常
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 枷锁
            /// </summary>
            Lock = 11,
            /// <summary>
            /// 砖块
            /// </summary>
            Brick = 12,
        }

        public State MyState = State.Normal;

        public bool CanHoldCandy { get { return MyState == State.Normal || MyState == State.Lock; } }

        public Candy MyCandy;

        #region 渲染层

        public Sprite SpriteBrick, SpriteLock;
        public GameObject SprNotNull, SprNull;

        public SpriteRenderer Spr;
        private State _lastState = State.Normal;
        void Update()
        {
            if (MyState == _lastState) return;

            //状态改变，可能有动画
            switch (_lastState)
            {
                case State.Null:
                    break;
                case State.Normal:
                    break;
                case State.Lock:
                    break;
                case State.Brick:
                    break;
            }

            if (MyState == State.Null)
            {
                SprNotNull.SetActive(false);
                SprNull.SetActive(true);
            }
            else
            {
                SprNotNull.SetActive(true);
                SprNull.SetActive(false);
            }

            switch (MyState)
            {
                case State.Null:
                    break;
                case State.Normal:
                    Spr.GetComponent<Renderer>().enabled = false;
                    break;
                case State.Lock:
                    Spr.sprite = SpriteLock;
                    Spr.GetComponent<Renderer>().enabled = true;
                    break;
                case State.Brick:
                    Spr.sprite = SpriteBrick;
                    Spr.GetComponent<Renderer>().enabled = true;
                    break;
            }
            _lastState = MyState;
        }
        #endregion

        /// <summary>
        /// 持有的糖果正在被消除,或者被特殊消除波及,解除格子负面状态，如砖块、枷锁
        /// </summary>
        public void BePopped()
        {
            if (MyState == State.Lock)
            {
                MyState = State.Normal;
                //动画,计分
                GameManager.Instance.CellEffectContainer.PlayLockBreakEffect(transform.localPosition);
                EnergyLightSpot.Create(transform.localPosition, 30, 2f);
                GameManager.Instance.CellEffectContainer.CreateAddEnergyFloatingLabel(transform.localPosition, 30);
            }
            else if (MyState == State.Brick)
            {
                MyState = State.Normal;
                //动画,计分
                GameManager.Instance.CellEffectContainer.PlayBrickBreakEffect(transform.localPosition);
                EnergyLightSpot.Create(transform.localPosition, 20, 1.8f);
                GameManager.Instance.CellEffectContainer.CreateAddEnergyFloatingLabel(transform.localPosition, 20);
                
            }
        }

        /// <summary>
        /// 邻格正在被正常消除，解除砖块
        /// </summary>
        public void AdjacentBeThreePopped()
        {
            if (MyState == State.Brick)
            {
                MyState = State.Normal;
                //动画,计分
                GameManager.Instance.CellEffectContainer.PlayBrickBreakEffect(transform.localPosition);
                EnergyLightSpot.Create(transform.localPosition, 20, 1.8f);
                GameManager.Instance.CellEffectContainer.CreateAddEnergyFloatingLabel(transform.localPosition, 20);
            }
        }
    }
}