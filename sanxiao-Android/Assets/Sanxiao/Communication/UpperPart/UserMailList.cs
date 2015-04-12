using Assets.Sanxiao.UI;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UserMailList : Proto.UserMailList, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.MailList = MailList;
            if (MailBoxPanel.Instance) MailBoxPanel.Instance.Execute(this);
            if (MenuUI.Instance) MenuUI.Instance.Execute(this);
        }
    }
}