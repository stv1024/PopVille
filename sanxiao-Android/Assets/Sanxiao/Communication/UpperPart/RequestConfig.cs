using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    /// <summary>
    /// 各种配置的hash值列表。
    /// 客户端登陆的时候发给客户端。
    /// 如果客户端发现某个配置的hash值与缓存的hash值不一致，则请求下载该配置。
    /// 0:全局配置。
    /// 1:充值包的配置。
    /// 2:技能数据。
    /// 3:技能配置。
    /// 4:兑换包配置。
    /// 5:蔬菜配置。
    /// 6:推图关卡配置。
    /// 7:技能简介的文本。
    /// 8:技能等级详情文本。
    /// 9:蔬菜介绍的文本。
    /// 10:等待的时候显示的提示文本。
    /// </summary>
    public class RequestConfig : Proto.RequestConfig, IUpperSentCmd
    {
        public int CmdType { get { return 1101; } }
    }
}