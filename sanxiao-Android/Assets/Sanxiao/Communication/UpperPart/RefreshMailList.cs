using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RefreshMailList : Proto.RefreshMailList, IUpperSentCmd
    {
        public RefreshMailList(int readState) : base(readState)
        {
        }

        public int CmdType
        {
            get { return 2231; }
        }
    }
}