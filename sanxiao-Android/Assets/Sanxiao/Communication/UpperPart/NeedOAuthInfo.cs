namespace Assets.Sanxiao.Communication.UpperPart
{
    /// <summary>
    /// NONE = -1;
    /// SINA_WEIBO = 0;public static final int 
    /// QQ_WEIBO = 1;public static final int 
    /// QIHU_360 = 101;public static final int 
    /// XIAOMI = 102;
    /// BAIDU = 103;
    /// </summary>
    public class NeedOAuthInfo : Proto.NeedOAuthInfo, IUpperReceivedCmd
    {
        public void Execute()
        {
            MainController.Instance.Execute(this);
        }
    }
}