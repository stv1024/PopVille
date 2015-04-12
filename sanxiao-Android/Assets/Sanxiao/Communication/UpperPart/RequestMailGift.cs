using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestMailGift : Proto.RequestMailGift, IUpperSentCmd
    {
        public RequestMailGift(string mailId) : base(mailId)
        {
        }

        public int CmdType
        {
            get { return 2241; }
        }
    }
}