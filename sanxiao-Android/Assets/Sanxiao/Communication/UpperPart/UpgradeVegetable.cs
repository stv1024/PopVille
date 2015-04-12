using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UpgradeVegetable : Proto.UpgradeVegetable, IUpperSentCmd
    {
        public int CmdType
        {
            get { return 2024; }
        }

        public UpgradeVegetable(int vegetableCode)
            : base(vegetableCode)
        {
        }
    }
}