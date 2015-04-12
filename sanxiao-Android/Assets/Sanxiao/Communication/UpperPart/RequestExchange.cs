using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestExchange : Proto.RequestExchange, IUpperSentCmd
    {
        public RequestExchange(string name, int count)
            : base(name, count)
        {
        }

        public int CmdType
        {
            get { return 2101; }
        }
    }
}