using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class BindOAuthInfo : Proto.BindOAuthInfo, IUpperSentCmd
    {
        public BindOAuthInfo
        (
            int type,
            string authorizeCode,
            string deviceId
        )
            : base(type, authorizeCode, deviceId)
        {
        }

        public int CmdType
        {
            get { return 2302; }
        }
    }
}