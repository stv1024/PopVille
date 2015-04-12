namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestNickname : IUpperSentCmd
    {
        public int CmdType
        {
            get { return 2003; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}