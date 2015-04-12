using System.Collections;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Match;
using Assets.Sanxiao.UI.Panel;
using Fairwood.Math;
using UnityEngine;
using MatchOk = Assets.Sanxiao.Communication.UpperPart.MatchOk;
using RequestStartChallenge = Assets.Sanxiao.Communication.UpperPart.RequestStartChallenge;
using User = Assets.Sanxiao.Communication.Proto.User;

namespace Assets.Sanxiao.UI
{
    public class MatchUI : BaseUI
    {
        #region 单例UI通用

        private static MatchUI _instance;

        public static MatchUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 MatchUI instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        protected override void Release()
        {

        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/MatchUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static MatchUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<MatchUI>(prefab, MainRoot.UIParent);//创建并成为单例
            Instance.OnStage();
            return Instance;
        }

        public override void OffStage()
        {
            Instance = null;//成为孤立
            base.OffStage();
        }

        public static void ExitStage()
        {
            if (Instance)
            {
                Instance.OffStage();
            }
        }

        #endregion

        void Start()
        {
            if (GameData.LastChallengeID != null)
            {
                RefreshBeforeChallenge();
            }
        }

        public void OnBeginMatchClick()
        {

        }

        public Sprite SpriteForPushLevel, SpriteForRealFighting;
        public GameObject GrpPushLevel, GrpRealFighting;

        public GameObject GrpCanTeam, GrpCannotTeam;
        public UIWidget BtnAddFriend1, BtnAddFriend2;
        public UITexture Reinforce1Portrait, Reinforce2Portrait;

        public Character RivalSingleCharacter;
        public Character[] RivalCharacterList;
        public UILabel LblRivalNickname;
        public UILabel LblRivalLevel;
        //public UILabel LblMyRoundCount, LblMyLevel, LblMyExp, LblMyReputation, LblMyGlobalRank;
        //public UISprite SprRivalWin, SprMyWin;
        //public MorlnUIButtonScale BtnStartChallenge;
        public GameObject BtnReturn, BtnNewMatch, BtnReMatch;
        //public GameObject ArrowMyReputation, ArrowRivalReputation, ArrowMyGlobalRank, ArrowRivalGlobalRank;

        /// <summary>
        /// 对手持有技能列表容器
        /// </summary>
        public UIGrid GridDefenseSkills;

        public GameObject SkillIconTemplate;

        public UILabel LblWaitHint;


        #region 推图
        /// <summary>
        /// 挑战局，匹配后状态，之前没调用过ResetAndRefreshMyInfo()所以不用再刷新别的东西
        /// </summary>
        public void RefreshBeforeChallenge()
        {
            //0.切换用途
            GrpPushLevel.SetActive(true);
            GrpRealFighting.SetActive(false);

            //1.刷新我的信息

            //2.刷新敌人信息
            if (GameData.RivalBossData != null)
            {
                if (GameData.RivalBossData == null)
                {
                    Debug.LogError("没有对方Boss数据");
                }
                else
                {
                    LblRivalNickname.text = GameData.RivalBossData.Nickname;
                    if (GameData.FellowDataList == null || GameData.FellowDataList.Count == 0)
                    {
                        foreach (var character in RivalCharacterList)
                        {
                            character.ClearToEmpty();
                            character.gameObject.SetActive(false);
                        }
                        RivalSingleCharacter.gameObject.SetActive(true);
                        RivalSingleCharacter.CharacterCode = GameData.RivalBossData.Character;
                        RivalSingleCharacter.Refresh();
                        RivalSingleCharacter.WearEquip(GameData.RivalBossData.WearEquipList);
                    }
                    else
                    {
                        RivalSingleCharacter.ClearToEmpty();
                        RivalSingleCharacter.gameObject.SetActive(false);
                        for (int i = 0; i < RivalCharacterList.Length; i++)
                        {
                            var character = RivalCharacterList[i];
                            if (i == 0)
                            {
                                character.gameObject.SetActive(true);
                                character.CharacterCode = GameData.RivalBossData.Character;
                                character.Refresh();
                                character.WearEquip(GameData.RivalBossData.WearEquipList);
                            }
                            else
                            {
                                if (i - 1 < GameData.FellowDataList.Count)
                                {
                                    character.gameObject.SetActive(true);
                                    character.CharacterCode = GameData.FellowDataList[i - 1].Character;
                                    character.Refresh();
                                }
                                else
                                {
                                    character.ClearToEmpty();
                                }
                            }
                        }
                    }
                }

                //3.拥有技能
                var skillCodeList = new List<SkillEnum>();
                var skillLevelList = new List<int>();
                foreach (var useSkillEvent in GameData.RivalBossData.SkillEventList)
                {
                    if (!skillCodeList.Contains((SkillEnum) useSkillEvent.SkillCode))
                    {
                        skillCodeList.Add((SkillEnum) useSkillEvent.SkillCode);
                        skillLevelList.Add(useSkillEvent.SkillLevel);
                    }
                }
                RefreshSkillGrid(skillCodeList, skillLevelList);
            }

            if (GameData.LastSubLevelData.CanTeam)
            {
                GrpCanTeam.SetActive(true);
                GrpCannotTeam.SetActive(false);

                RefreshReinforce();//刷新增援信息
            }
            else
            {
                GrpCannotTeam.SetActive(true);
                GrpCanTeam.SetActive(false);
            }
        }

