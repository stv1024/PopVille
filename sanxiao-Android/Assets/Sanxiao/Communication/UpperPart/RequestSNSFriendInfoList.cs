using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestSNSFriendInfoList : IUpperSentCmd
    {
        public RequestSNSFriendInfoList() : base()
        {
        }

        public int CmdType
        {
            get { return 2311; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}