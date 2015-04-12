using System;
using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEditor;
using UnityEngine;

namespace Assets.Sanxiao.Editor
{
    public class SkillEffectInfoHolderEditor : EditorWindow
    {
        [MenuItem("Morln/动画时间信息编辑器-动画师")]
        static void Init()
        {
            GetWindow<SkillEffectInfoHolderEditor>().title = "动画时间信息编辑器";

            _target =
                (AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/Skill Effect Info Holder.prefab",
                                               typeof (GameObject)) as GameObject).GetComponent<SkillEffectInfoHolder>();
        }

        private static SkillEffectInfoHolder _target;
        private static bool _showMoreButton = false;

        private void OnGUI()
        {
            if (!_target)
            {
                _target =
                    (AssetDatabase.LoadAssetAtPath("Assets/Resources/Data/Skill Effect Info Holder.prefab",
                                                   typeof(GameObject)) as GameObject).GetComponent<SkillEffectInfoHolder>();
            }

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("点击右边按钮后还要Save Project");

                if (GUILayout.Button("我修改过了")) //会保留数据并排序
                {
                    EditorUtility.SetDirty(_target);
                }
            }
            EditorGUILayout.EndHorizontal();

            _target.PlayTrajectoryEffectTime = EditorGUILayout.FloatField("开始→弹道特效的时间", _target.PlayTrajectoryEffectTime,
                                                                          GUILayout.Width(250));
            _target.TimeFromPlayAttackToRivalBeAttacked = EditorGUILayout.FloatField("开始→对方受击的时间",
                                                                                     _target
                                                                                         .TimeFromPlayAttackToRivalBeAttacked,
                                                                                     GUILayout.Width(250));
            _target.BeAttackedLength = EditorGUILayout.FloatField("物理受击动画的长度", _target.BeAttackedLength,
                                                                  GUILayout.Width(250));
            EditorGUILayout.LabelField("*以下时刻都是在[播放弹道特效时刻]基础上计算的");
            EditorGUILayout.LabelField("*设为-1表示没有这种效果");
            EditorGUILayout.LabelField("|----技能名称----|-魔受击时刻-|弹道特效长度|散落特效时刻|开发优先级|");

            for (int i = 0; i < _target.SkillCodeList.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(_target.SkillCodeList[i].ToString(), GUILayout.Width(90));
                    _target.BeMagicallyAttackTimeList[i] =
                        EditorGUILayout.FloatField(_target.BeMagicallyAttackTimeList[i], GUILayout.Width(67));
                    _target.TrajectoryAnimationLengthList[i] =
                        EditorGUILayout.FloatField(_target.TrajectoryAnimationLengthList[i], GUILayout.Width(67));
                    _target.PlaySanxiaoEffectTimeList[i] =
                        EditorGUILayout.FloatField(_target.PlaySanxiaoEffectTimeList[i], GUILayout.Width(67));
                    _target.ScatterPointList[i] = new Vector2(-172, 50);
                    if (new[] {101, 102, 202, 206, 207}.Contains((int) _target.SkillCodeList[i]))
                    {
                        EditorGUILayout.LabelField("高");
                    }
                    else if (new[] {103, 204, 205, 209}.Contains((int) _target.SkillCodeList[i]))
                    {
                        EditorGUILayout.LabelField("中");
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("打开技能效果测试器", GUILayout.Width(120)))
            {
                SkillEffectSimulator.Init();
            }
            _showMoreButton = EditorGUILayout.Foldout(_showMoreButton, "高级...");
            if (_showMoreButton)
            {
                if (GUILayout.Button("Refresh", GUILayout.Width(60))) //会保留数据并排序
                {
                    var skillCodes = Enum.GetValues(typeof (SkillEnum)) as SkillEnum[];
                    var newSkillCodes = new SkillEnum[skillCodes.Length];
                    var newBMATs = new float[skillCodes.Length];
                    var newTALs = new float[skillCodes.Length];
                    var newPSETs = new float[skillCodes.Length];
                    var newSPs = new Vector2[skillCodes.Length];

                    for (int i = 0; i < skillCodes.Length; i++)
                    {
                        newSkillCodes[i] = skillCodes[i];
                        var oldi = _target.SkillCodeList.ToList().FindIndex(x => x == newSkillCodes[i]);
                        if (oldi >= 0)
                        {
                            newBMATs[i] = _target.BeMagicallyAttackTimeList[oldi];
                            newTALs[i] = _target.TrajectoryAnimationLengthList[oldi];
                            newPSETs[i] = _target.PlaySanxiaoEffectTimeList[oldi];
                            newSPs[i] = _target.ScatterPointList[oldi];
                        }
                    }

                    _target.SkillCodeList = newSkillCodes;
                    _target.BeMagicallyAttackTimeList = newBMATs;
                    _target.TrajectoryAnimationLengthList = newTALs;
                    _target.PlaySanxiaoEffectTimeList = newPSETs;
                    _target.ScatterPointList = newSPs;
                }
            }
        }

        void ResetFirstTime()//会清除所有数据，慎用
        {
            var skillCodes = Enum.GetValues(typeof(SkillEnum)) as SkillEnum[];
            _target.SkillCodeList = new SkillEnum[skillCodes.Length];
            for (int i = 0; i < skillCodes.Length; i++)
            {
                _target.SkillCodeList[i] = skillCodes[i];
            }
            _target.BeMagicallyAttackTimeList = new float[skillCodes.Length];
            _target.TrajectoryAnimationLengthList = new float[skillCodes.Length];
            _target.PlaySanxiaoEffectTimeList = new float[skillCodes.Length];
        }
    }
}