        public void OnAddFriend1Click()
        {
            ReinforcePanel.Load();
            Requester.Instance.Send(new RequestRandomTeamMemberList());
        }
        public void OnAddFriend2Click()
        {
            ReinforcePanel.Load();
            Requester.Instance.Send(new RequestRandomTeamMemberList());
        }

        public void DidAddFriend()
        {
            RefreshReinforce();
        }

        void RefreshReinforce()
        {
            if (GameData.Reinforce1 == null)
            {
                BtnAddFriend1.enabled = true;
                Reinforce1Portrait.enabled = false;
            }
            else
            {
                BtnAddFriend1.enabled = false;
                Reinforce1Portrait.enabled = GameData.Reinforce1Portrait != null;
                Reinforce1Portrait.mainTexture = GameData.Reinforce1Portrait;
            }
            if (GameData.Reinforce2 == null)
            {
                BtnAddFriend2.enabled = true;
                Reinforce2Portrait.enabled = false;
            }
            else
            {
                BtnAddFriend2.enabled = false;
                Reinforce2Portrait.enabled = GameData.Reinforce2Portrait != null;
                Reinforce2Portrait.mainTexture = GameData.Reinforce2Portrait;
            }
        }

        public void OnStartChallengeClick()
        {
            if (GameData.LastSubLevelData.CanTeam)
            {
                ReinforcePanel.Load();
                Requester.Instance.Send(new RequestRandomTeamMemberList());
            }
            else
            {
                Requester.Instance.Send(new RequestStartChallenge(GameData.LastChallengeID));
            }
        }

