using System.Linq;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestMailGiftOK : Proto.RequestMailGiftOK, IUpperReceivedCmd
    {
        public void Execute()
        {
            var mail = CommonData.MailList.Find(x => x.MailId == MailId);
            if (mail != null)
            {
                foreach (var gift in mail.GiftList.Where(gift => SnList.Contains(gift.Sn)))
                {
                    gift.IsObtained = true;
                }
            }
            CommonData.MyUser = User;
        }
    }
}