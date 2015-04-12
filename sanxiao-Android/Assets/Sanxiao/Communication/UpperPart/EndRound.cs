using System.Collections.Generic;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class EndRound : Proto.EndRound, IUpperReceivedCmd
    {
        public void Execute()
        {
            UMengPlugin.UMengEvent(Win ? EventId.MULTI_WIN : EventId.MULTI_LOSE, null);

            //更新两人的玩家数据
            CommonData.MyUser = MyInfo;
            CommonData.RivalUser = RivalInfo;

            //记录这一局是不是赢了
            CommonData.IsLastRoundWin = Win;

            MainController.Instance.Execute(this);

            if (GameManager.Instance) GameManager.Instance.Execute(this);
        }
    }
}