using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RequestUserVegetable : Proto.RequestUserVegetable, IUpperSentCmd
    {
        public RequestUserVegetable(int vegetableCode) : base(vegetableCode)
        {
        }

        public int CmdType
        {
            get { return 2221; }
        }
    }
}