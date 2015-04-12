using System;
using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Fairwood.Math;

namespace Assets.Sanxiao
{
    public static class CommonData
    {

        public static User MyUser = new User {Nickname = "我啊",// CharacterCode = 0
        };
        /// <summary>
        /// 是否进入新手教程状态。进入后立即设为false。
        /// </summary>
        public static bool FirstTimeGuide;

        public static User RivalUser;

        public static User MyUserOld;

        public static User RivalUserOld;

        public static bool IsLastRoundWin;

        private static List<UserSkill> _mySkillList;

        public static List<UserSkill> MySkillList
        {
            get { return _mySkillList ?? (_mySkillList = new List<UserSkill>()); }
            set { _mySkillList = value; }
        }

        public static List<UserVegetable> MyVegetableList;

        public class VegetableMatureInfo
        {
            public int Code;
            /// <summary>
            /// 成熟的时刻
            /// </summary>
            public float MatureTime;

            public VegetableMatureInfo(int code, float matureTime)
            {
                Code = code;
                MatureTime = matureTime;
            }
        }

        public static readonly List<VegetableMatureInfo> MyVegetableMatureInfoList = new List<VegetableMatureInfo>();

        public static List<MajorLevelUnlockInfo> ChallengeUnlockInfoList = new List<MajorLevelUnlockInfo>();

        public static List<UserCharacter> MyCharacterList;

        public static UserCharacter CurUserCharacter
        {
            get { return MyCharacterList.Find(x => x.CharacterCode == MyUser.CharacterCode); }
        }

        public static List<UserEquip> MyEquipList;

        private static Communication.Proto.Config _config;

        public static Communication.Proto.Config Config
        {
            get { return _config ?? (_config = new Communication.Proto.Config()); }
            set { _config = value; }
        }

        public static int? JustUnlockedMajorLevelId;
        /// <summary>
        /// (MajorId, SubId)
        /// </summary>
        public static IntVector2? JustUnlockedSubLevelId;

        public static List<SNSFriendUnlockInfo> SnsFriendUnlockInfoList;

        public static List<Mail> MailList;

        public static class FriendData
        {
            public static List<SNSFriendInfo> FriendList;
        }

        public static class HeartData
        {
            /// <summary>
            /// 自增长上限，Count可以超过MaxCount，超过后就不会再自动增长了
            /// </summary>
            public static int MaxCount = 6;

            //static readonly List<IRefreshable> CountListeners = new List<IRefreshable>();
            //public static void AddCountListener(IRefreshable listener)
            //{
            //    if (!CountListeners.Contains(listener)) CountListeners.Add(listener);
            //}
            //public static void RemoveCountListener(IRefreshable listener)
            //{
            //    CountListeners.RemoveAll(x => ReferenceEquals(x, listener));
            //}
            private static long _count;
            public static long Count
            {
                get { return _count; }
                set
                {
                    if (_count == value) return;
                    _count = value;
                    CountListenerList.CallRefresh();
                }
            }
            public static DataListenerList CountListenerList = new DataListenerList(new List<IRefreshable>());

            /// <summary>
            /// 领取下一个爱心的RealTime时刻
            /// </summary>
            public static float NextHeartRealTime;
        }

        public static class PkData
        {
            /// <summary>
            /// 自增长上限，Count可以超过MaxCount，超过后就不会再自动增长了
            /// </summary>
            public static int MaxCount = 5;

            private static long _count;
            public static long Count
            {
                get { return _count; }
                set
                {
                    if (_count == value) return;
                    _count = value;
                    CountListenerList.CallRefresh();
                }
            }
            public static DataListenerList CountListenerList = new DataListenerList(new List<IRefreshable>());

            /// <summary>
            /// 领取下一个爱心的RealTime时刻
            /// </summary>
            public static float NextGainRealTime;
        }

        public static class SnsData
        {
            public static class SinaWeibo
            {
                public static string Uid;
                public static string AccessToken;
                public static DateTime ExpireOn;
            }
        }
    }

    public struct DataListenerList
    {
        readonly List<IRefreshable> _listeners;

        public DataListenerList(List<IRefreshable> list)
        {
            _listeners = new List<IRefreshable>();
            _listeners = list;
        }

        public void Add(IRefreshable listener)
        {
            if (!_listeners.Contains(listener)) _listeners.Add(listener);
        }
        public void Remove(IRefreshable listener)
        {
            _listeners.RemoveAll(x => ReferenceEquals(x, listener));
        }

        public void CallRefresh()
        {
            foreach (var refreshable in _listeners)
            {
                refreshable.Refresh();
            }
        }
    }

    public interface IRefreshable
    {
        void Refresh();
    }
}
