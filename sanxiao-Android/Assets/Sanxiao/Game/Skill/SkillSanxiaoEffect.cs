using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 不同技能的策略类
    /// </summary>
    public class SkillSanxiaoEffect : MonoBehaviour
    {

        /// <summary>
        /// 必须在创建时立即调用，必须设置gameManager
        /// </summary>
        /// <param name="skillCode"></param>
        /// <param name="level"></param>
        /// <param name="gameManager"></param>
        public void Initialize(SkillEnum skillCode, int level, GameManager gameManager)
        {
            _skillCode = skillCode;
            _level = level;
            _gameManager = gameManager;
        }

        SkillEnum _skillCode;
        int _level;
        GameManager _gameManager;

        /// <summary>
        /// 从施法者播放扔技能动画开始
        /// </summary>
        protected void Start()
        {
            if (_gameManager == null)
            {
                Debug.LogError("没有Initialize就Start了");
                Destroy(this);
                return;
            }
            var skillParameterConfig =
                        ConfigManager.GetConfig(ConfigManager.ConfigType.SkillParameterConfig) as SkillParameterConfig;
            SkillParameter curSkillConfig = null;
            if (skillParameterConfig != null)
            {
                curSkillConfig = skillParameterConfig.SkillParameterList.Find(x => (SkillEnum)x.SkillCode == _skillCode);
            }
            else
            {
                Debug.LogError("没有skillParameterConfig");
            }
            List<IntVector2> sanxiaoTakeEffectCells = null;
            switch (_skillCode)
            {
                case SkillEnum.CreateStripe:
                    #region 101
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        sanxiaoTakeEffectCells = _gameManager.MyGrid.CreateStripe(count);
                    }
                    break;
                    #endregion
                case SkillEnum.CreateBomb:
                    #region 102
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        sanxiaoTakeEffectCells = _gameManager.MyGrid.CreateBomb(count);
                    }
                    break;
                    #endregion
                case SkillEnum.CreateColorful:
                    #region 103
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        sanxiaoTakeEffectCells = _gameManager.MyGrid.CreateColorful(count);
                    }
                    break;
                    #endregion
                case SkillEnum.AllHint:
                    #region 104
                    {
                        var time = 0.3f;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            time = curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level;
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        _gameManager.PlaySkillEffect_AllHints(time);
                    }
                    break;
                    #endregion
                case SkillEnum.HideGenre:
                    #region 105
                    {
                        var time = 1f;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            time = curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level;
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        _gameManager.PlaySkillEffect_HideGenre(time);
                    }
                    break;
                    #endregion
                case SkillEnum.GoldenFinger:
                    #region 106
                    {
                        var time = 0.1f;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            time = curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level;
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        _gameManager.MyGrid.StartGoldenFinger(time);
                    }
                    break;
                    #endregion
                case SkillEnum.Ice:
                    #region 202
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        _gameManager.SkillMaskContainer.Show(_gameManager.SkillMaskContainer.IceMaskPrefab, count);
                    }
                    break;
                    #endregion
                case SkillEnum.Shake:
                    #region 204
                    {
                        var amplitude = 0.01f;
                        var time = 1f;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            amplitude = curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level;
                            time = curSkillConfig.ConstantList[2] + curSkillConfig.ConstantList[3] * _level;
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        UIShake.ShakeAUI(GameUI.Instance.gameObject, amplitude, time);
                    }
                    break;
                    #endregion
                case SkillEnum.Stone:
                    #region 205
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        _gameManager.MyGrid.AddStoneToQueue(count);
                    }
                    break;
                    #endregion
                case SkillEnum.Lock:
                    #region 206
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1]*_level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        sanxiaoTakeEffectCells = _gameManager.MyGrid.FormLocks(count);
                    }
                    break;
                    #endregion
                case SkillEnum.Brick:
                    #region 207
                    {
                        var count = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            count =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1]*_level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        sanxiaoTakeEffectCells = _gameManager.MyGrid.FormBricks(count);
                    }
                    break;
                    #endregion
                case SkillEnum.ReduceEnergy:
                    #region 209
                    {
                        var reduce = 1;
                        if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                        {
                            reduce =
                                Mathf.FloorToInt(curSkillConfig.ConstantList[0] + curSkillConfig.ConstantList[1] * _level);
                        }
                        else
                        {
                            Debug.LogError("没有技能参数配置或常数数量不对Code:" + _skillCode);
                        }
                        GameData.MyEnergy = Mathf.Max(0, GameData.MyEnergy - reduce);
                    }
                    break;
                    #endregion
                default:
                    Debug.LogError("没有就别调用，浪费CPU！");
                    Destroy(this);
                    break;
            }

            if (sanxiaoTakeEffectCells != null && sanxiaoTakeEffectCells.Count > 0)
            {
                var bulletPrefab =
                    MorlnResources.Load<GameObject>("UI/GameUI/SkillScatterEffects/ScatterEffectBullet-" + _skillCode);
                if (bulletPrefab)
                {
                    var trackPrefab =
                        MorlnResources.Load<GameObject>("UI/GameUI/SkillScatterEffects/ScatterEffectTrack");
                    if (trackPrefab)
                    {
                        foreach (var sanxiaoTakeEffectCell in sanxiaoTakeEffectCells)
                        {
                            var track = PrefabHelper.InstantiateAndReset<SkillScatterEffectTrackInfo>(trackPrefab,
                                                                                                      transform);
                            track.transform.localPosition =
                                transform.InverseTransformPoint(
                                    GameManager.Instance.FightingPanel.transform.TransformPoint(
                                        GameManager.Instance.SkillEffectInfo.GetScatterPoint(_skillCode)));
                            var dir =
                                GameManager.Instance.transform.InverseTransformPoint(
                                    GameManager.Instance.MyGrid.transform.TransformPoint(
                                        GameManager.Instance.MyGrid.GetCellPosition(sanxiaoTakeEffectCell))) -
                                track.transform.localPosition;
                            track.transform.right = dir;
                            track.transform.localScale = (Vector2.one*dir.magnitude/500).ToVector3(1);
                            var bullet = PrefabHelper.InstantiateAndReset(bulletPrefab, track.TrackPoint);

                            Destroy(track.gameObject, track.Lifespan);
                        }
                    }
                }
            }
            switch (_skillCode)
            {
                //TODO:需要特别设置销毁时间的在这里添加代码
                default:
                    Destroy(this);
                    break;
            }
        }
    }
}