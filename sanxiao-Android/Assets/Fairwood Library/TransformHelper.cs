using System.Linq;
using UnityEngine;

namespace Fairwood.Math
{
    public static class TransformHelper
    {

        public static void ResetTransform(this Transform tra)
        {
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one;
        }

        public static void ResetTransform(this Transform tra, Transform parent)
        {
            tra.parent = parent;
            tra.localPosition = Vector3.zero;
            tra.localRotation = Quaternion.identity;
            tra.localScale = Vector3.one;
        }

        /// <summary>
        /// 使用target的parent
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="target"></param>
        public static void ResetAs(this Transform tra, Transform target)
        {
            tra.parent = target.parent;
            tra.localPosition = target.localPosition;
            tra.localRotation = target.localRotation;
            tra.localScale = target.localScale;
        }

        /// <summary>
        /// 使用指定的parent
        /// </summary>
        /// <param name="tra"></param>
        /// <param name="target"></param>
        /// <param name="parent"></param>
        public static void ResetAs(this Transform tra, Transform target, Transform parent)
        {
            tra.parent = parent;
            tra.localPosition = target.localPosition;
            tra.localRotation = target.localRotation;
            tra.localScale = target.localScale;
        }

        public static void SetLayer(Transform tra, int layer)
        {
            tra.gameObject.layer = layer;
            for (int i = 0, imax = tra.childCount; i < imax; i++)
            {
                SetLayer(tra.GetChild(i), layer);
            }
        }

        public static Transform FindChildRecursively(this Transform tra,string name)
        {
            return tra.name == name ? tra : (from Transform sub in tra select sub.FindChildRecursively(name)).FirstOrDefault(cur => cur);
        }

        public static void SetSortingLayer(this Transform tra, string name)
        {
            var sprs = tra.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var spriteRenderer in sprs)
            {
                spriteRenderer.sortingLayerName = name;
            }
        }
    }
}