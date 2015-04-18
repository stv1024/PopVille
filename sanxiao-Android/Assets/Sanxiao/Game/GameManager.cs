using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Sanxiao.AI;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game.Skill;
using Assets.Sanxiao.Test;
using Assets.Sanxiao.UI;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;
using EndRound = Assets.Sanxiao.Communication.UpperPart.EndRound;
using RivalUseSkill = Assets.Sanxiao.Communication.UpperPart.RivalUseSkill;
using StartRound = Assets.Sanxiao.Communication.UpperPart.StartRound;
using SyncData = Assets.Sanxiao.Communication.Proto.SyncData;
using SyncDataResponse = Assets.Sanxiao.Communication.UpperPart.SyncDataResponse;
using UploadChallenge = Assets.Sanxiao.Communication.UpperPart.UploadChallenge;
using UseSkillOk = Assets.Sanxiao.Communication.Proto.UseSkillOk;

namespace Assets.Sanxiao.Game
{
    /// <summary>
    /// 游戏控制者，生命周期从开局掉糖果到结局一方死亡
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            if (!SkillEffectInfo || !SkillEffectInfo.CheckInfoCompleteAndValid())
            {
                Debug.LogError("技能配置出错，请务必重新发布！");
            }
        }

        public enum GameStateEnum
        {
            /// <summary>
            /// 刚进入GameUI，收到StartRound前
            /// </summary>
            GetReady,
            /// <summary>
            /// 收到StartRound 到 收到EndRound
            /// </summary>
            Fighting,
            /// <summary>
            /// 收到EndRound，到离开GameUI
            /// </summary>
            Ending,
        }

        public GameStateEnum GameState;

        /// <summary>
        /// 会随机多少种Genre，考虑了HideGenre技能
        /// </summary>
        public int CurCandyGenreCount
        {
            get { return _isDurationHideGenreState ? GameData.InitVegetableTypeCount - 1 : GameData.InitVegetableTypeCount; }
        }

        public GameUI GameUI;
        public StateFlag StateFlag;
        public FightingPanel FightingPanel;
        //public EndRoundPanel EndRoundPanel;

        public UILabel LblMyEnergy;

        public Grid MyGrid;
        public PopEffectContainer PopEffectContainer;
        public CellEffectContainer CellEffectContainer;

        public enum OpponentStateEnum
        {
            RemoteRealPerson,
            LocalAI,
            RemoteChallenge,
            FreshmanGuide,
        }

        public SanxiaoAI AI;

        public OpponentStateEnum OpponentState;

        public readonly int[] OurLogic2CharacterIndex = new int[GameData.TeamMaxNumber];
        public readonly int[] RivalLogic2CharacterIndex = new int[GameData.TeamMaxNumber];
        /// <summary>
        /// index是视觉层的，用OurLogic2CharacterIndex[逻辑层index]得到
        /// </summary>
        public Character[] OurCharacterList = new Character[GameData.TeamMaxNumber];
        /// <summary>
        /// index是视觉层的，用RivalLogic2CharacterIndex[逻辑层index]得到
        /// </summary>
        public Character[] RivalCharacterList = new Character[GameData.TeamMaxNumber];

        public Character MyCharacter
        {
            get { return OurCharacterList[OurLogic2CharacterIndex[0]]; }
            //set { OurCharacterList[0] = value; }
        }

        public Transform SkillTrajectoryEffectContainer;

        public SkillEffectInfoHolder SkillEffectInfo;

        readonly Communication.Proto.Character[] _ourConfigCharacterList = new Communication.Proto.Character[GameData.TeamMaxNumber];
        readonly UserCharacter[] _ourUserCharacterList = new UserCharacter[GameData.TeamMaxNumber];

        readonly Communication.Proto.Character[] _rivalConfigCharacterList = new Communication.Proto.Character[GameData.TeamMaxNumber];
        readonly UserCharacter[] _rivalUserCharacterList = new UserCharacter[GameData.TeamMaxNumber];

        /// <summary>
        /// 如果StartRound来得早，此时客户端可能还没有调用ResetAndRefreshBeforeRealTimeFighting。防止第2次调用
        /// </summary>
        private bool _hasAlreadyResetAndRefreshBeforeRealTimeFighting;
        public void ResetAndRefreshBeforeRealTimeFighting()
        {
            if (_hasAlreadyResetAndRefreshBeforeRealTimeFighting) return;
            _hasAlreadyResetAndRefreshBeforeRealTimeFighting = true;

            ClearDefenserSetAIAndCameraAndTimeScale();

            MyGrid.ResetCells(7, 7, null); //充值网格

            OurLogic2CharacterIndex[0] = 1;
            RivalLogic2CharacterIndex[0] = 1;

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
            }

            var characterConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.CharacterConfig) as CharacterConfig;
            if (characterConfig == null)
            {
                Debug.LogError("怎能没有CharacterConfig");
                return;
            }
            var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
            if (equipConfig == null)
            {
                Debug.LogError("怎能没有EquipConfig");
                return;
            }

            #region 数值

            //我
            GameData.OurRoundInitHealthList[0] = CommonData.MyUser.RoundInitHealth;
            GameData.MyEnergyCapacity = CommonData.MyUser.EnergyCapacity;
            _ourUserCharacterList[0] =
                CommonData.MyCharacterList.Find(x => x.CharacterCode == CommonData.MyUser.CharacterCode);
            _ourConfigCharacterList[0] =
                characterConfig.CharacterList.Find(x => x.CharacterCode == CommonData.MyUser.CharacterCode);

            GameData.OurHealthList[0] = CommonData.MyUser.RoundInitHealth;

            GameData.OurEnergyList[0] = _ourConfigCharacterList[0].InitEnergy;

            GameData.OurAttackAddList[0] = 0;
            GameData.OurCriticalStrikeRateList[0] = 0;
            GameData.OurDodgeRateList[0] = 0;
            foreach (var equip in _ourUserCharacterList[0].WearEquipList.Select(equipCode => equipConfig.EquipList.Find(x => x.EquipCode == equipCode))
                                                                  .Where(equip => equip != null))
            {
                GameData.OurHealthList[0] += (int) equip.HealthAdd;
                GameData.OurAttackAddList[0] += (int) equip.AttackAdd;
                GameData.OurCriticalStrikeRateList[0] += equip.CriticalStrikeRate;
                GameData.OurDodgeRateList[0] += equip.DodgeRate;
            }

            OurCharacterList[OurLogic2CharacterIndex[0]].CharacterCode = _ourConfigCharacterList[0].CharacterCode;

            //对手
            GameData.RivalRoundInitHealthList[0] = CommonData.RivalUser.RoundInitHealth;
            GameData.RivalEnergyList[0] = 0;
            _rivalUserCharacterList[0] = new UserCharacter(CommonData.RivalUser.CharacterCode);
            _rivalConfigCharacterList[0] =
                characterConfig.CharacterList.Find(
                    x => x.CharacterCode == CommonData.RivalUser.CharacterCode);
            if (_rivalConfigCharacterList[0] == null) //出错了也能运行
            {
                Debug.LogError("找不到CharacterConfig:code:" + CommonData.RivalUser.CharacterCode);
                _rivalConfigCharacterList[0] =
                    new Communication.Proto.Character(CommonData.RivalUser.CharacterCode, 999);
            }

            GameData.RivalEnergyList[0] = _rivalConfigCharacterList[0].InitEnergy;

            GameData.RivalHealthList[0] = CommonData.RivalUser.RoundInitHealth;
            GameData.RivalAttackAddList[0] = 0;
            GameData.RivalCriticalStrikeRateList[0] = 0;
            GameData.RivalDodgeRateList[0] = 0;
            foreach (var equip in _rivalUserCharacterList[0].WearEquipList.Select(equipCode => equipConfig.EquipList.Find(x => x.EquipCode == equipCode))
                                                                    .Where(equip => equip != null))
            {
                GameData.RivalHealthList[0] += (int) equip.HealthAdd;
                GameData.RivalAttackAddList[0] += (int) equip.AttackAdd;
                GameData.RivalCriticalStrikeRateList[0] += equip.CriticalStrikeRate;
                GameData.RivalDodgeRateList[0] += equip.DodgeRate;
            }

            RivalCharacterList[RivalLogic2CharacterIndex[0]].CharacterCode =
                _rivalConfigCharacterList[0].CharacterCode;

            #endregion

            ResetVegetableData();
            
            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                if (_ourUserCharacterList[i] == null)
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].Refresh(0.5f);
                }
            }
            for (int i = 0; i < RivalCharacterList.Length; i++)
            {
                if (_rivalUserCharacterList[i] == null)
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].Refresh(0.5f);
                }
            }

            GameUI.LblTimer.text = "准备";

            StateFlag.ShowReady();

            GameUI.BtnPause.SetActive(false);
        }

        /// <summary>
        /// 由MainController控制开始实时对战局
        /// </summary>
        public void StartRealTimeRound()
        {
            ResetAndRefreshBeforeRealTimeFighting();//防止重复调用
            GameState = GameStateEnum.Fighting;
            OpponentState = OpponentStateEnum.RemoteRealPerson;
            MyGrid.StartRound();//激活网格，可以开始掉糖果

            //TODO:模拟对手
            //MyOpponentAI = PrefabHelper.InstantiateAndReset<OpponentAI>(OpponentAIPrefab, transform);
            //if (MainController.Instance && MainController.Instance.ConnectServer)
            //{
            //    if (CurLocalServer) Destroy(CurLocalServer.gameObject);
            //}
            //else
            //{
            //    if (CurLocalServer) CurLocalServer.StartPlaying(1000);
            //}

            GameData.StartRoundTime = Time.time;

            StateFlag.ShowGo();
        }

        public void ResetAndRefreshBeforeChallengeFighting(int countDown)
        {
            ClearDefenserSetAIAndCameraAndTimeScale();

            #region 初始化网格，根据正确的SubLevelData

            if (GameData.LastSubLevelData == null)
            {
                MyGrid.ResetCells(7, 7, null);
            }
            else
            {
                MyGrid.ResetCells(GameData.LastSubLevelData.Height, GameData.LastSubLevelData.Width,
                                  GameData.LastSubLevelData.GridConfigList);
            }

            #endregion

            #region 初始化玩家、角色数据

            if (GameData.FriendDataList == null || GameData.FriendDataList.Count == 0)
            {
                OurLogic2CharacterIndex[0] = 1;
            }
            else
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    OurLogic2CharacterIndex[i] = i;
                }
            }
            if (GameData.FellowDataList == null || GameData.FellowDataList.Count == 0)
            {
                RivalLogic2CharacterIndex[0] = 1;
            }
            else
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    RivalLogic2CharacterIndex[i] = i;
                }
            }

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
            }

            var characterConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.CharacterConfig) as CharacterConfig;
            if (characterConfig == null)
            {
                Debug.LogError("怎能没有CharacterConfig");
                return;
            }


            #endregion

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                if (_ourUserCharacterList[i] == null)
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].Refresh(i == 0 ? 0.7f : 0.5f);
                }
            }

            for (int i = 0; i < RivalCharacterList.Length; i++)
            {
                if (_rivalUserCharacterList[i] == null)
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].Refresh(i == 0 ? 0.7f : 0.5f);
                }
            }

            GameUI.LblTimer.text = "准备";
            StateFlag.ShowReady();

            GameUI.BtnPause.SetActive(true);
        }

        /// <summary>
        /// 由MainController控制开始异步挑战局
        /// </summary>
        public void StartChallengeRound()//和StartRound类似功能
        {
            GameState = GameStateEnum.Fighting;
            OpponentState = OpponentStateEnum.RemoteChallenge;
            MyGrid.StartRound();


            GameData.StartRoundTime = Time.time;

            StateFlag.ShowGo();
        }

        /// <summary>
        /// 新手教学用到的
        /// </summary>
        public void ResetAndRefreshAndStartFreshmanGuideRound()//和StartRound类似功能
        {
            ClearDefenserSetAIAndCameraAndTimeScale();

            #region 初始化玩家、角色数据

            if (GameData.FriendDataList == null || GameData.FriendDataList.Count == 0)
            {
                OurLogic2CharacterIndex[0] = 1;
            }
            else
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    OurLogic2CharacterIndex[i] = i;
                }
            }
            if (GameData.FellowDataList == null || GameData.FellowDataList.Count == 0)
            {
                RivalLogic2CharacterIndex[0] = 1;
            }
            else
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    RivalLogic2CharacterIndex[i] = i;
                }
            }

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
                _ourUserCharacterList[i] = null;
                _ourConfigCharacterList[i] = null;
            }

            var characterConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.CharacterConfig) as CharacterConfig;
            if (characterConfig == null)
            {
                Debug.LogError("怎能没有CharacterConfig");
                return;
            }
            var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
            if (equipConfig == null)
            {
                Debug.LogError("怎能没有EquipConfig");
                return;
            }

            #region 数值

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                var hasOne = true;
                if (i == 0)
                {
                    GameData.OurRoundInitHealthList[i] = 100;
                    GameData.MyEnergyCapacity = 120;
                    _ourUserCharacterList[i] = new UserCharacter(90001);
                    _ourConfigCharacterList[i] =
                        characterConfig.CharacterList.Find(x => x.CharacterCode == 90001);

                    GameData.OurHealthList[i] = 100;
                }
                else if (GameData.FriendDataList != null && i - 1 < GameData.FriendDataList.Count)
                {
                    GameData.OurRoundInitHealthList[i] = GameData.FriendDataList[i - 1].RoundInitHealth;
                    _ourUserCharacterList[i] = new UserCharacter(GameData.FriendDataList[i - 1].Character);
                    _ourConfigCharacterList[i] =
                        characterConfig.CharacterList.Find(
                            x => x.CharacterCode == GameData.FriendDataList[i - 1].Character);
                    GameData.OurHealthList[i] = GameData.FriendDataList[i - 1].RoundInitHealth;
                }
                else
                {
                    hasOne = false;
                }
                if (hasOne)
                {
                    if (_ourConfigCharacterList[i] == null) //出错了也能运行
                    {
                        if (GameData.FriendDataList != null)
                        {
                            Debug.LogError("找不到CharacterConfig:code:" + GameData.FriendDataList[i - 1].Character);
                        }
                        else
                        {
                            Debug.LogError("找不到CharacterConfig，同时GameData.FriendDataList还 == null");
                        }
                        _ourConfigCharacterList[i] =
                            new Communication.Proto.Character(_ourUserCharacterList[i].CharacterCode, 999);
                    }

                    GameData.OurEnergyList[i] = 0;//必须从0开始，新手教程才能计算

                    GameData.OurAttackAddList[i] = 0;
                    GameData.OurCriticalStrikeRateList[i] = 0;
                    GameData.OurDodgeRateList[i] = 0;
                    
                    //不算装备

                    OurCharacterList[OurLogic2CharacterIndex[i]].CharacterCode =
                        _ourConfigCharacterList[i].CharacterCode;
                }
            }
            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                var hasOne = true;
                if (i == 0)
                {
                    GameData.RivalRoundInitHealthList[i] = GameData.RivalBossData.RoundInitHealth;
                    _rivalUserCharacterList[i] = new UserCharacter(GameData.RivalBossData.Character);
                    GameData.RivalHealthList[i] = GameData.RivalBossData.RoundInitHealth;
                    _rivalConfigCharacterList[i] =
                        characterConfig.CharacterList.Find(
                            x => x.CharacterCode == GameData.RivalBossData.Character);
                }
                else if (GameData.FellowDataList != null && i - 1 < GameData.FellowDataList.Count)
                {
                    GameData.RivalRoundInitHealthList[i] = GameData.FellowDataList[i - 1].RoundInitHealth;
                    _rivalUserCharacterList[i] = new UserCharacter(GameData.FellowDataList[i - 1].Character);
                    GameData.RivalHealthList[i] = GameData.FellowDataList[i - 1].RoundInitHealth;
                    _rivalConfigCharacterList[i] =
                        characterConfig.CharacterList.Find(
                            x => x.CharacterCode == GameData.FellowDataList[i - 1].Character);
                }
                else
                {
                    hasOne = false;
                }
                if (hasOne)
                {
                    if (_rivalConfigCharacterList[i] == null) //出错了也能运行
                    {
                        //Debug.LogError("找不到CharacterConfig:code:" + _rivalUserCharacterList[i].CharacterCode);
                        _rivalConfigCharacterList[i] =
                            new Communication.Proto.Character(_rivalUserCharacterList[i].CharacterCode, 999);
                    }

                    GameData.RivalAttackAddList[i] = 0;
                    GameData.RivalCriticalStrikeRateList[i] = 0;
                    GameData.RivalDodgeRateList[i] = 0;
                    //不算装备

                    RivalCharacterList[RivalLogic2CharacterIndex[i]].CharacterCode =
                        _rivalConfigCharacterList[i].CharacterCode;
                }
            }

            #endregion

            GameData.InitVegetableTypeCount = 5;

            for (int i = 0; i < GameData.InitVegetableTypeCount; i++)
            {
                //GameData.CandyToVegetable[i] = CommonData.MyVegetableList[i].VegetableCode; //TODO:以后根据关卡设计文件来加载

                GameData.CandyEnergyList[i] = 10;
            }

            #endregion

            for (int i = 0; i < GameData.TeamMaxNumber; i++)
            {
                if (_ourUserCharacterList[i] == null)
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    OurCharacterList[OurLogic2CharacterIndex[i]].Refresh(i == 0 ? 0.7f : 0.5f);
                }
            }
            for (int i = 0; i < RivalCharacterList.Length; i++)
            {
                if (_rivalUserCharacterList[i] == null)
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].ClearToEmpty();
                }
                else
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[i]].Refresh(i == 0 ? 0.7f : 0.5f);
                }
            }

            GameUI.LblTimer.text = "准备";
            //StateFlag.ShowReady();


            GameState = GameStateEnum.Fighting;
            OpponentState = OpponentStateEnum.FreshmanGuide;
            MyGrid.StartRound();

            //方案1：3个角色一起攻击
            var defenser = DefenserHolder.AddComponent<Defenser>();//只有Boss有DefenseData
            DefenserList.Add(defenser);
            defenser.StartPlaying(this, true, 0, GameData.RivalBossData);

            GameData.StartRoundTime = Time.time;

            //StateFlag.ShowGo();

            GameUI.BtnPause.SetActive(false);
        }

        public void ClearDefenserSetAIAndCameraAndTimeScale()
        {
            GameState = GameStateEnum.GetReady; //设置游戏状态，开打前

            foreach (var defenser in DefenserList) //确保清除所有Defenser
            {
                Destroy(defenser);
            }
            DefenserList.Clear();

            AI = new SanxiaoAI(MyGrid); //创建一个用于提示的AI对象

            MyGrid.ControlCamera = Camera.main; //设置控制摄像机

            Resume();//包含 Time.timeScale = 1; //设置时间尺度，确保为1
        }

        /// <summary>
        /// 刷新蔬菜数据
        /// </summary>
        void ResetVegetableData()
        {
            //蔬菜.TODO:以后根据关卡设计文件来加载
            GameData.InitVegetableTypeCount = Mathf.Min(6,
                                                        CommonData.MyVegetableList != null
                                                            ? CommonData.MyVegetableList.Count
                                                            : 6);

            var vegetableConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.VegetableConfig) as VegetableConfig;
            if (vegetableConfig == null)
            {
                Debug.LogError("怎能没有VegetableConfig，别玩了");
                MainController.RestartApp();
                return;
            }
            for (int i = 0; i < GameData.InitVegetableTypeCount; i++)
            {
                GameData.CandyToVegetable[i] = CommonData.MyVegetableList[i].VegetableCode; //TODO:以后根据关卡设计文件来加载

                var vegetable =
                    vegetableConfig.VegetableList.Find(
                        x => x.VegetableCode == GameData.CandyToVegetable[i]);
                if (vegetable != null)
                {
                    var ind = CommonData.MyVegetableList[i].CurrentLevel - 1;
                    if (ind < 0)
                    {
                        Debug.LogError("MyVegetableList[i].CurrentLevel怎么能<1");
                        ind = 0;
                    }
                    if (ind < vegetable.LevelEnergyList.Count)
                    {
                        GameData.CandyEnergyList[i] = vegetable.LevelEnergyList[ind];
                    }
                    else
                    {
                        Debug.LogError("怎么能MyVegetableList[i].CurrentLevel-1>=vegetable.LevelEnergyList.Count");
                        GameData.CandyEnergyList[i] = 1;
                    }
                }
                else
                {
                    Debug.LogError("怎么能找不到vegatable.code:" + GameData.CandyToVegetable[i]);
                }
            }
        }

        public GameObject SkillSanxiaoEffectHolder;

        private void Update()
        {
            #region 控制4槽的刷新

            if (_playingSkillList.Count <= 0)
            {
                GameUI.PgrMyHealth.fillAmount = GameData.MyHealth*1f/CommonData.MyUser.RoundInitHealth;
                GameUI.LblMyHealth.text = GameData.MyHealth.ToString(CultureInfo.InvariantCulture);
            }

            if (_playingSkillList.Count <= 0)
            {
                LblMyEnergy.text = String.Format("怒气 {0}", GameData.MyEnergy);
                GameUI.PgrMyEnergy.fillAmount = GameData.MyEnergy * 1f / GameData.MyEnergyCapacity;
            }

            if (_playingSkillList.Count <= 0)
            {
                if (OpponentState == OpponentStateEnum.RemoteChallenge)
                {
                    GameUI.PgrRivalHealth.fillAmount = GameData.RivalHealthList[0] * 1f / GameData.RivalRoundInitHealthList[0];
                    GameUI.LblRivalHealth.text = GameData.RivalHealthList[0].ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    GameUI.PgrRivalHealth.fillAmount = GameData.RivalHealth * 1f /
                                                  (CommonData.RivalUser != null
                                                       ? CommonData.RivalUser.RoundInitHealth
                                                       : 1000);
                    GameUI.LblRivalHealth.text = GameData.RivalHealth.ToString(CultureInfo.InvariantCulture);
                }
            }

            if (_playingSkillList.Count <= 0)
            {
                if (OpponentState == OpponentStateEnum.RemoteChallenge)
                {
                    GameUI.PgrRivalEnergy.fillAmount = GameData.RivalEnergyList[0] * 1f / 1000;
                }
                else
                {
                    if (CommonData.RivalUser != null)
                    {
                        GameUI.PgrRivalEnergy.fillAmount = GameData.RivalEnergy * 1f / GameData.RivalEnergyCapacity;
                    }
                }
            }

            #endregion

            if (GameState == GameStateEnum.Fighting)
            {
                if (!SomeoneIsUsingSkill)
                {
                    if (UseSkillInfoQueue.Count > 0)
                    {
                        PlayUseSkill(UseSkillInfoQueue.Dequeue());
                    }
                }

                if (GameData.MyEnergy >= GameData.MyEnergyCapacity && !_isLettingPlayerUseSkill)
                {
                    //LetPlayerUseSkill();
                }

                if (GameData.MyEnergy > GameData.MyEnergyCapacity) //不能超过上限
                {
                    GameData.MyEnergy = GameData.MyEnergyCapacity;
                }

                if (GameData.MyHealth <= 0)
                {

                }

                GameUI.LblTimer.text = Mathf.Clamp(
                    Mathf.CeilToInt(120 - Time.time + GameData.StartRoundTime), 0, 100000)
                         .ToString(CultureInfo.InvariantCulture);
            }
        }

        public SkillEnum[] ThreeSkills = new SkillEnum[3];

        public UIPlayTween CurUIPlayTween;
        public FightingPanel CurFightingPanel;

        public GameObject AvatarAnimTemplate;

        /// <summary>
        /// 现在被物理攻击的是我方的哪个人
        /// </summary>
        public int TargetOfUs
        {
            get
            {
                for (int i = GameData.TeamMaxNumber - 1; i >= 0; i--)
                {
                    if (GameData.OurHealthList[i] > 0) return i;
                }
                return 0;
            }
        }
        /// <summary>
        /// 现在被物理攻击的是敌方的哪个人
        /// </summary>
        public int TargetOfRival
        {
            get
            {
                for (int i = GameData.TeamMaxNumber - 1; i >= 0; i--)
                {
                    if (GameData.RivalHealthList[i] > 0) return i;
                }
                return 0;
            }
        }

        public readonly Queue<UseSkillInfo> UseSkillInfoQueue = new Queue<UseSkillInfo>();

        public class UseSkillInfo
        {
            public bool ByOur;
            public int Index;
            public SkillEnum SkillCode;
            public readonly int[] PhysicalDamages = new int[GameData.TeamMaxNumber];

            public UseSkillInfo(bool byOur, int index, SkillEnum skillCode, int[] physicalDamages)
            {
                ByOur = byOur;
                Index = index;
                SkillCode = skillCode;
                for (var i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    if (i < physicalDamages.Length) PhysicalDamages[i] = physicalDamages[i];
                }
            }
            public override string ToString()
            {
                return "UseSkillInfo:" + ByOur + "," + Index + "," + SkillCode + ",{" + PhysicalDamages[0] + "," +
                       PhysicalDamages[1] + "," + PhysicalDamages[2] + "}";
            }
        }

        /// <summary>
        /// 是否处于某个技能效果状态，此时不能播放其他技能效果。
        /// </summary>
        public bool SomeoneIsUsingSkill;

        /// <summary>
        /// 还没有收到服务端确认Ok的技能事件。有顺序的
        /// </summary>
        public readonly List<UseSkillInfo> _mySuspendingSkillList = new List<UseSkillInfo>();
        /// <summary>
        /// 还没有收到服务端确认Ok却已错过正常三消生效的技能事件。有顺序的
        /// </summary>
        public readonly List<UseSkillInfo> _myOverdueSkillList = new List<UseSkillInfo>();
        /// <summary>
        /// 一旦开始处理一个技能，就加入这个队列，直到处理完毕再移除。此列表不空时就应该忽略SyncData
        /// </summary>
        public readonly List<UseSkillInfo> _playingSkillList = new List<UseSkillInfo>();

        #region Skill技能

        /// <summary>
        /// 正在显示技能使用面板让用户使用技能
        /// </summary>
        private bool _isLettingPlayerUseSkill;
        void LetPlayerUseSkill()
        {
            if (_isLettingPlayerUseSkill)//如果上一次让用户使用技能的机会还没有用掉，就自动施放上一次机会的技能
            {
                AutoUseSkill();
                CancelInvoke("AutoUseSkill");
            }
            
            _isLettingPlayerUseSkill = true;//设置状态

            if (OpponentState != OpponentStateEnum.FreshmanGuide)
            {
                Invoke("AutoUseSkill", 12f); //数值，自动施放的缓冲

                //设置3个Skill
                var random3Skills = SkillUtil.Random3OfMySkill();
                for (int i = 0; i < 3; i++) //i是grade
                {
                    ThreeSkills[i] = i < random3Skills.Count ? random3Skills[i] : 0; //如果没有解锁该档次，将填入0
                }
            }
            else//新手教程状态则特殊
            {
                ThreeSkills[0] = (SkillEnum) 206;
                ThreeSkills[1] = 0;
                ThreeSkills[2] = 0;
            }
            for (var index = 0; index < GameUI.SkillButtons.Length; index++) //刷新3个技能使用按钮
            {
                if (OpponentState != OpponentStateEnum.FreshmanGuide)
                {
                    GameUI.SkillButtons[index].Refresh(ThreeSkills[index]);
                }
                else
                {
                    GameUI.SkillButtons[index].Refresh(ThreeSkills[index], 1, "锁链", 80);
                }
            }
            CurUIPlayTween.Play(true);//面板跳入
        }
        void AutoUseSkill()
        {
            UseSelectedSkill(0);
        }
        public void UseSelectedSkill(int index)
        {
            if (GameState == GameStateEnum.Fighting)
            {
                if (!_isLettingPlayerUseSkill)
                {
                    Debug.LogError("_isLettingPlayerUseSkill == false，怎么能使用技能");
                    //return;
                }
                if (index < 0 || index >= 3)
                {
                    Debug.LogError("使用技能但index错了:" + index);
                    return;
                }

                var skillCode = ThreeSkills[index]; //技能Code
                if (OpponentState != OpponentStateEnum.FreshmanGuide)
                {
                    var skillLevel = SkillUtil.GetMySkillLevel(skillCode);
                    var cmd = SkillUtil.GetUseSkillCmd(skillCode); //开始使用技能之旅
                    var physicalDamages = new int[GameData.TeamMaxNumber];
                    physicalDamages[0] = cmd.PhysicalDamage;
                    if (OpponentState == OpponentStateEnum.RemoteChallenge) //推图
                    {
                        UMengPlugin.UMengEvent(EventId.PUSH_EMIT_SKILL,
                                               new Dictionary<string, object>
                                                   {
                                                       {"major", GameData.LastChallengeMajorLevelID},
                                                       {"sub", GameData.LastChallengeSubLevelID},
                                                       {"code", cmd.SkillCode},
                                                       {"level", skillLevel}
                                                   });//发送统计事件

                        for (int i = 0; i < GameData.FriendDataList.Count; i++)
                        {
                            var teamAdd = GameData.FriendDataList[i];
                            physicalDamages[1 + i] = teamAdd.AttackAdd;
                        }
                        UseSkillInfoQueue.Enqueue(new UseSkillInfo(true, 0, skillCode, physicalDamages));
                    }
                    else //实时对战
                    {
                        UMengPlugin.UMengEvent(EventId.MULTI_EMIT_SKILL,
                                               new Dictionary<string, object>
                                                   {
                                                       {"code", cmd.SkillCode},
                                                       {"level", skillLevel}
                                                   });//发送统计事件

                        Requester.Instance.Send(cmd); //向服务器发送UseSkill命令

                        if (CurLocalServer != null) CurLocalServer.Execute(cmd);
                        var useSkillInfo = new UseSkillInfo(true, 0, skillCode, physicalDamages);
                        StartCoroutine(SomeoneUseSkill(useSkillInfo));
                    }
                    GameData.MyEnergy -= SkillUtil.GetSkillUseEnergy(skillCode); //不检测会不会不够
                }
                else//新手教程状态下
                {
                    StartCoroutine(SomeoneUseSkill(new UseSkillInfo(true, 0, skillCode, new[] { 80, 30, 0 })));
                    GameData.MyEnergy -= 80; //不检测会不会不够
                }
            }

            CancelInvoke("AutoUseSkill");//不再自动使用技能
            _isLettingPlayerUseSkill = false;//设置状态

            CurUIPlayTween.Play(false);//面板跳出
        }

        public void PlayUseSkill(UseSkillInfo useSkillInfo)
        {
            SomeoneIsUsingSkill = true;

            StartCoroutine(SomeoneUseSkill(useSkillInfo));
        }

        public const float TeamAttackInterval = 0.15f;//可能要改

        /// <summary>
        /// 某方攻击
        /// </summary>
        IEnumerator SomeoneUseSkill(UseSkillInfo useSkillInfo)
        {
            var byOur = useSkillInfo.ByOur;
            var index = useSkillInfo.Index;
            var skillCode = useSkillInfo.SkillCode;
            var physicalDamages = useSkillInfo.PhysicalDamages;

            if (OpponentState == OpponentStateEnum.RemoteRealPerson)
            {
                _playingSkillList.Add(useSkillInfo);

                if (byOur)
                {
                    _mySuspendingSkillList.Add(useSkillInfo);
                }
            }

            //var trajectoryAnimationLength = SkillEffectInfo.GetTrajectoryAnimationLength(skillCode);
            var playSanxiaoEffectTime = SkillEffectInfo.GetPlaySanxiaoEffectTime(skillCode);
            var tarIndex = byOur ? TargetOfRival : TargetOfUs;

            var totalDamage =
                new[] {0, 1, 2}.Select(
                    i => (byOur ? GameData.OurHealthList[i] : GameData.RivalHealthList[i]) > 0 ? physicalDamages[i] : 0)
                               .Sum();//计算此次攻击的物理伤害

            var ko = tarIndex == 0 &&
                     ((byOur ? GameData.RivalHealth : GameData.MyHealth) <= totalDamage);//目标是老大，且老大血量低于总伤害，则致命一击
            if (ko)
            {
                GameData.LastUploadChallengeOkCmd = null; //先清除这个数据，之后用这个数据判断是否提交成功
                StartCoroutine(EndChallengeRoundCoroutine());
            }
            else
            {
                var g = PrefabHelper.InstantiateAndReset(AvatarAnimTemplate, transform);
                var aa = g.GetComponent<AttackAvatar>();
                aa.Initialize(
                    CharacterBAPool.Dequeue(byOur
                                                ? OurCharacterList[OurLogic2CharacterIndex[0]].CharacterCode
                                                : RivalCharacterList[RivalLogic2CharacterIndex[0]].CharacterCode),
                    !byOur);
                g.SetActive(true);
                Destroy(g, 3);
                //if (!byOur)
                //{
                //    var scale = g.transform.localScale;
                //    scale.x *= -1;
                //    g.transform.localScale = scale;
                //}
            }

            //方案1
            StartCoroutine(TeamAttackCoroutine(byOur, tarIndex, physicalDamages));

            //方案2
            //var character = byOur ? OurCharacterList[OurLogic2CharacterIndex[index]] : RivalCharacterList[RivalLogic2CharacterIndex[index]];
            //if (character)
            //{
            //    if (byOur) character.Attack(OurLogic2CharacterIndex[index], RivalLogic2CharacterIndex[tarIndex]);
            //    else character.Attack(RivalLogic2CharacterIndex[index], OurLogic2CharacterIndex[tarIndex]);
            //}
            //else
            //{
            //    Debug.LogError("怎么可能没有character。index:" + index);
            //}

            if (!ko)
            {
                #region 大哥施放技能

                if (index == 0) //Boss大哥才能施放技能
                {
                    yield return new WaitForSeconds(SkillEffectInfo.PlayTrajectoryEffectTime);

                    //var totalLength = Mathf.Max(trajectoryAnimationLength, playSanxiaoEffectTime);

                    //播放弹道特效
                    var prefab = GetSkillTrajectoryEffectPrefab(skillCode);
                    if (prefab) //美术制作了效果
                    {
                        var ste = PrefabHelper.InstantiateAndReset<SkillTrajectoryEffectInfo>(prefab,
                                                                                              SkillTrajectoryEffectContainer
                                                                                                  .transform);
                        if (skillCode.IsSkillAffectSelf())
                        {
                            if (!byOur) ste.FlipDirection(); //翻转
                        }
                        else
                        {
                            if (byOur) ste.FlipDirection(); //翻转
                        }

                        var beMagicallyAttackTime = SkillEffectInfo.GetBeMagicallyAttackTime(skillCode);
                        if (beMagicallyAttackTime >= 0)
                        {
                            if (skillCode.IsSkillAffectSelf())
                            {
                                StartCoroutine(DelayToBeMagicallyAttack(byOur, index, beMagicallyAttackTime));
                            }
                            else
                            {
                                StartCoroutine(DelayToBeMagicallyAttack(!byOur, tarIndex, beMagicallyAttackTime));
                            }
                        }
                    }

                    if (playSanxiaoEffectTime > 0)
                    {
                        yield return new WaitForSeconds(playSanxiaoEffectTime);
                    }

                    var skillLevel = SkillUtil.GetMySkillLevel(skillCode);

                    if (OpponentState != OpponentStateEnum.RemoteRealPerson)
                    {
                        SanxiaoTakeEffect(byOur, skillCode, skillLevel); //三消、数值生效
                    }
                    else
                    {
                        if (byOur)
                        {
                            if (!_mySuspendingSkillList.Contains(useSkillInfo))
                                //如果不是实时对战，直接三消生效；如果实时对战，需要服务端确认Ok才能三消生效，确认的特征是从_mySuspendingSkillList里剔除
                            {
                                SanxiaoTakeEffect(true, skillCode, skillLevel); //三消、数值生效
                                _playingSkillList.Remove(useSkillInfo);
                            }
                            else //错过了生效时间
                            {
                                _mySuspendingSkillList.Remove(useSkillInfo);
                                _myOverdueSkillList.Add(useSkillInfo);
                            }
                        }
                        else
                        {
                            SanxiaoTakeEffect(false, skillCode, skillLevel); //三消、数值生效
                            _playingSkillList.Remove(useSkillInfo);
                        }
                    }

                    if (OpponentState == OpponentStateEnum.RemoteChallenge && byOur) //推图时，我施放的技能对防守数据有影响
                    {
                        var defenser = DefenserList.Find(x => x.IsRival && x.Index == 0);
                        if (skillCode == SkillEnum.ReduceEnergy)
                        {
                            var skillParameterConfig =
                                ConfigManager.GetConfig(ConfigManager.ConfigType.SkillParameterConfig) as
                                SkillParameterConfig;
                            SkillParameter curSkillConfig = null;
                            if (skillParameterConfig != null)
                            {
                                curSkillConfig =
                                    skillParameterConfig.SkillParameterList.Find(
                                        x => (SkillEnum) x.SkillCode == SkillEnum.ReduceEnergy);
                            }
                            else
                            {
                                Debug.LogError("没有skillParameterConfig");
                            }
                            var reduce = 1;
                            if (curSkillConfig != null && curSkillConfig.ConstantList.Count >= 2)
                            {
                                reduce =
                                    Mathf.FloorToInt(curSkillConfig.ConstantList[0] +
                                                     curSkillConfig.ConstantList[1]*skillLevel);
                            }
                            else
                            {
                                Debug.LogError("没有技能参数配置或常数数量不对Code:" + SkillEnum.ReduceEnergy);
                            }
                            if (defenser != null) defenser.ReduceEnergy(reduce);
                        }
                        if (!skillCode.IsSkillAffectSelf())
                        {
                            if (defenser != null) defenser.Disturb(skillLevel*0.3f + 0.4f); //延迟
                        }
                    }
                }
                else
                {
                    yield return
                        new WaitForSeconds(SkillEffectInfo.TimeFromPlayAttackToRivalBeAttacked +
                                           SkillEffectInfo.BeAttackedLength); //只有物理攻击，对方受击动画播放完后即可结束此次技能效果
                }

                #endregion

                SomeoneIsUsingSkill = false;
            }
        }

        private IEnumerator TeamAttackCoroutine(bool byOur, int tarIndex, int[] physicalDamages)
        {
            if (byOur)
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    if (OurCharacterList[OurLogic2CharacterIndex[i]] && GameData.OurHealthList[i] > 0)
                    {
                        OurCharacterList[OurLogic2CharacterIndex[i]].Attack(OurLogic2CharacterIndex[i], RivalLogic2CharacterIndex[tarIndex]);
                        if (physicalDamages[i] > 0)
                            StartCoroutine(CauseDamageCoroutine(false, tarIndex, physicalDamages[i],
                                                                SkillEffectInfo.TimeFromPlayAttackToRivalBeAttacked));
                        yield return new WaitForSeconds(TeamAttackInterval);
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameData.TeamMaxNumber; i++)
                {
                    if (RivalCharacterList[RivalLogic2CharacterIndex[i]] && GameData.RivalHealthList[i] > 0)
                    {
                        RivalCharacterList[RivalLogic2CharacterIndex[i]].Attack(RivalLogic2CharacterIndex[i], OurLogic2CharacterIndex[tarIndex]);
                        if (physicalDamages[i] > 0)
                            StartCoroutine(CauseDamageCoroutine(true, tarIndex, physicalDamages[i],
                                                                SkillEffectInfo.TimeFromPlayAttackToRivalBeAttacked));
                        yield return new WaitForSeconds(TeamAttackInterval);
                    }
                }
            }
        }

        IEnumerator CauseDamageCoroutine(bool onUs, int tarIndex, int damage, float delay)
        {
            yield return new WaitForSeconds(delay);
            CreateDamageLabel(onUs, tarIndex, damage);//显示Label
            if (onUs)
            {
                GameData.OurHealthList[tarIndex] = Mathf.Max(0, GameData.OurHealthList[tarIndex] - damage);
                if (GameData.OurHealthList[tarIndex] <= 0)
                {
                    OurCharacterList[OurLogic2CharacterIndex[tarIndex]].Die();
                    FightingPanel.CharacterDie(true, OurLogic2CharacterIndex[tarIndex]);

                    var defenserIndex = DefenserList.FindIndex(x => !x.IsRival && x.Index == tarIndex);//找到Defenser，把它弄死
                    if (defenserIndex >= 0)
                    {
                        Destroy(DefenserList[defenserIndex]);
                        DefenserList.RemoveAt(defenserIndex);
                    }
                    if (tarIndex == 0)
                    {
                        //StateFlag.ShowGameOver();
                        if (OpponentState == OpponentStateEnum.RemoteChallenge)
                        {
                            EndChallengeRound(false);
                        }
                        else if (OpponentState == OpponentStateEnum.FreshmanGuide)
                        {
                            Debug.LogError("新手教程不应该是我方失败的结局");
                            //新手教程三消部分结局
                            FreshmanGuide.GotoNext();
                        }
                    }
                }
                else
                {
                    OurCharacterList[OurLogic2CharacterIndex[tarIndex]].BeAttacked();
                }
            }
            else
            {
                GameData.RivalHealthList[tarIndex] = Mathf.Max(0, GameData.RivalHealthList[tarIndex] - damage);
                if (GameData.RivalHealthList[tarIndex] <= 0)
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[tarIndex]].Die();
                    FightingPanel.CharacterDie(false, RivalLogic2CharacterIndex[tarIndex]);

                    var defenserIndex = DefenserList.FindIndex(x => x.IsRival && x.Index == tarIndex);//找到Defenser，把它弄死
                    if (defenserIndex >= 0)
                    {
                        Destroy(DefenserList[defenserIndex]);
                        DefenserList.RemoveAt(defenserIndex);
                    }

                    if (tarIndex == 0)
                    {
                        //StateFlag.ShowGameOver();
                        if (OpponentState == OpponentStateEnum.RemoteChallenge)
                        {
                            EndChallengeRound(true);
                        }
                        else if (OpponentState == OpponentStateEnum.FreshmanGuide)
                        {
                            //新手教程三消部分结局
                            FreshmanGuide.GotoNext();
                        }
                    }
                }
                else
                {
                    RivalCharacterList[RivalLogic2CharacterIndex[tarIndex]].BeAttacked();
                }
            }
        }

        IEnumerator DelayToBeMagicallyAttack(bool ourCharacter, int index, float delay)
        {
            if (delay >= 0)
            {
                yield return new WaitForSeconds(delay);
                (ourCharacter ? OurCharacterList : RivalCharacterList)[
                    (ourCharacter ? OurLogic2CharacterIndex : RivalLogic2CharacterIndex)[index]].BeMagicallyAttacked();
            }
        }

        public void Execute(UseSkillOk cmd)
        {
            //从Suspending列表移除，表示该技能已被服务端确认Ok
            var ind = _mySuspendingSkillList.FindIndex(x => x.SkillCode == (SkillEnum) cmd.SkillCode);
            if (ind >= 0)//正常
            {
                _mySuspendingSkillList.RemoveAt(ind);
            }
            else//技能过期了
            {
                ind = _myOverdueSkillList.FindIndex(x => x.SkillCode == (SkillEnum) cmd.SkillCode);
                if (ind >= 0)
                {
                    var useSkillInfo = _myOverdueSkillList[ind];
                    var skillLevel = SkillUtil.GetMySkillLevel(useSkillInfo.SkillCode);
                    SanxiaoTakeEffect(useSkillInfo.ByOur, useSkillInfo.SkillCode, skillLevel);
                    _playingSkillList.Remove(useSkillInfo);
                    _myOverdueSkillList.RemoveAt(ind);
                }
                else
                {
                    Debug.LogError("按照算法不应该此时两个列表都找不到这个技能事件");
                }
            }

            Execute(cmd.SyncData);

            if (cmd.HasKo && cmd.Ko)
            {
                StartCoroutine(_Die(true));
            }
        }

        public void Execute(RivalUseSkill cmd)
        {
            StartCoroutine(
                SomeoneUseSkill(new UseSkillInfo(false, 0, (SkillEnum) cmd.SkillCode, new[] {cmd.PhysicalDamage, 0, 0})));

            if (cmd.HasKo && cmd.Ko)
            {
                //TODO:FightingPanel.PlayKOEffect();
                StartCoroutine(_Die(false));
            }
        }

        /// <summary>
        /// 三消生效
        /// </summary>
        void SanxiaoTakeEffect(bool byOur, SkillEnum skillCode, int skillLevel)
        {
            if (byOur)
            {
                switch (skillCode)
                {
                    case SkillEnum.CreateStripe:
                    case SkillEnum.CreateBomb:
                    case SkillEnum.CreateColorful:
                    case SkillEnum.AllHint:
                    case SkillEnum.GoldenFinger:
                        SkillSanxiaoEffectHolder.AddComponent<SkillSanxiaoEffect>()
                                                .Initialize(skillCode, skillLevel, this);
                        break;
                }
            }
            else
            {
                switch (skillCode)
                {
                    case SkillEnum.Ice:
                    case SkillEnum.Shake:
                    case SkillEnum.Stone:
                    case SkillEnum.Lock:
                    case SkillEnum.Brick:
                        SkillSanxiaoEffectHolder.AddComponent<SkillSanxiaoEffect>()
                                                .Initialize(skillCode, skillLevel, this);
                        break;
                }
            }
        }

        private IEnumerator _Die(bool rivalDie)
        {
            yield return new WaitForSeconds(3);
            if (rivalDie)
            {
                RivalCharacterList[RivalLogic2CharacterIndex[0]].Die();
            }
            else
            {
                MyCharacter.Die();
            }
        }

        GameObject GetSkillTrajectoryEffectPrefab(SkillEnum skillCode)
        {
            var prefab = MorlnResources.Load<GameObject>("UI/GameUI/TrajectorySkillEffects/SkillTrajectoryEffect-" + skillCode);
            return prefab;
        }

        public GameObject DamageLabelTemplate;
        public Transform MyDamageLabelContainer, RivalDamageLabelContainer;
        void CreateDamageLabel(bool onUs, int index, long damage)
        {
            FloatingLabel fl;
            Vector3 locPos;
            if (onUs)
            {
                locPos =
                    MyDamageLabelContainer.InverseTransformPoint(
                        OurCharacterList[OurLogic2CharacterIndex[index]].transform.position).AddV3Y(100);
                fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(DamageLabelTemplate, MyDamageLabelContainer);
            }
            else
            {
                locPos =
                    RivalDamageLabelContainer.InverseTransformPoint(
                        RivalCharacterList[RivalLogic2CharacterIndex[index]].transform.position).AddV3Y(100);
                fl = PrefabHelper.InstantiateAndReset<FloatingLabel>(DamageLabelTemplate, RivalDamageLabelContainer);
            }
            fl.transform.localPosition = locPos;
            fl.gameObject.SetActive(true);
            fl.Reset(String.Format("-{0}", damage));
        }

        public SkillMaskContainer SkillMaskContainer;

        #region 全局提示

        private float _allHintEndTime;
        public void PlaySkillEffect_AllHints(float time)
        {
            _allHintEndTime = Mathf.Max(_allHintEndTime, Time.time + time);
            StopCoroutine("AllHintsCoroutine");
            StartCoroutine(AllHintsCoroutine());
        }
        IEnumerator AllHintsCoroutine()
        {
            while (true)
            {
                if (Time.frameCount%5 == 0)//不要每帧都检测提示
                {
                    var hintList = AI.GetAllHintExchanges(MyGrid);
                    if (hintList.Count > 0)
                    {
                        CellEffectContainer.ShowAllHintLines(hintList);
                    }
                    else
                    {
                        CellEffectContainer.ClearAllHintLines();
                    }
                }
                yield return new WaitForEndOfFrame();
                if (Time.time > _allHintEndTime) break;
            }
            CellEffectContainer.ClearAllHintLines();
        }
        #endregion

        #region HideGenre

        private bool _isDurationHideGenreState;
        private float _hideGenreEndTime;
        public void PlaySkillEffect_HideGenre(float time)
        {
            _hideGenreEndTime = Mathf.Max(_hideGenreEndTime, Time.time + time);
            StopCoroutine("HideGenreCoroutine");
            StartCoroutine(HideGenreCoroutine());
        }
        IEnumerator HideGenreCoroutine()
        {
            _isDurationHideGenreState = true;

            #region 消除所有已隐藏Genre的糖果

            for (int i = 0; i < Grid.Height; i++)
            {
                for (int j = 0; j < Grid.Width; j++)
                {
                    if (MyGrid[i, j].MyCandy && MyGrid[i, j].MyCandy.Genre == CurCandyGenreCount)
                    {
                        MyGrid[i, j].MyCandy.Pop(0);
                        MyGrid[i, j].MyCandy = null;
                    }
                }
            }
            #endregion

            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (Time.time > _hideGenreEndTime) break;
            }
            _isDurationHideGenreState = false;
        }
        #endregion

        #endregion

        #region SyncData

        public void Execute(SyncData cmd)
        {
            if (_playingSkillList.Count == 0)
            {
                Debug.LogWarning("rivalHealth,la:"+GameData.RivalHealth+", new:"+cmd.RivalHealth);
                GameData.MyHealth = cmd.MyHealth;
                GameData.RivalHealth = cmd.RivalHealth;
                GameData.RivalEnergy = cmd.RivalEnergy;
            }

            Requester.Instance.Send(new SyncDataResponse(GameData.MyEnergy));
        }

        #endregion

        /// <summary>
        /// 实时对战结束，由服务端发起
        /// </summary>
        /// <param name="cmd"></param>
        public void Execute(EndRound cmd)
        {
            GameState = GameStateEnum.Ending;
            MyGrid.EndRound();
            if (CurLocalServer != null)
            {
                Destroy(CurLocalServer.gameObject); //销毁对手AI
                CurLocalServer = null;
            }

            //StateFlag.ShowGameOver();

            Time.timeScale = 0.333f;
            StartCoroutine(_ShowFightEndEffectDelay());
        }
        IEnumerator _ShowFightEndEffectDelay()
        {
            yield return new WaitForSeconds(1);
            Time.timeScale = 1;
            PrefabHelper.InstantiateAndReset(FightEndEffectPrefab, transform);
        }

        /// <summary>
        /// 异步挑战结束，由客户端自行发起
        /// </summary>
        /// <param name="win"></param>
        public void EndChallengeRound(bool win)
        {
            if (GameState == GameStateEnum.Ending) return;
            GameState = GameStateEnum.Ending;
            GameData.LastRoundWin = win; //记录胜负
            MyGrid.EndRound();
            MainController.Instance.EndChallengeRound();
            if (CurLocalServer != null)
            {
                Destroy(CurLocalServer.gameObject); //销毁对手AI
                CurLocalServer = null;
            }
            foreach (var defenser in DefenserList)
            {
                Destroy(defenser);
            }
            DefenserList.Clear();

            var myUser = CommonData.MyUser;
            var cmd = new UploadChallenge(GameData.LastChallengeID, win,
                                          new DefenseData(myUser.Nickname, myUser.Level, myUser.CharacterCode,
                                                          myUser.RoundInitHealth, myUser.EnergyCapacity));
            Requester.Instance.Send(cmd);

            UMengPlugin.UMengEvent(win ? EventId.PUSH_WIN : EventId.PUSH_LOSE,
                                   new Dictionary<string, object>
                                       {
                                           {"major", GameData.LastChallengeMajorLevelID},
                                           {"sub", GameData.LastChallengeSubLevelID},
                                       }); //发送统计事件
        }

        public IEnumerator EndChallengeRoundCoroutine()
        {
            Time.timeScale = 0.333f;

            yield return new WaitForSeconds(1.7f);
            Time.timeScale = 1;
            PrefabHelper.InstantiateAndReset(FightEndEffectPrefab, transform);
            yield return new WaitForSeconds(1f);

            while (GameData.LastUploadChallengeOkCmd == null) //等待拿到UploadChallengeOk，再进入结局界面
            {
                yield return new WaitForEndOfFrame();
            }

            MainController.Instance.EndPushLevelRoundGotoEndRoundUI();
        }

        public GameObject FightEndEffectPrefab;

        #region Shuffle洗牌

        private void OnShuffleClick()
        {
            //TODO:只能在全部Static时才能使用


            //TODO:检测货币是否够
            MyGrid.Shuffle();
            //TODO:扣除货币
            Requester.Instance.Send(new Shuffle());//发送命令
        }

        #endregion


        #region 对手模拟

        public GameObject LocalServerPrefab;

        public LocalServer CurLocalServer;

        public GameObject DefenserHolder;
        public readonly List<Defenser> DefenserList = new List<Defenser>();

        /// <summary>
        /// 如果没有LocalServer就创建一个，保证一定会有CurLocalServer
        /// </summary>
        public void SetupLocalServer()
        {
            if (!CurLocalServer) CurLocalServer = PrefabHelper.InstantiateAndReset<LocalServer>(LocalServerPrefab, transform);//创建LocalServer
        }
        #endregion

        #region 暂停/继续
        public void Pause()
        {
            if (GameState == GameStateEnum.Fighting)
            {
                GameUI.PausePanel.SetActive(true);
                if (OpponentState != OpponentStateEnum.RemoteRealPerson)
                {
                    Time.timeScale = 0;
                }
            }
        }
        public void Resume()
        {
            GameUI.PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        public void Quit()
        {
            Time.timeScale = 1;
            MainController.Instance.QuitPushLevelRoundGotoPushLevelUI();
            
        }
        #endregion

        void OnTest0Click()
        {
            Responder.Instance.Execute(new StartRound());
        }
        void OnTest1Click()
        {
            Responder.Instance.Execute(new EndRound
            {
                MyInfo = TestData.User0,
                RivalInfo = TestData.User1,
                Win = true
            });
        }
        void OnTestAllHintClick()
        {
            PlaySkillEffect_AllHints(10);
        }
    }
}