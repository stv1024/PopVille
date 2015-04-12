using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class BuyCharacter : Proto.BuyCharacter, IUpperSentCmd
    {
        public BuyCharacter
        (
            int characterCode
        )
            : base(characterCode)
        {
        }

        public int CmdType
        {
            get { return 2031; }
        }
    }
}