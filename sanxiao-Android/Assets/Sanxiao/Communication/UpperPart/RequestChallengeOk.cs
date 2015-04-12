using Assets.Sanxiao.Data;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestChallengeOk : Proto.RequestChallengeOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            MainController.Instance.Execute(this);
        }
    }
}