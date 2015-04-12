using System;
using System.Collections.Generic;
using Assets.Sanxiao;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.Test;
using Fairwood.Math;
using UnityEditor;
using UnityEngine;
using EditUserInfoResult = Assets.Sanxiao.Communication.UpperPart.EditUserInfoResult;
using EndRound = Assets.Sanxiao.Communication.UpperPart.EndRound;
using MatchOk = Assets.Sanxiao.Communication.UpperPart.MatchOk;
using NeedOAuthInfo = Assets.Sanxiao.Communication.UpperPart.NeedOAuthInfo;
using RequestChallengeOk = Assets.Sanxiao.Communication.UpperPart.RequestChallengeOk;
using RequestMyHeartInfo = Assets.Sanxiao.Communication.UpperPart.RequestMyHeartInfo;
using RivalUseSkill = Assets.Sanxiao.Communication.UpperPart.RivalUseSkill;
using StartChallenge = Assets.Sanxiao.Communication.UpperPart.StartChallenge;
using StartRound = Assets.Sanxiao.Communication.UpperPart.StartRound;
using SyncData = Assets.Sanxiao.Communication.Proto.SyncData;
using UploadChallengeOk = Assets.Sanxiao.Communication.UpperPart.UploadChallengeOk;
using UseSkillOk = Assets.Sanxiao.Communication.UpperPart.UseSkillOk;
using UserHeartInfo = Assets.Sanxiao.Communication.UpperPart.UserHeartInfo;
using UserMailList = Assets.Sanxiao.Communication.UpperPart.UserMailList;

/// <summary>
/// 服务器模拟器
/// </summary>
public class ServerSimulator : EditorWindow
{
    [MenuItem("Morln/ServerSimulator")]
    static void Init()
    {
        var editor = GetWindow<ServerSimulator>();
        editor.title = "服务器模拟器";
    }

