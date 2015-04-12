using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class EditUserInfo : Proto.EditUserInfo, IUpperSentCmd
    {
        public EditUserInfo() : base()
        {
        }

        public int CmdType
        {
            get { return 2001; }
        }
    }
}