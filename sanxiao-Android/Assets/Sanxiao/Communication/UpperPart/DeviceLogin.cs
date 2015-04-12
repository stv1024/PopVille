using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class DeviceLogin : Proto.DeviceLogin, IUpperSentCmd
    {
        private DeviceLogin(){}
        public DeviceLogin
            (
            string deviceuid,
            ClientInfo clientinfo
            )
            : base
                (
                deviceuid,
                clientinfo
                )
        {
        }

        public int CmdType
        {
            get { return 1001; }
        }
    }
}