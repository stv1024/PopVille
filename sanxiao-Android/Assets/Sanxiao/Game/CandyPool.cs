using System.Collections;
using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

    public class CandyPool : MonoBehaviour
    {
        private static CandyPool _instance;

        public static CandyPool Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public GameObject CandyPrefab;

        /// <summary>
        /// 池
        /// </summary>
        public readonly Queue<GameObject> CandyPoolQueue = new Queue<GameObject>();

        /// <summary>
        /// 将没有BA的Candy放入队列，会自动处理Hierarchy，自动失活。千万要先拿走BA。
        /// </summary>
        /// <param name="go"></param>
        public static void Enqueue(GameObject go)
        {
            if (go == null)
            {
                Debug.LogError("go == null");
                return;
            }
            if (go.transform.parent == Instance.transform)//早已入队了
            {
                return;
            }

            Instance.CandyPoolQueue.Enqueue(go);
            go.transform.parent = Instance.transform;
            go.SetActive(false);
        }

        public static void Enqueue(GameObject go, float delay)
        {
            Instance.StartCoroutine(EnqueueCoroutine(go, delay));
        }

        static IEnumerator EnqueueCoroutine(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (go != null) Enqueue(go);
        }

        public static GameObject Dequeue()
        {
            if (Instance.CandyPoolQueue.Count > 0)
            {
                var go = Instance.CandyPoolQueue.Dequeue();
                go.SetActive(true);
                return go;
            }
            return PrefabHelper.InstantiateAndReset(Instance.CandyPrefab, Instance.transform);
        }

        public void Preload(int count = 20)
        {
            while (CandyPoolQueue.Count < count)
            {
                Enqueue(PrefabHelper.InstantiateAndReset(Instance.CandyPrefab, Instance.transform));
            }
        }
    }
