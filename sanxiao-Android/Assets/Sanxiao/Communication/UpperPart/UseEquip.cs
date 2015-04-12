using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UseEquip : Proto.UseEquip, IUpperSentCmd
    {
        public UseEquip(int characterCode, int equipCode, bool useOrNot)
            : base(characterCode, equipCode, useOrNot)
        {
        }

        public int CmdType
        {
            get { return 2201; }
        }
    }
}