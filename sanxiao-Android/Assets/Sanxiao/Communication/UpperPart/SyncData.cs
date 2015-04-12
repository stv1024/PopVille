using Assets.Sanxiao.Game;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class SyncData : Proto.SyncData, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Execute(this);
            }
        }
    }
}