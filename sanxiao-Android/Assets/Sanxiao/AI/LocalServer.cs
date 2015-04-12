using System.Collections;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.Test;
using Assets.Sanxiao.UI;
using UnityEngine;
using EndRound = Assets.Sanxiao.Communication.UpperPart.EndRound;
using RivalUseSkill = Assets.Sanxiao.Communication.UpperPart.RivalUseSkill;
using SyncData = Assets.Sanxiao.Communication.Proto.SyncData;
using UseSkill = Assets.Sanxiao.Communication.UpperPart.UseSkill;
using UseSkillOk = Assets.Sanxiao.Communication.UpperPart.UseSkillOk;

namespace Assets.Sanxiao.AI
{
    /// <summary>
    /// 对手的AI，单机状态下模拟对手
    /// </summary>
    public class LocalServer : MonoBehaviour
    {
        private User _rivalUser;
        private DefenseData _defenseData;

        public int CurrenctRivalHealth;

        public void StartPlaying(User rivalUser, DefenseData defenseData)
        {
            _rivalUser = rivalUser;
            _defenseData = defenseData;
            CurrenctRivalHealth = _rivalUser.RoundInitHealth;
            enabled = true;
            StartCoroutine(PlayingChallenge());
        }

        public void StartPlaying(int initHealth)
        {
            CurrenctRivalHealth = initHealth;
            enabled = true;
            _lastUseSkillTime = Time.realtimeSinceStartup;
        }

        public void BeAttackedBySkill(SkillEnum skillCode, int skillLevel, int physicalDamage)
        {
            CurrenctRivalHealth = Mathf.Max(CurrenctRivalHealth - physicalDamage, 0);
            if (CurrenctRivalHealth <= 0)
            {
                //游戏结束
            }
        }

        IEnumerator PlayingChallenge()
        {
            if (_defenseData != null)
            {
                Debug.Log("Play with DefenseData");
                while (GameManager.Instance.GameState != GameManager.GameStateEnum.Ending)
                {
                    foreach (UseSkillEvent e in _defenseData.SkillEventList)
                    {
                        var startWaitTime = Time.time;
                        var nextSkillEventTime = Time.time + e.TimeDelta*0.001f;
                        while (true)
                        {
                            if (Time.time >= nextSkillEventTime)
                            {
                                break;
                            }
                            GameData.RivalEnergy =
                                Mathf.RoundToInt((Time.time - startWaitTime)/(nextSkillEventTime - startWaitTime)*
                                                 (CommonData.RivalUser != null
                                                      ? CommonData.RivalUser.EnergyCapacity
                                                      : 1000));
                            yield return new WaitForEndOfFrame();
                        }
                        var cmd = new RivalUseSkill();
                        cmd.SkillCode = e.SkillCode;
                        cmd.SkillLevel = e.SkillLevel;
                        cmd.PhysicalDamage = e.PhysicalDamage;
                        var ko = GameData.MyHealth <= e.PhysicalDamage;
                        if (ko) cmd.Ko = true;
                        cmd.SyncData = new SyncData(Mathf.Max(GameData.MyHealth - e.PhysicalDamage, 0),
                                                    CurrenctRivalHealth, 0);
                        GameManager.Instance.Execute(cmd);

                        if (ko)
                        {
                            GameManager.Instance.EndChallengeRound(false);
                        }
                    }
                }
            }
        }

        private float _lastUseSkillTime;


        void Update()
        {
            if (!GameUI.Instance || !GameManager.Instance) return;
            if (_defenseData == null)//无防守数据的，单机、断线
            {
                if (Time.realtimeSinceStartup - _lastUseSkillTime > 15 && Random.value < 0.02f)
                {
                    Debug.Log("AI施放技能间隔:" + (Time.realtimeSinceStartup - _lastUseSkillTime));
                    var physicalDamage = Random.Range(180, 220);
                    _lastUseSkillTime = Time.realtimeSinceStartup;

                    var cmd = new RivalUseSkill();
                    var r = Random.value;
                    if (r < 0.4f)
                    {
                        cmd.SkillCode = (int) SkillEnum.Ice;
                        cmd.SkillLevel = 1;
                    }
                    else if (r < 0.7f)
                    {
                        cmd.SkillCode = (int) SkillEnum.ExtraDamage;
                        cmd.SkillLevel = 1;
                    }
                    else
                    {
                        cmd.SkillCode = (int) SkillEnum.Brick;
                        cmd.SkillLevel = 1;
                    }
                    cmd.PhysicalDamage = physicalDamage;
                    var ko = GameData.MyHealth <= physicalDamage;
                    if (ko) cmd.Ko = true;
                    cmd.SyncData = new SyncData(Mathf.Max(GameData.MyHealth - physicalDamage, 0),
                                                CurrenctRivalHealth, 0);
                    GameManager.Instance.Execute(cmd);

                    if (ko)
                    {
                        Responder.Instance.Execute(new EndRound
                            {
                                MyInfo = CommonData.MyUser,
                                RivalInfo = CommonData.RivalUser,
                                Win = false
                            });
                    }
                }
            }
            else//有防守数据的，挑战
            {
                
            }
        }

        public void Execute(UseSkill cmd)
        {
            CurrenctRivalHealth = Mathf.Max(CurrenctRivalHealth - cmd.PhysicalDamage, 0);
            var ko = CurrenctRivalHealth <= 0;

            var res = new UseSkillOk
                {
                    Ko = ko,
                    SkillCode = cmd.SkillCode,
                    PhysicalDamage = cmd.PhysicalDamage,
                    SyncData = new SyncData
                        {
                            MyHealth = GameData.MyHealth,
                            RivalHealth = CurrenctRivalHealth,
                            RivalEnergy = GameData.RivalEnergy,
                        }
                };
            Responder.Instance.Execute(res);

            if (ko)
            {
                if (_defenseData == null)//掉线托管或纯单机
                {
                    Responder.Instance.Execute(new EndRound
                        {
                            MyInfo = CommonData.MyUser,
                            RivalInfo = CommonData.RivalUser,
                            Win = true
                        });
                }
                else//异步挑战
                {
                    GameManager.Instance.EndChallengeRound(true);
                }
            }
        }
    }
}