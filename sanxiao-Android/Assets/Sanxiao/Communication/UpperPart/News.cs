using Assets.Sanxiao.UI.Menu;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class News : Proto.News, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (!NewsSubtitle.NewsQueue.Exists(x => x.NewsId == NewsId)) NewsSubtitle.NewsQueue.Add(this);
        }
    }
}