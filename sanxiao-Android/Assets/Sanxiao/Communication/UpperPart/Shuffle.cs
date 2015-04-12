using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class Shuffle : IUpperSentCmd
    {
        public Shuffle()
        {
        }

        public int CmdType
        {
            get { return (-1); }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}