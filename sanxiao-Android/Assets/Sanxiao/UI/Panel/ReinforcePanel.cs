using System.Collections.Generic;
using System.Globalization;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel.Reinforce;
using Fairwood.Math;
using UnityEngine;
using BindOAuthInfoOk = Assets.Sanxiao.Communication.UpperPart.BindOAuthInfoOk;
using RequestStartChallenge = Assets.Sanxiao.Communication.UpperPart.RequestStartChallenge;

namespace Assets.Sanxiao.UI.Panel
{
    public class ReinforcePanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static ReinforcePanel _instance;

        public static ReinforcePanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 ReinforcePanel instance now!");
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
                var go = Resources.Load("UI/ReinforcePanel") as GameObject;
                return go;
            }
        }

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

        private RandomTeamMember _reinforce1;
        private SNSFriendInfo _reinforce2;

        public Reinforce1Slot FriendSlot1;
        public FriendSlot FriendSlot2;
        public GameObject Reinforce1SlotTemplate, FriendSlotTemplate;
        public UIDraggablePanel DraggablePanel1, DraggablePanel2;
        public UIGrid Grid1, Grid2;
        private readonly List<Reinforce1Slot> _friendSlotList1 = new List<Reinforce1Slot>();
        private readonly List<FriendSlot> _friendSlotList2 = new List<FriendSlot>();

        protected override void Initialize()
        {
            Refresh();
        }
        public void Refresh()
        {
            _reinforce1 = GameData.Reinforce1;
            _reinforce2 = GameData.Reinforce2;

            #region 1

            RefreshList1();

            FriendSlot1.SetAndRefresh(_reinforce1, OnSelectFriend1, null);

            #endregion

            #region 2

            RefreshList2();

            FriendSlot2.SetAndRefresh(_reinforce2, OnSelectFriend2, null);
            #endregion

        }
        void RefreshList1()
        {
            foreach (var friendSlot in _friendSlotList1)
            {
                Destroy(friendSlot.gameObject);
            }
            _friendSlotList1.Clear();
            if (GameData.RandomTeamMemberList != null)
            {
                for (int i = 0; i < GameData.RandomTeamMemberList.Count; i++)
                {
                    var friendInfo = GameData.RandomTeamMemberList[i];
                    var go = PrefabHelper.InstantiateAndReset(Reinforce1SlotTemplate, Grid1.transform);
                    go.name = i.ToString(CultureInfo.InvariantCulture);
                    go.SetActive(true);
                    var cs = go.GetComponent<Reinforce1Slot>();
                    cs.SetAndRefresh(friendInfo, OnSelectFriend1, null);
                    cs.ToggleSelected(false);
                    _friendSlotList1.Add(cs);
                    go.GetComponent<UIDragPanelContents>().draggablePanel = DraggablePanel1;
                }
            }
            Grid1.repositionNow = true;
            Reinforce1SlotTemplate.SetActive(false);
        }
        void RefreshList2()
        {
            foreach (var friendSlot in _friendSlotList2)
            {
                Destroy(friendSlot.gameObject);
            }
            _friendSlotList2.Clear();
            if (CommonData.FriendData.FriendList != null)
            {
                for (int i = 0; i < CommonData.FriendData.FriendList.Count; i++)
                {
                    var friendInfo = CommonData.FriendData.FriendList[i];
                    var go = PrefabHelper.InstantiateAndReset(FriendSlotTemplate, Grid2.transform);
                    go.name = i.ToString(CultureInfo.InvariantCulture);
                    go.SetActive(true);
                    var cs = go.GetComponent<FriendSlot>();
                    cs.SetAndRefresh(friendInfo, OnSelectFriend2, null);
                    cs.ToggleSelected(false);
                    _friendSlotList2.Add(cs);
                    go.GetComponent<UIDragPanelContents>().draggablePanel = DraggablePanel2;
                }
            }
            Grid2.repositionNow = true;
            FriendSlotTemplate.SetActive(false);
        }

        public void OnSelectFriend1(Reinforce1Slot reinforce1Slot)
        {
            var friendInfo = reinforce1Slot.FriendInfo;
            if (friendInfo == null || _reinforce1 == friendInfo) //去选
            {
                _reinforce1 = null;
            }
            else//选中
            {
                _reinforce1 = friendInfo;
            }
            FriendSlot1.SetAndRefresh(_reinforce1, OnSelectFriend1, reinforce1Slot.TxrHeadIcon.mainTexture);
            foreach (var friendSlot in _friendSlotList1)
            {
                friendSlot.ToggleSelected(friendSlot.FriendInfo != null && _reinforce1 != null && friendSlot.FriendInfo.UserId == _reinforce1.UserId);
            }
        }
        public void OnSelectFriend2(FriendSlot reinforce2Slot)
        {
            var friendInfo = reinforce2Slot.FriendInfo;
            if (friendInfo == null || _reinforce2 == friendInfo) //去选
            {
                _reinforce2 = null;
            }
            else//选中
            {
                _reinforce2 = friendInfo;
            }
            FriendSlot2.SetAndRefresh(_reinforce2, OnSelectFriend2, reinforce2Slot.TxrHeadIcon.mainTexture);
            foreach (var friendSlot in _friendSlotList2)
            {
                friendSlot.ToggleSelected(friendSlot.FriendInfo != null && _reinforce2 != null && friendSlot.FriendInfo.UserId == _reinforce2.UserId);
            }
        }

        public void OnInviteFriendClick()
        {
            //TODO:调用友盟邀请，记得放一个宣传图
            //UMengPlugin.UMengShare("爆消农场");
        }

        public void ConfirmReinforce()
        {
            UMengPlugin.UMengEvent(EventId.ADD_TEAM,
                                   new Dictionary<string, object>
                                       {
                                           {"rand", _reinforce1!= null},
                                           {"frie", _reinforce2 != null}
                                       });

            GameData.Reinforce1 = _reinforce1; //应用选择的玩家
            GameData.Reinforce1Portrait = _reinforce1 == null ? null : FriendSlot1.TxrHeadIcon.mainTexture;//TODO:陌生人的头像怎么办
            GameData.Reinforce2 = _reinforce2;
            GameData.Reinforce2Portrait = _reinforce2 == null ? null : FriendSlot2.TxrHeadIcon.mainTexture;
            if (MatchUI.Instance) MatchUI.Instance.DidAddFriend();
            base.OnConfirmClick();

            var friendIdList = new List<string>();
            if (GameData.Reinforce1 != null) friendIdList.Add(GameData.Reinforce1.UserId);
            if (GameData.Reinforce2 != null) friendIdList.Add(GameData.Reinforce2.UserId);
            Requester.Instance.Send(new RequestStartChallenge(GameData.LastChallengeID)
            {
                FriendUserIdList = friendIdList
            });
        }

        public void Execute(Communication.UpperPart.RandomTeamMemberList randomTeamMemberList)
        {
            RefreshList1();
        }

        public void Execute(BindOAuthInfoOk cmd)
        {
            RefreshList2();
        }

        public override bool OnEscapeClick()
        {
            ConfirmReinforce();
            return true;
        }
    }
}