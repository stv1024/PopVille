using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class ExchangeOk : Proto.ExchangeOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            MorlnTooltip.ShowCentered("兑换成功\nName:" + Name + "\nCount:" + Count);
            CommonData.MyUser = User;
        }
    }
}