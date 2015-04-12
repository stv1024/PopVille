using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class NewMatch : IUpperSentCmd
    {
        public NewMatch()
        {
            
        }

        public int CmdType
        {
            get { return 10001; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}