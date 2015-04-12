using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestLeaderBoard : Proto.RequestLeaderBoard, IUpperSentCmd
    {
        public RequestLeaderBoard(int type)
            : base(type)
        {
        }

        public int CmdType
        {
            get { return 10201; }
        }
    }
}