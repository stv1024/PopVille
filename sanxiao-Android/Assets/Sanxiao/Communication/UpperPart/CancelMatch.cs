using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class CancelMatch : IUpperSentCmd
    {
        public CancelMatch()
        {
        }

        public int CmdType
        {
            get { return 10005; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}