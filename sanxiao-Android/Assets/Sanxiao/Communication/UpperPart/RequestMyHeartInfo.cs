using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestMyHeartInfo : IUpperSentCmd
    {
        public int CmdType
        {
            get { return 2011; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}