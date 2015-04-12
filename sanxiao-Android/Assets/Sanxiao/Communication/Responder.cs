using System;
using Assets.Sanxiao.Communication.UpperPart;
using UnityEngine;

namespace Assets.Sanxiao.Communication
{
    public class Responder
    {
        private Responder()
        {

        }

        private static Responder _instance;

        public static Responder Instance
        {
            get { return _instance ?? (_instance = new Responder()); }
        }

        public void Response(Proto.Packet packet)
        {
            IUpperReceivedCmd cmd = null;
            switch (packet.Type) //Packet→Cmd
            {
                case 0:
                    cmd = new HBReq();
                    break;
                case 1:
                    //客户端主动发心跳的回复，不需操作
                    break;
                case 111:
                    cmd = new News();
                    break;
                case 112:
                    cmd = new StopNews();
                    break;
                case 121:
                    cmd = new Poster();
                    break;
                case 1002:
                    cmd = new LoginOk();
                    break;
                case 1003:
                    cmd = new LoginFail();
                    break;
                case 1102:
                    cmd = new Config();
                    break;
                #region 2000+
                case 2002:
                    cmd = new EditUserInfoResult();
                    break;
                case 2004:
                    cmd = new NicknameProvided();
                    break;
                case 2012:
                    cmd = new UserHeartInfo();
                    break;
                case 2022:
                    cmd = new UpgradeSkillOk();
                    break;
                case 2023:
                    cmd = new UpgradeSkillFail();
                    break;
                case 2025:
                    cmd = new UpgradeVegetableOk();
                    break;
                case 2026:
                    cmd = new UpgradeVegetableFail();
                    break;
                case 2032:
                    cmd = new BuyCharacterOk();
                    break;
                case 2033:
                    cmd = new BuyCharacterFail();
                    break;
                case 2042:
                    cmd = new SpeedUpVegetableUpgradeOk();
                    break;
                case 2043:
                    cmd = new SpeedUpVegetableUpgradeFail();
                    break;
                case 2102:
                    cmd = new ExchangeOk();
                    break;
                case 2103:
                    cmd = new ExchangeFail();
                    break;
                case 2202:
                    cmd = new UseEquipOk();
                    break;
                case 2203:
                    cmd = new UseEquipFail();
                    break;
                case 2212:
                    cmd = new ChangeCharacterOk();
                    break;
                case 2213:
                    cmd = new ChangeCharacterFail();
                    break;
                case 2222:
                    cmd = new UserVegetable();
                    break;
                case 2223:
                    cmd = new RequestUserVegetableFail();
                    break;
                case 2232:
                    cmd = new UserMailList();
                    break;
                case 2242:
                    cmd = new RequestMailGiftOK();
                    break;
                case 2243:
                    cmd = new RequestMailGiftFail();
                    break;
                case 2301:
                    cmd = new NeedOAuthInfo();
                    break;
                case 2303:
                    cmd = new BindOAuthInfoOk();
                    break;
                case 2304:
                    cmd = new BindOAuthInfoFail();
                    break;
                case 2312:
                    cmd = new SNSFriendInfoList();
                    break;
                case 2322:
                    cmd = new RandomTeamMemberList();
                    break;
                #endregion
                case 10003:
                    cmd = new MatchOk();
                    break;
                case 10004:
                    cmd = new MatchFail();
                    break;
                case 10021:
                    cmd = new StartRound();
                    break;
                case 10022:
                    cmd = new EndRound();
                    break;
                case 10023:
                    cmd = new SyncData();
                    break;
                case 10032:
                    cmd = new UseSkillOk();
                    break;
                case 10033:
                    cmd = new UseSkillFail();
                    break;
                case 10034:
                    cmd = new RivalUseSkill();
                    break;
                #region 挑战
                case 10102:
                    cmd = new RequestChallengeOk();
                    break;
                case 10103:
                    cmd = new RequestChallengeFail();
                    break;
                case 10105:
                    cmd = new StartChallenge();
                    break;
                case 10106:
                    cmd = new RequestStartChallengeFail();
                    break;
                case 10108:
                    cmd = new UploadChallengeOk();
                    break;
                case 10109:
                    cmd = new UploadChallengeFail();
                    break;
                #endregion
                #region 排行榜
                case 10202:
                    cmd = new Leaderboard();
                    break;
                #endregion
                default:
                    Debug.LogError("未处理的Cmd:" + packet.Type);
                    break;
            }
            if (cmd != null)
            {
                try
                {
                    if (packet.Content != null) cmd.ParseFrom(packet.Content);
                    Debug.Log(string.Format("{0}:{1}", cmd.GetType().Name, cmd));
                    Execute(cmd);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        public void Execute(IUpperReceivedCmd cmd)
        {
            if (cmd == null)
            {
                Debug.LogError("cmd cannot be null!");
                return;
            }
            cmd.Execute();
        }
    }
}