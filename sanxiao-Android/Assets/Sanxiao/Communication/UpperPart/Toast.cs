namespace Assets.Sanxiao.Communication.UpperPart
{
    public class Toast : Proto.Toast, IUpperReceivedCmd
    {
        public void Execute()
        {
            switch (Type)
            {
                case 0:
                    MorlnFloatingToast.Create(Content);
                    break;
                    //TODO:其他弹出消息
            }
        }
    }
}