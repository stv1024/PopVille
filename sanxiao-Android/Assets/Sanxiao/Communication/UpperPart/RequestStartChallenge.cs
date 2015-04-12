using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestStartChallenge : Proto.RequestStartChallenge, IUpperSentCmd
    {
        public RequestStartChallenge(string challengeId) : base(challengeId)
        {
        }

        public int CmdType
        {
            get { return 10104; }
        }
    }
}