        public void OnReturnClick()//返回推图界面
        {
            StartCoroutine(_ReturnToPushLevelUI());
        }
        IEnumerator _ReturnToPushLevelUI()
        {
            MainRoot.Goto(MainRoot.UIStateName.PushLevel);
            while (!PushLevelUI.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            var majorLevelUnlockInfo =
                CommonData.ChallengeUnlockInfoList.Find(x => x.MajorLevelId == GameData.LastChallengeMajorLevelID);
            if (majorLevelUnlockInfo != null) PushLevelUI.Instance.EnterMajorLevel(majorLevelUnlockInfo);
        }
        #endregion

        #region 实时对战

        public GameObject GrpBeforeMatch;
        public UILabel LblMatching;
        public UILabel LblHealth, LblEnergyCapacity;
        public UILabel LblRivalReputation, LblRivalGlobalRank;
        public UILabel LblButton;
        public MorlnUIButtonScale BtnCancelMatch;

        /// <summary>
        /// 匹配前
        /// </summary>
        public void RefreshBeforeMatch()
        {
            LblMatching.text = string.Format("正在匹配对手\n今日还剩{0}次挑战次数", Random.Range(1, 21));

            GrpRealFighting.SetActive(true);
            GrpBeforeMatch.SetActive(true);
            GrpPushLevel.SetActive(false);

            //2.刷新对手信息
            LblRivalNickname.text = null;
            LblRivalLevel.text = null;
            LblHealth.text = null;
            LblEnergyCapacity.text = null;
            LblRivalReputation.text = null;
            LblRivalGlobalRank.text = null;
            RivalSingleCharacter.ClearToEmpty();
            //ArrowRivalReputation.SetActive(false);
            //ArrowRivalGlobalRank.SetActive(false);

            //3.WaitHint
            var waitHintTextConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.WaitHintTextConfig) as WaitHintTextConfig;
            if (waitHintTextConfig != null && waitHintTextConfig.HintList != null && waitHintTextConfig.HintList.Count > 0)
            {
                LblWaitHint.text = waitHintTextConfig.HintList[Random.Range(0, waitHintTextConfig.HintList.Count)];
            }
            else
            {
                LblWaitHint.text = null;
            }

            //4.按钮
            BtnReturn.SetActive(false);
            BtnNewMatch.gameObject.SetActive(false);
            BtnReMatch.gameObject.SetActive(false);

            LblButton.text = "取消匹配";
            BtnCancelMatch.isEnabled = true;

            //5.技能栏
            SkillIconTemplate.SetActive(false);
        }
        public void OnCancelMatchClick()
        {
            Requester.Instance.Send(new CancelMatch());
            MainRoot.Goto(MainRoot.UIStateName.Menu);
            UMengPlugin.UMengEvent(EventId.CANCEL_MATCH,null);
        }

        /// <summary>
        /// 开局前刷新对手信息，没有箭头，适用于对战或挑战的匹配前，匹配后
        /// </summary>
        public void RefreshRivalInfoBeforeRound(float countDown)
        {
            if (CommonData.RivalUser == null)
            {
                Debug.LogError("此时不能没有RivalUser");
                return;
            }

            GrpRealFighting.SetActive(true);
            GrpPushLevel.SetActive(false);
            GrpBeforeMatch.SetActive(false);

            LblRivalNickname.text = CommonData.RivalUser.Nickname;
            LblRivalLevel.text = CommonData.RivalUser.Level.ToString(CultureInfo.InvariantCulture);
            LblHealth.text = CommonData.RivalUser.RoundInitHealth.ToString(CultureInfo.InvariantCulture);
            LblEnergyCapacity.text = CommonData.RivalUser.EnergyCapacity.ToString(CultureInfo.InvariantCulture);
            LblRivalReputation.text = "积分：";//TODO:User没有这个信息
            LblRivalGlobalRank.text = "全球排名：";

            foreach (var character in RivalCharacterList)
            {
                character.ClearToEmpty();
                character.gameObject.SetActive(false);
            }
            RivalSingleCharacter.gameObject.SetActive(true);
            RivalSingleCharacter.CharacterCode = CommonData.RivalUser.CharacterCode;
            RivalSingleCharacter.Refresh();

            BtnCancelMatch.isEnabled = false;

            StartCoroutine(CountDown(countDown));
        }
        IEnumerator CountDown(float countDown)
        {
            var i = Mathf.RoundToInt(countDown);
            while (true)
            {
                if (i <= 0) break;
                LblButton.text = "倒计时  " + i;
                yield return new WaitForSeconds(1);
                i--;
            }
        }

        #endregion

