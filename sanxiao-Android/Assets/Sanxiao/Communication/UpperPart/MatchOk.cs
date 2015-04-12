using Assets.Sanxiao.UI;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class MatchOk : Proto.MatchOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.RivalUser = RivalInfo;

            MainController.Instance.Execute(this);
        }
    }
}