using Assets.Sanxiao.UI.Menu;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class StopNews : Proto.StopNews, IUpperReceivedCmd
    {
        public void Execute()
        {
            NewsSubtitle.NewsQueue.RemoveAll(x => x.NewsId == NewsId);
        }
    }
}