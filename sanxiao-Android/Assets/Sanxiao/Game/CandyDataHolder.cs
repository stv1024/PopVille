using UnityEngine;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 用于获取各种CandyAnimation的Prefab引用
    /// </summary>
    public class CandyDataHolder : MonoBehaviour
    {
        public static CandyDataHolder Instance { get; private set; }
        void Awake()
        {
            Instance = this;
        }

        public GameObject[] CandyBoneAnimationPrefabList = new GameObject[Candy.MaxGenreCount];

        public GameObject GetBoneAnimationPrefab(int genre)
        {
            return CandyBoneAnimationPrefabList[genre];//TODO:防止出错
        }
    }
}