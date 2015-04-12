using System.Collections.Generic;
using System.Globalization;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Test;
using UnityEngine;
using StartChallenge = Assets.Sanxiao.Communication.UpperPart.StartChallenge;

namespace Assets.Sanxiao.UI.Panel.Leaderboard
{
    /// <summary>
    /// 排行榜条目
    /// </summary>
    public class LeaderboardSlot : MonoBehaviour
    {
        public UILabel LblCurRank;
        public UILabel LblNickname;

        private LeaderboardItem _leaderboardItem;

        public void SetAndRefresh(LeaderboardItem leaderboardItem)
        {
            _leaderboardItem = leaderboardItem;

            if (_leaderboardItem != null)
            {
                LblCurRank.text = _leaderboardItem.GlobalRank.ToString(CultureInfo.InvariantCulture);
                LblNickname.text = _leaderboardItem.Nickname;
            }
            else
            {
                LblCurRank.text = null;
                LblNickname.text = null;
            }
        }

        public void OnChallengeClick()
        {
            if (_leaderboardItem == null)//TODO:测试用
            {
                Responder.Instance.Execute(TestData.Leaderboard0);
                return;
            }
            //Requester.Instance.Send(new RequestChallenge(_leaderboardItem.UserId,));

            if (_leaderboardItem.UserId == CommonData.MyUser.UserId) //TODO:测试用
            {
                Debug.LogWarning("挑战自己，测试用");
                Responder.Instance.Execute(new StartChallenge
                    {
                        ChallengeId = "04F3E163-1286-4657-B743-FAAB42096444",
                        FriendDataList = new List<TeamAdd> {TestData.TeamAdd0, TestData.TeamAdd1}
                    });
            }
        }
    }
}