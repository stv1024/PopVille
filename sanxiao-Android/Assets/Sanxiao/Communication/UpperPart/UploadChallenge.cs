using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UploadChallenge : Proto.UploadChallenge, IUpperSentCmd
    {
        public int CmdType
        {
            get { return 10107; }
        }

        public UploadChallenge(string challengeId, bool isWin, DefenseData myDefenseData)
            : base(challengeId, isWin, myDefenseData)
        {
        }
    }
}