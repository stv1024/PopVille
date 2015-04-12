using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class SpeedUpVegetableUpgrade : Proto.SpeedUpVegetableUpgrade, IUpperSentCmd
    {
        public SpeedUpVegetableUpgrade
        (
            int vegetableCode
        )
            : base(vegetableCode)
        {
        }

        public int CmdType
        {
            get { return 2041; }
        }
    }
}