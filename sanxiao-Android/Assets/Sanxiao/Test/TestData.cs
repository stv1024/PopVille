using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Test
{
    public class TestData
    {
        public static Communication.UpperPart.Config Config0
        {
            get
            {
                return new Communication.UpperPart.Config
                {
                    SkillConfig = new SkillConfig("A5A532EB-490C-48EC-B6EF-DF92713CD044")
                    {
                        SkillList = new List<Skill>
                        {
                            //new Skill(102,1,0,1){UnlockLevelList = new List<int>{1,2,4,7},UpgradeCostList = new List<MoneyCost>{new MoneyCost(0,200),new MoneyCost(0,500),new MoneyCost(0,1000),new MoneyCost(0,2000)}},
                            new Skill(103){UnlockLevelList = new List<int>{1,3,5,8},UpgradeCostList = new List<Currency>{new Currency(0,500),new Currency(0,1000),new Currency(0,1000),new Currency(1,200)}},
                            //new Skill(201,2,50,1){UnlockLevelList = new List<int>{1,3,5,8},UpgradeCostList = new List<MoneyCost>{new MoneyCost(0,500),new MoneyCost(0,1000),new MoneyCost(0,1000),new MoneyCost(1,200)}},
                            new Skill(202){UnlockLevelList = new List<int>{1,2,4,7},UpgradeCostList = new List<Currency>{new Currency(0,200),new Currency(0,500),new Currency(0,1000),new Currency(0,2000)}},
                            new Skill(207){UnlockLevelList = new List<int>{1,3,5,7},UpgradeCostList = new List<Currency>{new Currency(1,20),new Currency(1,50),new Currency(1,200),new Currency(1,500)}},
                            //new Skill(208,2,50,1){UnlockLevelList = new List<int>{1,4,8,12},UpgradeCostList = new List<MoneyCost>{new MoneyCost(0,500),new MoneyCost(0,2000),new MoneyCost(0,4000),new MoneyCost(1,400)}},
                            //new Skill(209,2,50,1){UnlockLevelList = new List<int>{1,4,8,12},UpgradeCostList = new List<MoneyCost>{new MoneyCost(0,500),new MoneyCost(0,2000),new MoneyCost(0,4000),new MoneyCost(1,400)}},
                            //new Skill(210,3,5,1){UnlockLevelList = new List<int>{1,3,5,7},UpgradeCostList = new List<MoneyCost>{new MoneyCost(1,20),new MoneyCost(1,50),new MoneyCost(1,200),new MoneyCost(1,500)}},
                        }
                    },

                    SkillParameterConfig = new SkillParameterConfig("F7C2C1EE-9F0B-41BA-9757-DD1CFCB42694")
                    {
                        SkillParameterList = new List<SkillParameter>
                        {
                            new SkillParameter(103){ConstantList = new List<float>{1,1}},
                            new SkillParameter(202){ConstantList = new List<float>{3,2}},
                            new SkillParameter(207){ConstantList = new List<float>{0,1}},
                            new SkillParameter(208){ConstantList = new List<float>{30,20}},
                        }
                    },

                    ExchangeConfig = new ExchangeConfig("39F520DF-99A6-424C-9C13-29C2B0F53ACE")
                    {
                        ExchangeList = ExchangeList0
                    },
                    RechargeConfig = new RechargeConfig("715695FC-E363-4A27-A503-7D2F541AD7CB")
                    {
                        RechargeList = RechargeList0
                    },

                    ChallengeLevelConfig = new ChallengeLevelConfig("A499CD0A-DDC5-4E8C-A099-DEF09CE81EA7")
                    {
                        MajorLevelList = new List<MajorLevelData>
                        {
                            new MajorLevelData(1, "北方牧场"){SubLevelList = new List<SubLevelData>
                                {
                                    new SubLevelData(1, "变种小鸡", true, 7, 7){GridConfigList = new List<int>
                                    {0,1,1,1,1,1,0,
                                    1,1,1,1,1,1,1,
                                    1,1,1,1,1,12,1,
                                    1,11,1,0,1,11,1,
                                    1,12,1,1,1,1,1,
                                    1,1,1,1,1,1,1,
                                    0,1,1,1,1,1,0,
                                    }},
                                    new SubLevelData(2, "变种大鹅", true, 5, 5){GridConfigList = new List<int>
                                    {0,1,1,1,1,1,0,
                                    1,1,1,1,1,11,1,
                                    1,1,1,1,1,12,1,
                                    1,11,1,0,1,11,1,
                                    1,12,1,1,1,1,1,
                                    1,11,1,1,1,1,1,
                                    0,1,0,1,0,1,0,
                                    }},
                                }},
                        }
                    },

                    VegetableConfig = new VegetableConfig("E76DBBE4-3C82-4F04-96FB-28A3C2BF0B83")
                        {
                            VegetableList = new List<Vegetable>
                                {
                                    new Vegetable(0, 5){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(1, 3){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(2, 5){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(3, 6){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(4, 7){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(5, 5){LevelEnergyList = new List<int>{10,12,14,16}},
                                    new Vegetable(6, 8){LevelEnergyList = new List<int>{10,12,14,16}},
                                }
                        },

                        CharacterConfig = new CharacterConfig("0BB509A6-4625-4BDC-B1EF-899274D38643")
                            {
                                CharacterList = new List<Communication.Proto.Character>
                                    {
                                        new Communication.Proto.Character(90001,1250),
                                        new Communication.Proto.Character(90002, 1360)
                                    }
                            }
                };
            }
        }

        public static Communication.UpperPart.LoginOk LoginOk0
        {
            get
            {
                return new Communication.UpperPart.LoginOk
                    {
                        MyUserInfo = User0,
                        MySkillList =
                            new List<UserSkill> {new UserSkill(201, 1), new UserSkill(202, 2), new UserSkill(207, 1)},
                        MyVegetableList = new List<UserVegetable>
                            {
                                new UserVegetable(0, 4, 1),
                                new UserVegetable(1, 5, 2),
                                new UserVegetable(2, 4, 3),
                                new UserVegetable(3, 0, 0),
                            },
                        MyCharacterList = new List<UserCharacter>
                            {
                                new UserCharacter(90001) {WearEquipList = new List<int> {1, 10001, 30001}}
                            },
                        MyEquipList = new List<UserEquip>
                            {
                                new UserEquip(1, 2),
                                new UserEquip(10001, 1),
                                new UserEquip(20001, 0),
                                new UserEquip(30001, 4),
                                new UserEquip(40001, 5),
                            },
                        ChallengeUnlockInfoList =
                            new List<MajorLevelUnlockInfo>
                                {
                                    new MajorLevelUnlockInfo(1, true)
                                        {
                                            SubLevelUnlockInfoList =
                                                new List<SubLevelUnlockInfo>
                                                    {
                                                        new SubLevelUnlockInfo(1, 1, 1, true),
                                                        new SubLevelUnlockInfo(1, 2, 2, true),
                                                        new SubLevelUnlockInfo(1, 3, 3, true)
                                                    }
                                        },
                                    new MajorLevelUnlockInfo(2, false)
                                        {
                                            SubLevelUnlockInfoList =
                                                new List<SubLevelUnlockInfo>
                                                    {
                                                        new SubLevelUnlockInfo(2, 1, 1, true),
                                                        new SubLevelUnlockInfo(2, 2, 0, true),
                                                        new SubLevelUnlockInfo(2, 3, 0, false)
                                                    }
                                        },
                                },
                        ConfigHash = new ConfigHash {ConfigHashList = new List<string>()},
                        SnsFriendUnlockInfoList = new List<SNSFriendUnlockInfo>
                            {
                                new SNSFriendUnlockInfo(0,"124325234","9CB497B5-ED5D-4BCC-897E-A7C725ADBDE4","微博好友〇",1,2){HeadIconUrl = "http://tx.haiqq.com/uploads/allimg/120804/222I934A-0.jpg"},
                                new SNSFriendUnlockInfo(0,"643543234","9CB497B5-EBCC-897E-A7C725ADBDE4","微博yiyi",1,6){HeadIconUrl = "http://www.qqw21.com/article/uploadpic/2012-9/201292023210999.jpg"},
                                new SNSFriendUnlockInfo(1,"214329534","97B5-ED5D-4BCC-897E-A7C725ADBDE4","朋友二",2,3),
                            }
                    };
            }
        }

        public static SkillIntroTextConfig SkillIntroTextConfig0
        {
            get
            {
                return new SkillIntroTextConfig
                    {
                        Hash = "69725AF6-1FD2-423D-9D5C-D94E1AB67381",
                        SkillCodeList = new List<int> {103, 202, 207},
                        DisplayNameList = new List<string> {"放虫子", "冰封", "扔砖头"},
                        IntroList = new List<string> {"生成彩色糖", "冰霜很有用的", "给对手砌砖墙"}
                    };
            }
        }

        public static SkillLevelDetailTextConfig SkillLevelDetailTextConfig0
        {
            get
            {
                return new SkillLevelDetailTextConfig
                {
                    Hash = "5A20BA38-4722-4313-8A7D-ACB53E810A07",
                    DetailList = new List<SkillLevelDetail>
                        {
                            new SkillLevelDetail(103){LevelDetailList = new List<string>{"变出2只彩色虫","变出3只彩色虫","变出4只彩色虫"}},
                            new SkillLevelDetail(202){LevelDetailList = new List<string>{"产生5个冰块","产生7个冰块","产生9个冰块"}},
                            new SkillLevelDetail(207){LevelDetailList = new List<string>{"扔1个砖块","扔2个砖块","扔3个砖块"}},
                        }
                };
            }
        }

        public static List<Exchange> ExchangeList0 = new List<Exchange>
            {
                new Exchange{Name = "79874adas", FromType = 1, FromAmount = 10, ToType = 10, ToAmount = 3200},
                new Exchange{Name = "345346234", FromType = 1, FromAmount = 50, ToType = 10, ToAmount = 17000},
                new Exchange{Name = "345374552", FromType = 1, FromAmount = 200, ToType = 10, ToAmount = 75000},
                new Exchange{Name = "235433466", FromType = 1, FromAmount = 1000, ToType = 10, ToAmount = 400000},
                new Exchange{Name = "263464fg37", FromType = 1, FromAmount = 5000, ToType = 10, ToAmount = 2100000},
                
                new Exchange{Name = "753252343", FromType = 1, FromAmount = 20, ToType = 1000, ToAmount = 5},
                new Exchange{Name = "135325423", FromType = 1, FromAmount = 60, ToType = 1000, ToAmount = 30},
            };

        public static List<Recharge> RechargeList0 = new List<Recharge>
            {
                new Recharge("0B6B0E13-D6B2-4578-BC74-C7FC07EC4D41", 6, 1, 60),
                new Recharge("A17AD023-92BD-4ACF-AA5E-148D79E89E7A", 30, 1, 320),
                new Recharge("338ADE91-3505-4FAB-9A38-A2E1A425EC0F", 68, 1, 760),
                new Recharge("D4D95BFA-A06A-4C44-B579-D2CCE17FC150", 168, 1, 1900),
                new Recharge("8B56BA17-5EEA-4108-B3A1-EDBC4549D5F4", 328, 1, 3820),
                new Recharge("1F2B84A0-211C-4891-8A74-395E9A46FCA1", 648, 1, 7780),
            };

        public static readonly User User0 = new User("8CFF536A-E2BB-4669-B1AE-81629A935B35", "我就是我", 3, 90001,
                                                     3250, 3200, 35) {Exp = 234, ExpCeil = 500, Money1 = 96, Money10 = 6464};
        public static readonly User User1 = new User("60FE1C3B-6522-4E20-A70C-2F3CDA013695", "敌鸡", 3, 90002, 120, 242, 12);

        public static Communication.UpperPart.Leaderboard Leaderboard0 = new Communication.UpperPart.Leaderboard
            {
                Type = 0,
                MyItem = new LeaderboardItem(12342, 12342, "新手鸡", "8CFF536A-E2BB-4669-B1AE-81629A935B35"),
                ItemList = new List<LeaderboardItem>
                    {
                        new LeaderboardItem(1,1,"线上测试","529733673657c711077859f8"),
                        new LeaderboardItem(2,2,"超级玩家","46391DE8-2BBC-4BFB-AAE5-31B3115F6684"),
                        new LeaderboardItem(3,3,"超级玩家","46391DE8-2BBC-4BFB-AA95-31B31S5F6684"),
                        new LeaderboardItem(4,4,"超级玩家","463934E8-2BBC-4BFB-AA95-31B3115F6684"),
                        new LeaderboardItem(5,5,"超级玩家","46391DE8-2BBC-4BFB-AA95-31B3115FD684"),
                        new LeaderboardItem(6,6,"超级玩家","46391DE8-2B32-4BFB-AA95-31B3115F6684"),
                        new LeaderboardItem(7,7,"超级玩家","46391DE8-2BBC-4BFB-AA95-31B3115F6684"),
                        new LeaderboardItem(8,8,"超级玩家","46391DE8-2BBC-4BAB-AAD5-31B3115F6684"),
                        new LeaderboardItem(9,9,"超级玩家","46391DE8-2BBC-4BFB-AAS5-31B3115G6684"),
                        new LeaderboardItem(10,10,"超级玩家","46391DE8-2BBC-4BFB-AA95-31B3115FD684"),
                    }
            };

        public static DefenseData DefenseData0
        {
            get
            {
                return new DefenseData
                    {
                        Nickname = "靓仔"+UnityEngine.Random.Range(10,100),
                        Level = 3,
                        Character = 90001,
                        WearEquipList = new List<int>(),
                        RoundInitHealth = 1200,
                        EnergyCapacity = 1100,
                        VegetableList = new List<VegetableUsed> { new VegetableUsed(0, 1), new VegetableUsed(1, 2) },
                        SkillEventList = new List<UseSkillEvent>
                            {
                                new UseSkillEvent(8000, 202, 1, 180),
                                new UseSkillEvent(11000, 207, 1, 220),
                                new UseSkillEvent(9000, 208, 1, 195),
                                new UseSkillEvent(9500, 202, 2, 180),
                                new UseSkillEvent(11000, 207, 1, 220),
                                new UseSkillEvent(9000, 208, 1, 195),
                                new UseSkillEvent(7000, 202, 2, 180),
                                new UseSkillEvent(4000, 207, 1, 220),
                                new UseSkillEvent(3000, 208, 1, 195),
                            }
                    };
            }
        }
        public static DefenseData DefenseData1
        {
            get
            {
                return new DefenseData
                {
                    Nickname = "玩家二货",
                    Level = 4,
                    Character = 90002,
                    WearEquipList = new List<int>(),
                    RoundInitHealth = 1300,
                    EnergyCapacity = 1300,
                    VegetableList = new List<VegetableUsed>{new VegetableUsed(0,1),new VegetableUsed(1,2)},
                    SkillEventList = new List<UseSkillEvent>
                            {
                                new UseSkillEvent(8000, 202, 1, 180),
                                new UseSkillEvent(11000, 204, 1, 220),
                                new UseSkillEvent(7000, 205, 1, 195),
                                new UseSkillEvent(6500, 206, 2, 180),
                                new UseSkillEvent(15000, 207, 1, 220),
                                new UseSkillEvent(9000, 103, 1, 195),
                                new UseSkillEvent(7000, 202, 2, 180),
                                new UseSkillEvent(14000, 207, 1, 220),
                                new UseSkillEvent(13000, 208, 1, 195),
                            }
                };
            }
        }
        public static DefenseData DefenseDataEmpty
        {
            get
            {
                return new DefenseData
                {
                    Nickname = "玩家二货",
                    Level = 4,
                    Character = 90002,
                    WearEquipList = new List<int>(),
                    RoundInitHealth = 130000,
                    EnergyCapacity = 1300,
                    VegetableList = new List<VegetableUsed> { new VegetableUsed(0, 1), new VegetableUsed(1, 2) },
                    SkillEventList = new List<UseSkillEvent>()
                };
            }
        }
        public static TeamAdd TeamAdd0
        {
            get
            {
                return new TeamAdd
                {
                    Nickname = "玩家二货",
                    Character = 90001,
                    RoundInitHealth = 1300,
                    AttackAdd = 25,
                };
            }
        }
        public static TeamAdd TeamAdd1
        {
            get
            {
                return new TeamAdd
                {
                    Nickname = "玩家三货",
                    Character = 90002,
                    RoundInitHealth = 1340,
                    AttackAdd = 75,
                };
            }
        }

        public static Communication.UpperPart.UploadChallengeOk UploadChallengeOk0
        {
            get
            {
                return new Communication.UpperPart.UploadChallengeOk
                    {
                        RoundRewardList = new List<Currency> {new Currency(10, 455), new Currency(100, 300)},
                        UnlockElement =
                            new UnlockElement
                                {
                                    LevelUp = new LevelUp(1, 2, 20, 40),
                                    EnergyCapacityUp = new EnergyCapacityUp(1, 2, 500, 800),
                                    EquipAppear = new EquipAppear {EquipCodeList = new List<int> {10001}},
                                    SkillUnlockList = new List<SkillUnlock>{new SkillUnlock(203)},
                                    VegetableUnlockList = new List<VegetableUnlock>{new VegetableUnlock(4, 6)},
                                    MajorLevelUnlockList = new List<MajorLevelUnlockInfo>{new MajorLevelUnlockInfo(2, true)}
                                },
                    };
            }
        }
    }
}
