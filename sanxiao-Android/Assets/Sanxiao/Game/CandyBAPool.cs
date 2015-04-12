using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game
{
    public class CandyBAPool : MonoBehaviour
    {
        private static CandyBAPool _instance;

        public static CandyBAPool Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            for (int i = 0; i < GenreCandyBAPoolList.Length; i++)
            {
                GenreCandyBAPoolList[i] = new Queue<GameObject>();//初始化队列
            }
        }

        public GameObject[] CandyBoneAnimationPrefabList = new GameObject[7];
        public GameObject ColorfulCandyBoneAnimationPrefab;

        public GameObject SpriteCandyPrefab;

        public GameObject CandyPopEffectPrefab;

        /// <summary>
        /// 不同Genre的糖果BA存入这个池，千万不能放错了
        /// </summary>
        public readonly Queue<GameObject>[] GenreCandyBAPoolList = new Queue<GameObject>[7];

        /// <summary>
        /// 将CandyBA放入队列，会自动处理Hierarchy，自动失活。千万不能放错。
        /// 该方法靠baGo.name确定分类
        /// </summary>
        /// <param name="baGo"></param>
        public static void Enqueue(GameObject baGo)
        {
            if (baGo == null)
            {
                Debug.LogError("baGo == null");
                return;
            }
            var genreStr = baGo.name.Substring(0, 1);
            int genre;
            if (!int.TryParse(genreStr, out genre))
            {
                Destroy(baGo);
                return;
            }
            if (genre < 0 || genre >= 7)
            {
                Destroy(baGo);
                return;
            }

            //检查是否有误，TODO：以后删除
            if (baGo.name.Substring(0, 1) != genre.ToString())
                Debug.LogError("入队错了,genre:" + genre + "; baGo:" + baGo.name);

            Instance.GenreCandyBAPoolList[genre].Enqueue(baGo);
            baGo.transform.parent = Instance.transform;
            baGo.transform.localPosition = Vector3.zero;
            baGo.SetActive(false);
        }

        public static GameObject Dequeue(CandyInfo candyInfo)
        {
            var genre = candyInfo.Genre;
            if (genre >= 0 && genre < 7)
            {
                if (Instance.GenreCandyBAPoolList[genre].Count > 0)
                {
                    var go = Instance.GenreCandyBAPoolList[genre].Dequeue();
                    go.SetActive(true);
                    return go;
                }
                return PrefabHelper.InstantiateAndReset(Instance.CandyBoneAnimationPrefabList[genre], Instance.transform);
            }
            if (candyInfo.Type == Candy.CandyType.Colorful) return PrefabHelper.InstantiateAndReset(Instance.ColorfulCandyBoneAnimationPrefab, Instance.transform);
            return PrefabHelper.InstantiateAndReset(Instance.SpriteCandyPrefab, Instance.transform);
        }

        public void PrepareAllCandys()
        {
            return;
            //PrepareAllCandys(10);//默认10个
        }

        public void PrepareAllCandys(int countEachGenre)
        {
            for (int genre = 0; genre < Candy.MaxGenreCount; genre++)
            {
                while (GenreCandyBAPoolList[genre].Count < countEachGenre)
                {
                    Enqueue(PrefabHelper.InstantiateAndReset(Instance.CandyBoneAnimationPrefabList[genre], Instance.transform));
                }
            }
        }

        

        public Sprite SpriteStone, SpriteChest, SpriteBloodBottle, SpriteEnergyBottle, SpriteCoinSack;

        public static Sprite GetSpriteForCandy(CandyInfo candyInfo)
        {Debug.Log(candyInfo.Genre.ToString());
            if (candyInfo.Type == Candy.CandyType.Stone) return Instance.SpriteStone;
            if (candyInfo.Type == Candy.CandyType.Chest) return Instance.SpriteChest;
            if (candyInfo.Genre == 202) return Instance.SpriteBloodBottle;
            if (candyInfo.Genre == 203) return Instance.SpriteEnergyBottle;
            if (candyInfo.Genre == 204) return Instance.SpriteCoinSack;
            return null;
        }
    }
}
