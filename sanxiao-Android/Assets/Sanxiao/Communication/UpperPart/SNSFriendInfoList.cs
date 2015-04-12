namespace Assets.Sanxiao.Communication.UpperPart
{
    public class SNSFriendInfoList : Proto.SNSFriendInfoList, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.FriendData.FriendList = FriendList;
        }
    }
}