using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class ReMatch : IUpperSentCmd
    {
        public ReMatch()
        {
            
        }

        public int CmdType
        {
            get { return 10002; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}