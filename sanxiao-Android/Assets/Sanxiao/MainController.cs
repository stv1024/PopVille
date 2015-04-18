using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI;
using Assets.Sanxiao.UI.Panel;
using Assets.Scripts;
using UnityEngine;
using DeviceLogin = Assets.Sanxiao.Communication.UpperPart.DeviceLogin;
using EndRound = Assets.Sanxiao.Communication.UpperPart.EndRound;
using LoginOk = Assets.Sanxiao.Communication.UpperPart.LoginOk;
using MatchOk = Assets.Sanxiao.Communication.UpperPart.MatchOk;
using NeedOAuthInfo = Assets.Sanxiao.Communication.UpperPart.NeedOAuthInfo;
using RequestChallengeOk = Assets.Sanxiao.Communication.UpperPart.RequestChallengeOk;
using RequestConfig = Assets.Sanxiao.Communication.UpperPart.RequestConfig;
using RequestMyHeartInfo = Assets.Sanxiao.Communication.UpperPart.RequestMyHeartInfo;
using RequestUserVegetable = Assets.Sanxiao.Communication.UpperPart.RequestUserVegetable;
using StartChallenge = Assets.Sanxiao.Communication.UpperPart.StartChallenge;
using StartRound = Assets.Sanxiao.Communication.UpperPart.StartRound;
using UploadChallengeOk = Assets.Sanxiao.Communication.UpperPart.UploadChallengeOk;
using Config = Assets.Sanxiao.Communication.UpperPart.Config;
using RefreshMailList = Assets.Sanxiao.Communication.UpperPart.RefreshMailList;

namespace Assets.Sanxiao
{
    /// <summary>
    /// 主控制层。管理各种状态，从开局到结局的细节由GameManager控制
    /// </summary>
    public class MainController : MonoBehaviour, NetworkManager.INetworkEventListener
    {
        public static MainController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            NetworkManager.Instance.AddListener(this);//监听网络事件

            SystemSettings.LoadSettingsFromHD();//从硬盘读取系统设置，如果没有会有默认值

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            ConfigManager.SetConfig(ConfigManager.ConfigType.CharacterConfig, new CharacterConfig
            {
                CharacterList = new List<Communication.Proto.Character>{
                    new Communication.Proto.Character(1, 0)
                }
            });

            CommonData.MyCharacterList = new List<UserCharacter>{
                new UserCharacter(1)
            };
        }
        void OnDestroy()
        {
            NetworkManager.Instance.RemoveListener(this);//去除监听器
        }

