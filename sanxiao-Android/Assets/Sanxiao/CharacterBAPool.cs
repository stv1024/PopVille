using System.Collections.Generic;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao
{
    public class CharacterBAPool : MonoBehaviour
    {
        private static CharacterBAPool _instance;

        public static CharacterBAPool Instance{get; private set; }

        void Awake()
        {
            Instance = this;
            
            for (int i = 0; i < CodeCharacterBAPoolList.Length; i++)
            {
                CodeCharacterBAPoolList[i] = new Queue<GameObject>();
            }
        }

        public readonly Queue<GameObject>[] CodeCharacterBAPoolList = new Queue<GameObject>[Character.MaxCharacterKindCount];

        public static void Enqueue(int code, GameObject baGo)
        {
            var index = code - 90001;
            if (index < 0 || index >= Character.MaxCharacterKindCount)
            {
                Debug.LogError("index错误，index=" + index);
                return;
            }
            if (baGo == null)
            {
                Debug.LogError("baGo == null");
                return;
            }
            Instance.CodeCharacterBAPoolList[index].Enqueue(baGo);
            baGo.transform.parent = Instance.transform;
            baGo.transform.localPosition = Vector3.zero;
            baGo.SetActive(false);
        }

        public static GameObject Dequeue(int code)
        {
            var index = code - 90001;//TODO: Fix
            if (index < 0 || index >= Character.MaxCharacterKindCount)
            {
                Debug.LogError("code错误，code=" + index);
                return null;
            }
            if (Instance.CodeCharacterBAPoolList[index].Count > 0)
            {
                var go = Instance.CodeCharacterBAPoolList[index].Dequeue();
                go.SetActive(true);
                return go;
            }
            var prefab = MorlnDownloadResources.Load<GameObject>("ResourcesForDownload/Character/Character" + code);
            return PrefabHelper.InstantiateAndReset(prefab, Instance.transform);
        }
    }
}