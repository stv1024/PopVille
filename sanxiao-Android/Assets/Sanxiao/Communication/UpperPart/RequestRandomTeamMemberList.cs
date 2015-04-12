using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestRandomTeamMemberList : IUpperSentCmd
    {
        public RequestRandomTeamMemberList()
        {
        }

        public int CmdType
        {
            get { return 2321; }
        }

        public byte[] GetProtoBufferBytes()
        {
            return null;
        }
    }
}