using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class Leaderboard : Proto.Leaderboard, IUpperReceivedCmd
    {
        public void Execute()
        {
            LeaderboardData.Data = this;

            if (LeaderboardPanel.Instance != null) LeaderboardPanel.Instance.Execute(this);
        }
    }
}