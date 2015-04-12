using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestChallenge : Proto.RequestChallenge, IUpperSentCmd
    {
        public RequestChallenge(int majorLevelId, int subLevelId)
            : base(majorLevelId, subLevelId)
        {
        }

        public int CmdType
        {
            get { return 10101; }
        }
    }
}