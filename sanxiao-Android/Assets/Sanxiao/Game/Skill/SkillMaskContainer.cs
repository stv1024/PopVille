using System.Collections.Generic;
using System.Linq;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// ÕÚÕÖ¼¼ÄÜ²ãÈÝÆ÷
    /// </summary>
    public class SkillMaskContainer : MonoBehaviour
    {
        //public static SkillMaskContainer Instance { get; private set; }
        //void Awake()
        //{
        //    Instance = this;
        //}

        public Transform RespawnParent { get { return transform; } }

        public GameObject IceMaskPrefab;

        public List<BaseSkillMask> SkillMasks = new List<BaseSkillMask>();

        public bool CanTouchThroughAllMasks(Vector2 localPos)
        {
            return SkillMasks.All(skillMask => skillMask == null || skillMask.CanTouchThroughThisMask(localPos));
        }

        public void Show(GameObject skillMaskPrefab, params int[] parameters)
        {
            var cs = PrefabHelper.InstantiateAndReset<BaseSkillMask>(skillMaskPrefab, RespawnParent);
            cs.Show(parameters);
            SkillMasks.Add(cs);
        }

        public void DidDestroy(BaseSkillMask skillMask)
        {
            if (skillMask == null) return;
            SkillMasks.Remove(skillMask);
        }
    }
}