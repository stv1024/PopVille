using System;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class HBReq : Proto.HBReq, IUpperReceivedCmd
    {
        public void Execute()
        {
            Requester.Instance.Send(new HBRes {SerialId = DateTime.Now.Ticks});
        }
    }
}