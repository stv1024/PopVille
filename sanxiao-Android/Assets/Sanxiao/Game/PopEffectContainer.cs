using System.Collections;
using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 消除特效容器
    /// </summary>
    public class PopEffectContainer : MonoBehaviour
    {
        public GameObject HEffectPrefab, VEffectPrefab, Bomb3EffectPrefab, Bomb5EffectPrefab;

        public Sprite ColorfulLine;

        public void NormalPopEffect(IntVector2 ij, int genre)
        {
            //粒子特效
            var ps = PopParticleSysPool.Dequeue().transform;
            ps.name = "PopParticleSys " + ij + "," + genre;
            ps.ResetTransform(transform);
            ps.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
            ps.particleSystem.startColor = GetPopEffectColor(genre);

            ps.particleSystem.Play();
            PopParticleSysPool.Enqueue(ps.gameObject, 5);
        }

        public AudioClip StripePopEffectAudio, BombPopEffectAudio;
        public void StripeHSpecialPopEffect(IntVector2 ij, int genre)
        {
            var go = PrefabHelper.InstantiateAndReset(HEffectPrefab, transform);
            go.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
            Destroy(go, 3);

        }
        public void StripeVSpecialPopEffect(IntVector2 ij, int genre)
        {
            var go = PrefabHelper.InstantiateAndReset(VEffectPrefab, transform);
            go.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
            Destroy(go, 3);
            AudioManager.PlayOneShot(StripePopEffectAudio);
        }

        public void Bomb3SpecialPopEffect(IntVector2 ij, int genre)
        {
            var go = PrefabHelper.InstantiateAndReset(Bomb3EffectPrefab, transform);
            go.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
            Destroy(go, 3);
            AudioManager.PlayOneShot(BombPopEffectAudio);
        }
        public void Bomb5SpecialPopEffect(IntVector2 ij, int genre)
        {
            var go = PrefabHelper.InstantiateAndReset(Bomb5EffectPrefab, transform);
            go.transform.localPosition = GameManager.Instance.MyGrid.GetCellPosition(ij);
            Destroy(go, 3);
            AudioManager.PlayOneShot(BombPopEffectAudio);
        }

        public AudioClip ColorfulScatterAudio;
        public void ColorfulScatterEffect(IntVector2 ij, int genre, List<IntVector2> scatteredIJs)
        {
            StartCoroutine(ColorfulScatterEffectCoroutine(ij, genre, scatteredIJs));
        }
        IEnumerator ColorfulScatterEffectCoroutine(IntVector2 ij, int genre, List<IntVector2> scatteredIJs)
        {
            AudioManager.PlayOneShot(ColorfulScatterAudio);

            var tras = new Transform[scatteredIJs.Count];
            var grid = GameManager.Instance.MyGrid;
            for (int i = 0; i < scatteredIJs.Count; i++)
            {
                var scatteredIJ = scatteredIJs[i];
                var go = new GameObject("ColorfulLine " + ij + "→" + scatteredIJ);
                go.layer = MainRoot.MainLayer;
                go.transform.ResetTransform(transform);
                go.transform.localPosition = grid.GetCellPosition(ij);
                tras[i] = go.transform;
                var spr = go.AddComponent<SpriteRenderer>();
                spr.sprite = ColorfulLine;
                spr.color = Color.white;
            }
            var startTime = Time.time;
            const float duration = 0.4f;
            while (true)
            {
                if (Time.time > startTime + duration) break;
                var t = (Time.time - startTime)/duration;//相位，从0~1，前0.5伸长，后0.5缩短
                for (int i = 0; i < scatteredIJs.Count; i++)
                {
                    var vect = grid.GetCellPosition(scatteredIJs[i]) - grid.GetCellPosition(ij);
                    tras[i].right = vect.normalized;
                    tras[i].localScale = tras[i].localScale.SetV3X(vect.magnitude/38*(1 - Mathf.Abs(t - 0.5f)*2));
                    if (t > 0.5f)
                    {
                        tras[i].localPosition = Vector2.Lerp(grid.GetCellPosition(ij), grid.GetCellPosition(scatteredIJs[i]), (t - 0.5f) * 2);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            for (int i = 0; i < scatteredIJs.Count; i++)
            {
                Destroy(tras[i].gameObject);
            }
        }

        public void Colorful_ColorfulEffect(IntVector2 ij)
        {
            StartCoroutine(Colorful_ColorfulEffectCoroutine(ij));
        }
        IEnumerator Colorful_ColorfulEffectCoroutine(IntVector2 ij)
        {
            for (int i = 0; i < Grid.Width; i++)
            {
                var scatteredIJs = new List<IntVector2>();
                for (int j = 0; j < Grid.Height; j++)
                {
                    scatteredIJs.Add(new IntVector2(i, j));
                }
                StartCoroutine(ColorfulScatterEffectCoroutine(ij, -1, scatteredIJs));
                yield return new WaitForSeconds(0.1f);
            }
        }

        public GameObject EnergyLightSpotPrefab;
        public Transform EnergyLightSpotDestination;

        public void CreateEnergyLightSpot(Vector2 localPos, int energy, float scale)
        {
            var go = PrefabHelper.InstantiateAndReset(EnergyLightSpotPrefab, transform);
            go.GetComponent<EnergyLightSpot>().Reset(localPos, transform.InverseTransformPoint(EnergyLightSpotDestination.position), energy, scale);
        }

        /// <summary>
        /// 0~6有特别的颜色，-1白色
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
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