        private void Start()
        {
            try
            {
                ConfigManager.ReadCacheAndLoadAllLargeConfigs(); //读取并加载大配置
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            //TODO：获取IP
            //TODO：连接服务器
            //TODO：登录

            if (!ClientInfoHolder.Instance)
            {
                Debug.LogError("没有ClientInfo Holder，必须检查");
                return;
            }

            MainRoot.Goto(MainRoot.UIStateName.Entrance);

            ImageResourcesManager.Init(7);

            #region GameCenter

            Debug.Log("Social:" + Social.Active);
            Social.localUser.Authenticate(success =>
                {
                    if (success)
                    {
                        Debug.Log("GameCenter登录成功");
                        var userInfo = "Username: " + Social.localUser.userName +
                                       "\nUser ID: " + Social.localUser.id +
                                       "\nIsUnderage: " + Social.localUser.underage;
                        Debug.Log(userInfo);
                        Social.localUser.LoadFriends(re =>
                            {
                                if (re)
                                {
                                    Debug.Log("LoadFriends ok");
                                    foreach (var userProfile in Social.localUser.friends)
                                    {
                                        Debug.Log("f:" + userProfile.id + ":" + userProfile.userName);
                                    }
                                }
                                else
                                {
                                    Debug.Log("LoadFriends fail");
                                }
                            });
                    }
                    else
                        Debug.Log("Authentication failed");
                });

            #endregion
        }

        private bool _inLoginProcess = true, _loginOk, _syncConfigOk, _downloadAssetBundleOk = false;
        void Update()
        {

            #region 检测爱心是否送达
            if (CommonData.HeartData.Count < CommonData.HeartData.MaxCount && Time.realtimeSinceStartup >= CommonData.HeartData.NextHeartRealTime)//当前数量未达到上限，并且达到了
            {

            }
            #endregion

            #region 检测蔬菜是否成熟

            if (Time.frameCount % 5 == 0 && !(State is State.Fighting))
            {
                foreach (var vmi in CommonData.MyVegetableMatureInfoList)
                {
                    if (vmi.MatureTime <= Time.realtimeSinceStartup)
                    {
                        Requester.Instance.Send(new RequestUserVegetable(vmi.Code));
                    }
                }
                CommonData.MyVegetableMatureInfoList.RemoveAll(x => x.MatureTime <= Time.realtimeSinceStartup);
            }
            #endregion
        }

        #region 疲劳值爱心系统
        /// <summary>
        /// 定时下一次获取爱心信息，会自动清除已有的定时
        /// </summary>
        /// <param name="time"></param>
        public void WaitingCheckHeart(float time)
        {
            ClearCheckHeartInvoke();
            if (time < 2) time = 2;//防止服务端数据错误导致疯狂发消息
            Invoke("RequestHeartInfo", time);
        }
        /// <summary>
        /// 清除获取爱心信息的定时事件。注销账号时记得调用
        /// </summary>
        public void ClearCheckHeartInvoke()
        {
            CancelInvoke("RequestHeartInfo");
        }
        void RequestHeartInfo()//TODO:防止不停地发
        {
            Requester.Instance.Send(new RequestMyHeartInfo());
        }

        #endregion

        #region Network Events 网络状态事件

        public void ConnectedToServer()
        {
            Requester.Instance.Send(new DeviceLogin(DeviceUID, ClientInfoHolder.GetClientInfo()));
            if (EntranceUI.Instance) EntranceUI.Instance.SetProgressValue(1f);
        }

        public void CannotConnectToServer()
        {
            Debug.LogError("CannotConnectToServer");
            UMengPlugin.UMengEvent(EventId.CANT_CONNECT_SERVER,
                                   new Dictionary<string, object> {{"net", (int) Application.internetReachability}});
            AlertDialog.Load("网络无法连接，请重试", "确定", ReLogin);
        }

        public void DisConnectToServer()
        {
            Debug.LogError("DisConnectToServer");
            UMengPlugin.UMengEvent(EventId.DIS_CONNECT_SERVER,
                                   new Dictionary<string, object> { { "net", (int)Application.internetReachability } });
            AlertDialog.Load("网络已经断开，请重试", "确定", ReLogin);
        }

        #endregion

        #region 登录

        public void Execute(LoginOk cmd)
        {
            _loginOk = true;

            Requester.Instance.Send(new RequestMyHeartInfo());

            //申请好友列表
            Requester.Instance.Send(new RequestSNSFriendInfoList());

            //申请信件
            Requester.Instance.Send(new RefreshMailList(2));

            //淡入音乐
            MusicManager.Instance.CrossFadeIn();

            //检测大型Config有无变化
            var configIdList = new List<int>();
            var values = Enum.GetValues(typeof (ConfigManager.ConfigType)) as ConfigManager.ConfigType[];
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (i < cmd.ConfigHash.ConfigHashList.Count)
                    {
                        if (cmd.ConfigHash.ConfigHashList[i] != ConfigManager.GetHash((ConfigManager.ConfigType) i))
                            configIdList.Add(i);
                    }
                    else
                    {
                        Debug.LogError("服务端ConfigHash.ConfigHashList长度不对.count:" + cmd.ConfigHash.ConfigHashList.Count);
                    }
                }
            }
            else
            {
                Debug.LogError("这怎么能是null，那还怎么获取Config");
            }
            if (configIdList.Count > 0)
            {
                _syncConfigOk = false;
                Requester.Instance.Send(new RequestConfig
                    {
                        ConfigIdList = configIdList
                    });
            }
            else
            {
                _syncConfigOk = true;
            }

            CommonData.FirstTimeGuide = cmd.HasFirstTimeGuide && cmd.FirstTimeGuide;
            if (CommonData.FirstTimeGuide)
            {
                Requester.Instance.Send(new RequestNickname());//提前准备好随机昵称列表
                MenuUI.HighlightAdventureBtn = true;//让冒险模式的按钮可以弹起来强调

                UMengPlugin.UMengEvent(EventId.VERY_BEGINNING, new Dictionary<string, object>{{"hour", DateTime.Now.Hour}});
            }

            //推送模块设置
            PushInfoManager.Instance.Init();

