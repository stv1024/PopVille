using Assets.Sanxiao.Communication.Proto;
using ProtoBuffer;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public interface IUpperSentCmd : ISendable
    {
        int CmdType { get; }
    }
}