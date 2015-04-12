using System;
using Assets.Sanxiao;
using Fairwood.Math;
using UnityEngine;

/// <summary>
/// 角色被施以技能的效果
/// </summary>
public class CharacterSkillEffectContainer : MonoBehaviour
{
    public GameObject IcePrefab;
    public GameObject BrickPrefab;
    public GameObject ExtraDamagePrefab;

    GameObject GetPrefab(SkillEnum skill)
    {
        switch (skill)
        {
            case SkillEnum.Ice:
                return IcePrefab;
            case SkillEnum.Brick:
                return BrickPrefab;
            case SkillEnum.ExtraDamage:
                return ExtraDamagePrefab;
            default:
                Debug.LogError("还未添加Prefab:" + skill);
                return null;
        }
    }

    public void ShowEffect(SkillEnum skill)
    {
        var prefab = GetPrefab(skill);
        if (prefab == null) return;
        var go = PrefabHelper.InstantiateAndReset(prefab, transform);
        Destroy(go, 10);
    }
}