            //下载动态资源
            if (!MorlnDownloadResources.DownloadAssetBundle)
            {
                StartCoroutine(MorlnDownloadResources.ForceLoadAssetBundle());
            }
        }

        public void Execute(Config cmd)
        {
            _syncConfigOk = true;
        }
        #endregion

        #region 匹配、挑战流程

        public void Execute(MatchOk cmd)
        {
            if (!MatchUI.Instance) return;//状态出错，别玩了
            
            GameData.RealTimeToStartRound = Time.realtimeSinceStartup + cmd.StartSeconds;

            GameData.FriendDataList = null;
            GameData.RivalBossData = null;
            GameData.FellowDataList = null;

            if (CommonData.MyUser != null) CommonData.MyUserOld = CommonData.MyUser.GetDuplicate(); //备份旧的MyUser
            if (CommonData.RivalUser != null)
                CommonData.RivalUserOld = CommonData.RivalUser.GetDuplicate(); //备份旧的RivalUser

            MatchUI.Instance.RefreshRivalInfoBeforeRound(cmd.StartSeconds * 0.5f);//显示对手角色，倒计时，提前进入游戏界面

            //TODO:显示使用爱心的动画,移到推图那里
            Requester.Instance.Send(new RequestMyHeartInfo());//获取爱心数据，确保客户端爱心数据正确

            //定时进入游戏界面
            StartCoroutine(GotoPlayingRealTimeCoroutine(Mathf.Max(0, cmd.StartSeconds * 0.5f)));

            MusicManager.Instance.CrossFadeOut();
        }
        IEnumerator GotoPlayingRealTimeCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            MainRoot.Goto(MainRoot.UIStateName.Game);
            while (!GameManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            GameManager.Instance.ResetAndRefreshBeforeRealTimeFighting();
            CandyBAPool.Instance.PrepareAllCandys();
        }
        public void Execute(StartRound startRound)
        {
            State = new State.PlayingRealTime();//切状态

            //MainRoot.Goto(MainRoot.UIStateName.Game);//确保进入游戏界面

            GameManager.Instance.StartRealTimeRound();
        }

        public void Execute(RequestChallengeOk cmd)
        {
            GameData.LastChallengeMajorLevelID = cmd.MajorLevelId;
            GameData.LastChallengeSubLevelID = cmd.SubLevelId;

            //为之后准备好SubLevelData
            var hasCorrectsubLevelData = false;
            var challengeLevelConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.ChallengeLevelConfig) as ChallengeLevelConfig;
            if (challengeLevelConfig != null)
            {
                var majorLevelData =
                    challengeLevelConfig.MajorLevelList.Find(x => x.MajorLevelId == GameData.LastChallengeMajorLevelID);
                if (majorLevelData != null)
                {
                    GameData.LastMajorLevelData = majorLevelData;
                    GameData.LastSubLevelData =
                        majorLevelData.SubLevelList.Find(x => x.SubLevelId == GameData.LastChallengeSubLevelID);
                    hasCorrectsubLevelData = true;
                }
            }
            if (!hasCorrectsubLevelData)
            {
                GameData.LastMajorLevelData = null;
                GameData.LastSubLevelData = null;
            }

            GameData.LastChallengeID = cmd.ChallengeId;
            GameData.FellowDataList = cmd.FellowDataList;
            GameData.RivalBossData = cmd.BossData;

            MainRoot.Goto(MainRoot.UIStateName.Match);
            if (MatchUI.Instance) MatchUI.Instance.RefreshBeforeChallenge();
        }
        public void Execute(StartChallenge cmd)
        {
            GameData.FriendDataList = cmd.FriendDataList;
            GameData.LastChallengeID = cmd.ChallengeId;

            MainRoot.Goto(MainRoot.UIStateName.Game);

            if (CommonData.MyUser != null) CommonData.MyUserOld = CommonData.MyUser.GetDuplicate(); //备份旧的MyUser

            StartCoroutine(GotoPlayingChallengeCoroutine(2));//最好有倒计时

            MusicManager.Instance.CrossFadeOut();//音乐渐出

            CandyBAPool.Instance.PrepareAllCandys();//预加载所有糖果
        }

        IEnumerator GotoPlayingChallengeCoroutine(float delay)
        {
            var curT = Time.time;
            while (!GameManager.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            GameManager.Instance.ResetAndRefreshBeforeChallengeFighting(2);
            while (Time.time-curT < delay)
            {
                yield return new WaitForEndOfFrame();
            }

            State = new State.PlayingChallenge();//切状态

            MainRoot.Goto(MainRoot.UIStateName.Game);//确保进入了GameUI，一般无用

            GameManager.Instance.StartChallengeRound();
        }

        public void Execute(EndRound cmd)
        {
            GameData.LastRoundWin = cmd.Win;

            State = new State.Shell();
            MusicManager.Instance.CrossFadeIn();
            StartCoroutine(GotoEndRoundUICoroutine(cmd));
        }
        IEnumerator GotoEndRoundUICoroutine(EndRound cmd)
        {
            yield return new WaitForSeconds(2);
            MainRoot.Goto(MainRoot.UIStateName.EndRound);
            while (!EndRoundUI.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            EndRoundUI.Instance.PlayEndRoundProcess(cmd);
        }

        public void EndChallengeRound()
        {
            State = new State.Shell();
        }
        public void GiveUpRealPersonRound()
        {
            State = new State.Shell();
            MusicManager.Instance.CrossFadeIn();
            MainRoot.Goto(MainRoot.UIStateName.Menu);
        }
        #endregion

        #region 结算

        public void Execute(UploadChallengeOk cmd)
        {
            StartCoroutine(GotoEndRoundUICoroutine(cmd));
        }

        IEnumerator GotoEndRoundUICoroutine(UploadChallengeOk cmd)
        {
            yield return new WaitForSeconds(2);


            foreach (var currency in cmd.RoundRewardList)
            {
                switch ((CurrencyType)currency.Type)
                {
                    case CurrencyType.Diamond:
                        CommonData.MyUser.Money1 += currency.Amount;
                        break;
                    case CurrencyType.Coin:
                        CommonData.MyUser.Money10 += currency.Amount;
                        break;
                    case CurrencyType.Exp:
                        CommonData.MyUser.Exp += currency.Amount;
                        break;
                    case CurrencyType.Heart:
                        CommonData.HeartData.Count += currency.Amount;
                        break;
                    case CurrencyType.PkCount:
                        //TODO:PK次数
                        break;
                }
            }

            if (cmd.HasUnlockElement)
            {
                var ue = cmd.UnlockElement;
                if (ue.HasLevelUp)
                {
                    CommonData.MyUser.Exp = ue.LevelUp.ToExp;
                    CommonData.MyUser.Level = ue.LevelUp.ToLevel;
                    //TODO:经验值上下限呢
                }
            }
            GameData.LastUploadChallengeOkCmd = cmd;
        }
        /// <summary>
        /// 推图挑战结束后，GameManager播放完慢镜头，然后调用该方法进入结局界面
        /// </summary>
        public void EndPushLevelRoundGotoEndRoundUI()
        {
            MainRoot.Goto(MainRoot.UIStateName.EndRound);
            StartCoroutine(_EndPushLevelRoundGotoEndRoundUI());
        }
        IEnumerator _EndPushLevelRoundGotoEndRoundUI()
        {
            while (!EndRoundUI.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            EndRoundUI.Instance.PlayEndRoundProcess(GameData.LastUploadChallengeOkCmd);
        }

        public void QuitPushLevelRoundGotoPushLevelUI()
        {
            State = new State.Shell();
            MainRoot.Goto(MainRoot.UIStateName.Entrance);
        }
        #endregion

        public State.IState State { get; private set; }

        #region Util

        public static string DeviceUID
        {
            get { return SystemInfo.deviceUniqueIdentifier; }//TODO:考虑iOS
        }
        #endregion

        /// <summary>
        /// 某些操作，如清除缓存或遇到略致命错误后，需要重新登录
        /// </summary>
        public void ReLogin()
        {
            Debug.LogWarning("重新登录游戏！@" + Time.realtimeSinceStartup);
            _inLoginProcess = true;
            _loginOk = false;
            _syncConfigOk = false;
            _downloadAssetBundleOk = true;
            Start();
        }

        /// <summary>
        /// 遇到致命错误，必须重启游戏时使用
        /// </summary>
        public static void RestartApp()
        {
            Debug.LogWarning("重启游戏！@" + Time.realtimeSinceStartup);
        }

        public void Execute(NeedOAuthInfo cmd)
        {
            string snsPlatform;
            switch (cmd.Type)
            {
                case 0:
                    snsPlatform = "微博";
                    break;
                case 1:
                    snsPlatform = "腾讯微博";
                    break;
                case 101:
                    snsPlatform = "360";
                    break;
                case 102:
                    snsPlatform = "小米";
                    break;
                case 103:
                    snsPlatform = "百度";
                    break;
                default:
                    snsPlatform = "社交";
                    break;
            }
            string text = null;
            if (cmd.HasName)
            {
                text = string.Format("您的{0}账号:{1}\n已经过期，赶紧重新授权吧", snsPlatform, cmd.Name); //TODO:美术提供颜色
            }
            else
            {
                text = "增加游戏乐趣，保护账号安全。是否现在绑定社交账号？";
            }
            AlertDialog.Load(text, "点击授权", () =>
                {
                    BindSnsPanel.Load();
                    switch (cmd.Type)
                    {
                        case 0:
                            BindSnsPanel.Instance.OnSinaWeiboClick();
                            break;
                            //TODO:其他社交账号处理
                    }
                }, true);
        }
    }

    /// <summary>
    /// 逻辑状态{Shell非游戏状态, Fighting游戏状态{实时, 异步挑战}}
    /// </summary>
    public static class State
    {
        public interface IState
        {
             
        }
        public class Shell : IState
        {
             
        }
        public abstract class Fighting : IState
        {
             
        }
        public class PlayingRealTime : Fighting
        {

        }
        public class PlayingChallenge : Fighting
        {
             
        }
    }
}