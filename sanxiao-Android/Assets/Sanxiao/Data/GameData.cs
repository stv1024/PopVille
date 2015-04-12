using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using UnityEngine;

namespace Assets.Sanxiao.Data
{
    public static class GameData
    {
        public static int LastChallengeMajorLevelID, LastChallengeSubLevelID;
        public static MajorLevelData LastMajorLevelData;
        public static SubLevelData LastSubLevelData;

        public static List<RandomTeamMember> RandomTeamMemberList;
        public static RandomTeamMember Reinforce1;
        public static Texture Reinforce1Portrait;
        public static SNSFriendInfo Reinforce2;
        public static Texture Reinforce2Portrait;

        public static string LastChallengeID;
        public static DefenseData RivalBossData;
        public static List<TeamAdd> FellowDataList;
        public static List<TeamAdd> FriendDataList;

        public static int MyHealth { get { return OurHealthList[0]; }set { OurHealthList[0] = value; } }
        public static int RivalHealth { get { return RivalHealthList[0]; } set { RivalHealthList[0] = value; } }
        public static int MyEnergy { get { return OurEnergyList[0]; } set { OurEnergyList[0] = value; } }
        public static int RivalEnergy { get { return RivalEnergyList[0]; } set { RivalEnergyList[0] = value; } }

        public static int MyEnergyCapacity;
        public static int RivalEnergyCapacity;
        public static int MyAttackAdd, RivalAttackAdd;
        public static int MyCriticalStrikeRate, RivalCriticalStrikeRate;
        public static int MyDodgeRate, RivalDodgeRate;

        /// <summary>
        /// 实时对战何时开启，RealTime，要排除设备因素导致不公平
        /// </summary>
        public static float RealTimeToStartRound;
        public static float StartRoundTime;

        /// <summary>
        /// 初始会随机出多少种蔬菜，不考虑HideGenre技能，游戏中随机糖果时不使用这个（使用CurCandyGenreCount）
        /// </summary>
        public static int InitVegetableTypeCount = 6;

        public static readonly int[] CandyToVegetable = new int[10];
        public static readonly int[] CandyEnergyList = new int[10];

        public static int[] OurHealthList = new int[TeamMaxNumber];
        public static int[] RivalHealthList = new int[TeamMaxNumber];

        public static int[] OurEnergyList = new int[TeamMaxNumber];
        public static int[] RivalEnergyList = new int[TeamMaxNumber];

        
        public static int[] OurRoundInitHealthList = new int[TeamMaxNumber];
        public static int[] RivalRoundInitHealthList = new int[TeamMaxNumber];

        public static int[] OurAttackAddList = new int[TeamMaxNumber];
        public static int[] RivalAttackAddList = new int[TeamMaxNumber];

        public static int[] OurCriticalStrikeRateList = new int[TeamMaxNumber];
        public static int[] RivalCriticalStrikeRateList = new int[TeamMaxNumber];

        public static int[] OurDodgeRateList = new int[TeamMaxNumber];
        public static int[] RivalDodgeRateList = new int[TeamMaxNumber];

        /// <summary>
        /// 这个Combo阶段已经有多少个基元消除了
        /// </summary>
        public static int CurComboAmount;

        /// <summary>
        /// 刚刚的一局赢了吗
        /// </summary>
        public static bool LastRoundWin;

        public static UploadChallengeOk LastUploadChallengeOkCmd;

        #region 常数

        public const int TeamMaxNumber = 3;

        /// <summary>
        /// 点燃到爆炸的标准延迟，用于自爆炸弹，自爆彩色糖。但自爆条纹糖往往需要带一点随机便宜量
        /// </summary>
        public const float DefaultFiredWaitingTime = 1.5f;

        #endregion
    }
}