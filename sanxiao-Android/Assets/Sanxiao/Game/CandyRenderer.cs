using System.Text;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// Candy的渲染器
    /// </summary>
    public class CandyRenderer : MonoBehaviour
    {

        public Candy Candy;

        int Genre
        {
            get
            {
                //return 0;
                return Info.Genre;
            }
        }
        CandyInfo Info
        {
            get
            {
                return Candy.MyInfo;
            }
        }

        /// <summary>
        /// 美术制作的东西
        /// </summary>
        public GameObject ArtContent;
        public Animator CandyAnimator;
        public SpriteRenderer CandySpriteRenderer;

        private int _curShownGenre = -127;

        private float _nextBlinkTime;

        void Update()
        {
            if (CandyAnimator && Time.time > _nextBlinkTime)
            {
                SetNextBlinkTime();
                if (Candy.IsStatic) Blink();// if (Candy.IsStatic && CandyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal")) Blink();//TODO:怎么判断是否处于静止状态
            }
        }
        /// <summary>
        /// 设定下一次眨眼时间
        /// </summary>
        void SetNextBlinkTime()
        {
            _nextBlinkTime = Time.time + Random.Range(10f, 30f);
        }

        public void Refresh()
        {
            //调整大小
            var stdSize = Mathf.Max(7, Grid.MaxSide);
// ReSharper disable PossibleLossOfFraction
            transform.localScale = new Vector3(100*8/stdSize, 100*8/stdSize, 1);
// ReSharper restore PossibleLossOfFraction

            if (_curShownGenre != Genre)
            {
                if (ArtContent != null)
                {
                    if (CandyAnimator)
                    {
                        CandyBAPool.Enqueue(CandyAnimator.gameObject);
                    }
                    else
                    {
                        Destroy(ArtContent);
                    }
                    ArtContent = null;
                }

                _curShownGenre = Genre;
            }

            if (ArtContent == null)
            {
                var go = CandyBAPool.Dequeue(Info);
                go.transform.parent = transform;
                go.transform.ResetTransform();
                ArtContent = go;
                CandyAnimator = go.GetComponent<Animator>();
                CandySpriteRenderer = go.GetComponent<SpriteRenderer>();
            }

            if (CandyAnimator != null)
            {
                Reset();

                if (Genre >= 0)
                {
                    if (Info.Type == Candy.CandyType.H)
                    {
                        CandyAnimator.SetTrigger("BecomeH");
                    }
                    else if (Info.Type == Candy.CandyType.V)
                    {
                        CandyAnimator.SetTrigger("BecomeV");
                    }
                    else if (Info.Type == Candy.CandyType.Bomb)
                    {
                        if (Candy.Fired)
                        {
                            CandyAnimator.SetTrigger("BecomeFiredBomb");
                        }
                        else
                        {
                            CandyAnimator.SetTrigger("BecomeBomb");
                        }
                    }
                }
            }
            
            if (CandySpriteRenderer)//如果是Item，则要更换Sprite
            {
                CandySpriteRenderer.sprite = CandyBAPool.GetSpriteForCandy(Info);
            }

            SetNextBlinkTime();
        }

        public bool HasSupport
        {
            set { if (CandyAnimator != null) CandyAnimator.SetBool("HasSupport", value); }
        }

        public void Blink()
        {
            if (CandyAnimator != null) CandyAnimator.SetTrigger("Blink");
        }
        public void Pop()
        {
            if (CandyAnimator != null)
            {
                CandyAnimator.SetTrigger("Pop");
                CandyAnimator.SetInteger("Juice", Random.Range(0, 2));
            }
            Invoke("_DelayReleaseAnimator", 0.6f);
        }
        void _DelayReleaseAnimator()
        {
            ReleaseAnimation();
        }

        public void Exchange()
        {
            if (CandyAnimator != null) CandyAnimator.SetTrigger("Exchange");
        }

        public void Reset()
        {
            if (CandyAnimator != null)
            {
                HasSupport = true;
                CandyAnimator.ResetTrigger("Blink");
                CandyAnimator.ResetTrigger("Pop");
                CandyAnimator.ResetTrigger("Exchange");
                CandyAnimator.ResetTrigger("BecomeH");
                CandyAnimator.ResetTrigger("BecomeV");
                CandyAnimator.ResetTrigger("BecomeBomb");
                CandyAnimator.ResetTrigger("BecomeFiredBomb");
                CandyAnimator.SetTrigger("Reset");
            }
        }

        public void ReleaseAnimation()
        {
            if (CandyAnimator != null)
            {
                CandyBAPool.Enqueue(CandyAnimator.gameObject);
                CandyAnimator = null;
            }
            if (CandySpriteRenderer != null)
            {
                Destroy(CandySpriteRenderer.gameObject);
                CandySpriteRenderer = null;
            }
        }

        void OnPress(bool isPressed)
        {
            if (CandyAnimator != null) CandyAnimator.SetBool("BeingPressed", isPressed);
        }

        public static Color GetPopEffectColor(int genre)
        {
            switch (genre)
            {
                case 0:
                    return new Color32(230, 19, 4, 255);
                case 1:
                    return new Color32(224, 205, 18, 255);
                case 2:
                    return new Color32(255, 102, 21, 255);
                case 3:
                    return new Color32(245, 190, 142, 255);
                case 4:
                    return new Color32(98, 228, 1, 255);
                case 5:
                    return new Color32(196, 51, 242, 255);
                case 6:
                    return new Color32(225, 217, 176, 255);
            }
            return Color.white;

        }
    }
}