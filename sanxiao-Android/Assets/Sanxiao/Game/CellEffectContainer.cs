using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.AI;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 消除特效容器
    /// </summary>
    public class CellEffectContainer : MonoBehaviour
    {
        public GameObject BrickBreakEffectPrefab, LockBreakEffectPrefab;

        public void PlayBrickBreakEffect(Vector2 localPos)
        {
            var go = PrefabHelper.InstantiateAndReset(BrickBreakEffectPrefab, transform);
            go.transform.localPosition = localPos;
            Destroy(go, 3);

            AudioManager.PlayOneShot(GameManager.Instance.MyGrid.BrickBreadAudio);
        }
        public void PlayLockBreakEffect(Vector2 localPos)
        {
            var go = PrefabHelper.InstantiateAndReset(LockBreakEffectPrefab, transform);
            go.transform.localPosition = localPos;
            Destroy(go, 3);

            AudioManager.PlayOneShot(GameManager.Instance.MyGrid.LockBreakAudio);
        }

        public void NormalPopEffect(IntVector2 ij, int genre)
        {
            //粒子特效
            var ps = PopParticleSysPool.Dequeue().transform;
            ps.ResetTransform(transform);
            ps.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);

            ps.GetComponent<ParticleSystem>().Play();
            PopParticleSysPool.Enqueue(ps.gameObject, 5);
        }

        public void CreateAddExpFloatingLabel(Vector2 localPos, long addExp)
        {
            var fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(AddEnergyLabelPrefab, transform);
            fl.Reset(string.Format("+{0} XP", addExp), 1f);
            fl.transform.localPosition = localPos;
        }
        public GameObject AddEnergyLabelPrefab;
        public void CreateAddEnergyFloatingLabel(Vector2 localPos, long addEnergy)
        {
            var fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(AddEnergyLabelPrefab, transform);
            fl.Reset(string.Format("{0}蓄力", addEnergy), 1f);
            fl.transform.localPosition = localPos;
        }

        public GameObject ComboLabelPrefab;
        public void CreateAddComboLabel(Vector2 localPos, int multiple)
        {
            var fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(ComboLabelPrefab, transform);
            fl.Reset(string.Format("Combo\n蓄力×{0}", multiple), 1f);
            fl.transform.localPosition = localPos;
        }
        public GameObject PraiseLabelPrefab;
        public void CreatePraiseLabel(int multiple)
        {
            var fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(PraiseLabelPrefab, transform);
            string text;
            switch (multiple)
            {
                case 2:
                    text = "Good\n×2";
                    break;
                case 3:
                    text = "Awesome\n×3";
                    break;
                case 4:
                    text = "Perfect\n×4";
                    break;
                default:
                    text = "Holy Shit!!!\n×"+multiple;
                    break;
            }
            fl.Reset(text, 0.5f);
        }

        #region 连线型提示

        public GameObject HintLinePrefab;
        public Transform HintLineContainer;
        readonly GameObject[][] _horizontalHintLineArray = new GameObject[Grid.Height][];
        readonly GameObject[][] _verticalHintLineArray = new GameObject[Grid.Height][];

        public void ShowAllHintLines(List<CandyExchange> hintList)
        {
            #region 初始化Array

            if (_horizontalHintLineArray[0] == null)
            {
                for (int i = 0; i < _horizontalHintLineArray.Length; i++)
                {
                    _horizontalHintLineArray[i] = new GameObject[Grid.Width];
                }
            }
            if (_verticalHintLineArray[0] == null)
            {
                for (int i = 0; i < _verticalHintLineArray.Length; i++)
                {
                    _verticalHintLineArray[i] = new GameObject[Grid.Width];
                }
            }

            #endregion

            ClearAllHintLines();

            foreach (var candyExchange in hintList)
            {
                if (candyExchange.IJ.i == candyExchange.IJ1.i) //横
                {
                    var ij = candyExchange.IJ.j < candyExchange.IJ1.j ? candyExchange.IJ : candyExchange.IJ1;
                    if (_horizontalHintLineArray[ij.i][ij.j] == null)
                    {
                        _horizontalHintLineArray[ij.i][ij.j] = PrefabHelper.InstantiateAndReset(HintLinePrefab,
                                                                                                HintLineContainer);
                        _horizontalHintLineArray[ij.i][ij.j].transform.localPosition =
                            GameManager.Instance.MyGrid.GetCellPosition(ij);
                    }
                    _horizontalHintLineArray[ij.i][ij.j].SetActive(true);
                }
                else //竖
                {
                    var ij = candyExchange.IJ.i < candyExchange.IJ1.i ? candyExchange.IJ : candyExchange.IJ1;
                    if (_verticalHintLineArray[ij.i][ij.j] == null)
                    {
                        _verticalHintLineArray[ij.i][ij.j] = PrefabHelper.InstantiateAndReset(HintLinePrefab,
                                                                                                HintLineContainer);
                        _verticalHintLineArray[ij.i][ij.j].transform.localPosition =
                            GameManager.Instance.MyGrid.GetCellPosition(ij);
                        _verticalHintLineArray[ij.i][ij.j].transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                    _verticalHintLineArray[ij.i][ij.j].SetActive(true);
                }
            }
        }

        public void ClearAllHintLines()
        {
            for (int i = 0; i < Grid.Height; i++)
            {
                for (int j = 0; j < Grid.Width; j++)
                {
                    if (_horizontalHintLineArray[i][j]) _horizontalHintLineArray[i][j].SetActive(false);
                    if (_verticalHintLineArray[i][j]) _verticalHintLineArray[i][j].SetActive(false);
                }
            }
        }

        #endregion
    }
}