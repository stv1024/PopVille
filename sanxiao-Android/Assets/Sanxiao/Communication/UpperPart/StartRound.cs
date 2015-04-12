namespace Assets.Sanxiao.Communication.UpperPart
{
    public class StartRound : Proto.StartRound, IUpperReceivedCmd
    {
        public void Execute()
        {
            MainController.Instance.Execute(this);
        }
    }
}
