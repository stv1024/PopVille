using System.Collections;
using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 怒气光点，糖果消除后飞出光点，飞向怒气槽。所蕴含的能量点都在这里！这样保证光点飞到蓄力槽时才会增加蓄力值
    /// </summary>
    public class EnergyLightSpot : MonoBehaviour
    {
        public AnimationCurve Curve0, Curve1;

        private int _energy;

        public void Reset(Vector2 localPosStart, Vector2 localPosDestination, int energy)
        {
            Reset(localPosStart, localPosDestination, energy, 1);
        }

        public void Reset(Vector2 localPosStart, Vector2 localPosDestination, int energy, float scale)
        {
            _energy = energy;
            transform.localScale = new Vector3(scale, scale, 1);
            StopAllCoroutines();
            StartCoroutine(Play(localPosStart, localPosDestination));
        }

        IEnumerator Play(Vector2 localPosStart, Vector2 localPosDestination)
        {
            var strayPoint = localPosStart + Random.insideUnitCircle.normalized*Random.Range(10f, 30f);
            for (float t = 0; t < 1; t+=1.6f*Time.deltaTime)//1/1.6f的持续时间
            {
                var f0 = Curve0.Evaluate(t);
                var f1 = Curve1.Evaluate(t);
                transform.localPosition = localPosStart + (strayPoint - localPosStart)*f0 +
                                          (localPosDestination - localPosStart)*f1;

                yield return new WaitForEndOfFrame();
            }
            GameData.MyEnergy += _energy;
            Destroy(gameObject);
        }

        public static void Create(Vector2 localPos, int energy, float scale)
        {
            GameManager.Instance.PopEffectContainer.CreateEnergyLightSpot(localPos, energy, scale);
        }
        public static void Create(Vector2 localPos, int energy)
        {
            GameManager.Instance.PopEffectContainer.CreateEnergyLightSpot(localPos, energy, 1);
        }
        public static void Create(IntVector2 ij, int energy)
        {
            Create(GameManager.Instance.MyGrid.GetCellPosition(ij), energy);
        }
    }
}