        /// <summary>
        /// Grid下不应该有除Template之外的子物体，所以该方法是一次性的
        /// </summary>
        /// <param name="skillCodes"></param>
        void RefreshSkillGrid(IList<SkillEnum> skillCodes, IList<int> skillLevels)
        {
            //Grid下不应该有除Template之外的子物体
            for (var i = 0; i < skillCodes.Count; i++)
            {
                var skillCode = skillCodes[i];
                var slot = PrefabHelper.InstantiateAndReset<SkillSlot>(SkillIconTemplate, GridDefenseSkills.transform);
                slot.name = i.ToString(CultureInfo.InvariantCulture);
                slot.gameObject.SetActive(true);
                slot.SetAndRefresh(skillCode, skillLevels[i]);
            }
            GridDefenseSkills.repositionNow = true;
            SkillIconTemplate.SetActive(false);
        }
        
        //public void Execute(MatchOk cmd)
        //{
        //    DidFindRival(cmd.RivalInfo, cmd.StartSeconds);
        //}

        /// <summary>
        /// 刷新成结算时状态
        /// </summary>
        public void RefreshEndingRoundState()
        {
            if (CommonData.MyUser == null)
            {
                Debug.LogError("MyUser == null");
                return;
            }
            if (CommonData.RivalUser == null)
            {
                Debug.LogError("RivalUser == null");
                return;
            }
            if (CommonData.MyUserOld == null)
            {
                Debug.LogError("MyUserOld == null");
                return;
            }
            if (CommonData.RivalUserOld == null)
            {
                Debug.LogError("RivalUserOld == null");
                return;
            }

            //1，刷新我的信息
            //LblMyNickname.text = CommonData.MyUser.Nickname;
            //LblMyRoundCount.text = string.Format("{0}", CommonData.MyUser.RoundCount);
            //LblMyLevel.text = string.Format("{0}", CommonData.MyUser.Level);
            //LblMyExp.text = string.Format("{0}/{1}", CommonData.MyUser.Exp, CommonData.MyUser.ExpCeil);
            //MyCharacter.CharacterCode = 0;// CommonData.MyUser.CharacterCode;
            //MyCharacter.Refresh();


            //2.刷新对手信息
            LblRivalNickname.text = CommonData.RivalUser.Nickname;
            //LblRivalRoundCount.text = string.Format("{0}", CommonData.RivalUser.RoundCount);
            LblRivalLevel.text = string.Format("{0}", CommonData.RivalUser.Level);
            RivalSingleCharacter.CharacterCode = 0;// rivalUser.CharacterCode;
            RivalSingleCharacter.Refresh();

            //3.胜利失败牌子
            //SprMyWin.enabled = true;
            //SprMyWin.spriteName = CommonData.IsLastRoundWin ? "label-Win" : "label-Lose";
            //SprRivalWin.enabled = true;
            //SprRivalWin.spriteName = !CommonData.IsLastRoundWin ? "label-Win" : "label-Lose";

            //4.按钮
            BtnReturn.SetActive(true);
            BtnNewMatch.gameObject.SetActive(true);
            BtnReMatch.gameObject.SetActive(true);
        }



        void OnNewMatchClick()//开新一局
        {
            Requester.Instance.Send(new NewMatch());//发送消息

            RefreshBeforeMatch();
        }

        void OnReMatchClick()//复仇
        {
            Requester.Instance.Send(new ReMatch());//发送消息
        }

        void OnGreetingClick()
        {
            Responder.Instance.Execute(new MatchOk { RivalInfo = new User { Nickname = "敌鸡", }, StartSeconds = 2 });
        }



        private static float GetArrowAngle(long l0, long l1)
        {
            return GetArrowAngle(l0, l1, false);
        }
        private static float GetArrowAngle(long l0, long l1, bool inverse)
        {
            if (!inverse)
            {
                if (l0 < l1) return 0;
                if (l0 > l1) return 180;
                return -90;
            }
            else
            {
                if (l0 < l1) return 180;
                if (l0 > l1) return 0;
                return -90;
            }
        }
    }
}
