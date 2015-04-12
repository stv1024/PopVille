using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class ExchangeFail : Proto.ExchangeFail, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (Result.HasMsg)
            {
                MorlnFloatingToast.Create(Result.Msg);
            }
        }
    }
}