using System.Collections;
using System.Collections.Generic;
using Fairwood.Math;
using UnityEngine;

    public class PopParticleSysPool : MonoBehaviour
    {
        private static PopParticleSysPool _instance;

        public static PopParticleSysPool Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public GameObject PopParticleSysPrefab;

        /// <summary>
        /// 池
        /// </summary>
        public readonly Queue<GameObject> PopParticleSysPoolList = new Queue<GameObject>();

        /// <summary>
        /// 将PopParticle放入队列，会自动处理Hierarchy，自动失活。
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

            Instance.PopParticleSysPoolList.Enqueue(go);
            go.transform.parent = Instance.transform;
            //go.transform.localPosition = Vector3.zero;
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
            if (Instance.PopParticleSysPoolList.Count > 0)
            {
                var go = Instance.PopParticleSysPoolList.Dequeue();
                go.SetActive(true);
                return go;
            }
            return PrefabHelper.InstantiateAndReset(Instance.PopParticleSysPrefab, Instance.transform);
        }

        public void Preload(int count = 20)
        {
            while (PopParticleSysPoolList.Count < count)
            {
                Enqueue(PrefabHelper.InstantiateAndReset(Instance.PopParticleSysPrefab, Instance.transform));
            }
        }
    }
