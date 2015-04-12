using ProtoBuffer;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public interface IUpperReceivedCmd : IReceiveable
    {
        void Execute();
    }
}