    private static int _skillLevel = 1;
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("LoginOk"))
            {
                Responder.Instance.Execute(TestData.LoginOk0);
            }
            if (GUILayout.Button("NeedOAuthInfo"))
            {
                Responder.Instance.Execute(new NeedOAuthInfo{Type = 0, Name = "围脖昵"});
            }
            if (GUILayout.Button("Config"))
            {
                Responder.Instance.Execute(TestData.Config0);
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("UserMailList"))
        {
            Responder.Instance.Execute(new UserMailList
                {
                    MailList =
                        new List<Mail>
                            {
                                new Mail("32432" ,0, "23", "来信者依依", true, (DateTime.Now - new TimeSpan(1, 2, 3)).Ticks)
                                    {
                                        Title = "标题而yiyi",
                                        Content = "这是很重1要的事情"
                                    },
                                new Mail("3243a2" ,0, "233", "来信者尔尔", true, (DateTime.Now - new TimeSpan(13, 2, 3)).Ticks)
                                    {
                                        Title = "标题而",
                                        Content = "这是很重要的事情2"
                                    },
                                new Mail("324f32" ,0, "23", "来信者散散", false, (DateTime.Now - new TimeSpan(0, 2, 3)).Ticks)
                                    {
                                        Title = "标题而3",
                                        Content = "这是很重3要的事情"
                                    },
                                new Mail("3g2432" ,0, "23", "来信者丝丝", false, (DateTime.Now - new TimeSpan(0, 0, 3)).Ticks)
                                    {
                                        Title = "标题444",
                                        Content = "这是很重要的事情4"
                                    }
                            }
                });
        }
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("StartGuide"))
            {
                FreshmanGuide.StartGuide();
                MusicManager.Instance.CrossFadeOut();
            }
            if (GUILayout.Button("EditUserInfoResult"))
            {
                Responder.Instance.Execute(new EditUserInfoResult {NicknameResult = new MsgResult(10000)});
            }
            if (GUILayout.Button("Set JustUnlockedSubId"))
            {
                CommonData.JustUnlockedSubLevelId = new IntVector2(1, 3);
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("MatchResult"))
        {
            Responder.Instance.Execute(new MatchOk { RivalInfo = new User { Nickname = "敌鸡", }, StartSeconds = 4 });
        }
        if (GUILayout.Button("StartRound"))
        {
            Responder.Instance.Execute(new StartRound());
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("I Use GoFi"))
            {
                GameManager.Instance.ThreeSkills[0] = SkillEnum.GoldenFinger;
                GameManager.Instance.UseSelectedSkill(0);
            }
            if (GUILayout.Button("Ice"))
            {
                GameManager.Instance.ThreeSkills[0] = SkillEnum.Ice;
                GameManager.Instance.UseSelectedSkill(0);
            }
            if (GUILayout.Button("Lock"))
            {
                GameManager.Instance.ThreeSkills[0] = SkillEnum.Lock;
                GameManager.Instance.UseSelectedSkill(0);
            }
            if (GUILayout.Button("Bricks"))
            {
                GameManager.Instance.ThreeSkills[0] = SkillEnum.Brick;
                GameManager.Instance.UseSelectedSkill(0);
            }
            if (GUILayout.Button("ExtraDamage"))
            {
                GameManager.Instance.ThreeSkills[0] = SkillEnum.ExtraDamage;
                GameManager.Instance.UseSelectedSkill(0);
            }
        }
        EditorGUILayout.EndHorizontal();

        _skillLevel = EditorGUILayout.IntSlider("技能等级", _skillLevel, 1, 10);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("RivalUse Ice"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.Ice, SkillLevel = _skillLevel });
            }
            if (GUILayout.Button("Shake"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.Shake, SkillLevel = _skillLevel });
            }
            if (GUILayout.Button("Stone"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.Stone, SkillLevel = _skillLevel });
            }
            if (GUILayout.Button("Lock"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.Lock, SkillLevel = _skillLevel });
            }
            if (GUILayout.Button("Bricks"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.Brick, SkillLevel = _skillLevel });
            }
            if (GUILayout.Button("ExtraDamage"))
            {
                Responder.Instance.Execute(new RivalUseSkill { SkillCode = (int)SkillEnum.ExtraDamage, SkillLevel = _skillLevel });
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Rival.Ice.KO"))
            {
                Responder.Instance.Execute(new RivalUseSkill
                    {
                        SkillCode = (int)SkillEnum.Ice,
                        SkillLevel = 2,
                        PhysicalDamage = 200,
                        Ko = true,
                        SyncData = new SyncData { MyHealth = 0, RivalEnergy = 0, RivalHealth = 235, }
                    });
            }
            if (GUILayout.Button("UseSkillOk.KO"))
            {
                Responder.Instance.Execute(new UseSkillOk
                    {
                        SkillCode = (int)SkillEnum.Ice,
                        PhysicalDamage = 200,
                        Ko = true,
                        SyncData = new SyncData { MyHealth = 320, RivalEnergy = 236, RivalHealth = 0, }
                    });
            }
            if (GUILayout.Button("EndRound"))
            {
                Responder.Instance.Execute(new EndRound
                    {
                        MyInfo = TestData.User0,
                        RivalInfo = TestData.User1,
                        Win = true
                    });
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("RequestMyHeartInfo"))
            {
                Requester.Instance.Send(new RequestMyHeartInfo());
            }
            if (GUILayout.Button("UserHeartInfo"))
            {
                Responder.Instance.Execute(new UserHeartInfo { Count = 3, NextNeedTime = 212402 });
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Leaderboard"))
            {
                Responder.Instance.Execute(TestData.Leaderboard0);
            }
            if (GUILayout.Button("RequestChallengeOk"))
            {
                Responder.Instance.Execute(new RequestChallengeOk
                {
                    ChallengeId = "04F3E163-1286-4657-B743-FAAB42096444",
                    BossData = TestData.DefenseData0,
                    FellowDataList = new List<TeamAdd>{TestData.TeamAdd0, TestData.TeamAdd1},
                });
            }
            if (GUILayout.Button("StartChallenge"))
            {
                Responder.Instance.Execute(new StartChallenge
                    {
                        ChallengeId = "04F3E163-1286-4657-B743-FAAB42096444",
                        FriendDataList = new List<TeamAdd> { TestData.TeamAdd0, TestData.TeamAdd1 },
                    });
            }
            if (GUILayout.Button("UploadChallengeOk"))
            {
                Responder.Instance.Execute(new UploadChallengeOk
                    {
                        ChallengeId = "CAECEB40-F4A0-4BAD-8B02-D2FAF87152B9",
                        UnlockElement =
                            new UnlockElement
                                {
                                    EnergyCapacityUp = new EnergyCapacityUp(1, 2, 214, 325),
                                    LevelUp = new LevelUp(),
                                    SkillUnlockList = new List<SkillUnlock> {new SkillUnlock(205)},
                                    MajorLevelUnlockList = new List<MajorLevelUnlockInfo>{new MajorLevelUnlockInfo(2, true)}
                                },
                        RoundRewardList = new List<Currency>(),
                        StarCount = 2,
                    });
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    
}