using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class SyncDataResponse : Proto.SyncDataResponse, IUpperSentCmd
    {
        private SyncDataResponse(){}

        public SyncDataResponse(int myenergy)
            : base(myenergy)
        {

        }

        public int CmdType
        {
            get { return 10024; }
        }
    }
}