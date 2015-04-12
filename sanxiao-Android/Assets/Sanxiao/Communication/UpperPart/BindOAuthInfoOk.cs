using System;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class BindOAuthInfoOk : Proto.BindOAuthInfoOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.SnsData.SinaWeibo.Uid = Uid;
            CommonData.SnsData.SinaWeibo.AccessToken = AccessToken;
            CommonData.SnsData.SinaWeibo.ExpireOn = DateTime.Now + TimeSpan.FromSeconds(ExpireTime);

            if (BindSnsPanel.Instance) BindSnsPanel.Instance.Execute(this);
            if (ReinforcePanel.Instance) ReinforcePanel.Instance.Execute(this);
        }
    }
}