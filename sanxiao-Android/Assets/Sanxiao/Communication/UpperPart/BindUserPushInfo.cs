using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class BindUserPushInfo : Proto.BindUserPushInfo, IUpperSentCmd
    {
        public BindUserPushInfo() : base()
        {
        }

        public int CmdType
        {
            get { return 2305; }
        }
    }
}