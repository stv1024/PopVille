using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class HBRes : Proto.HBRes, IUpperSentCmd
    {
        public HBRes() : base()
        {
        }

        public int CmdType
        {
            get { return 1; }
        }
    }
}