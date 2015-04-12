using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.UI.Panel.Leaderboard;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class LeaderboardPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static LeaderboardPanel _instance;

        public static LeaderboardPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 LeaderboardPanel instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/LeaderboardPanel") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 会自动向服务端申请数据
        /// </summary>
        public static void Load()
        {
            if (Instance)
            {
                MainRoot.FocusPanel(Instance);
            }
            else
            {
                if (!Prefab) return;
                MainRoot.ShowPanel(Prefab);
            }
            if (Instance) Instance.Initialize();
        }

        public static void UnloadInterface()
        {
            if (Instance) Instance.OnConfirmClick();
        }

        #endregion

        private const int Count = 10;

        private Communication.Proto.Leaderboard _leaderboard;

        public LeaderboardSlot MySlot;

        public UIGrid Grid;

        readonly LeaderboardSlot[] _leaderboardSlots = new LeaderboardSlot[Count];

        protected override void Initialize()
        {
            Requester.Instance.Send(new RequestLeaderBoard(0));
        }

        void SetAndRefresh(Communication.Proto.Leaderboard leaderboard)
        {
            _leaderboard = leaderboard;

            for (int i = 0; i < Count; i++)
            {
                if (i < _leaderboard.ItemList.Count)
                {
                    if (_leaderboardSlots[i] == null)
                    {
                        var go = PrefabHelper.InstantiateAndReset(MySlot.gameObject, Grid.transform);
                        go.name = "Slot " + i;
                        _leaderboardSlots[i] = go.GetComponent<LeaderboardSlot>();
                    }
                    if (_leaderboardSlots[i] != null)
                    {
                        _leaderboardSlots[i].gameObject.SetActive(true);
                        _leaderboardSlots[i].SetAndRefresh(_leaderboard.ItemList[i]);
                    }
                }
                else
                {
                    if (_leaderboardSlots[i] != null) _leaderboardSlots[i].gameObject.SetActive(false);
                }
            }
            Grid.repositionNow = true;

            MySlot.SetAndRefresh(_leaderboard.MyItem);
        }

        public void Execute(Communication.Proto.Leaderboard cmd)
        {
            SetAndRefresh(cmd);
        }
    }
}