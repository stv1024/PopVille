using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class ChangeCharacter : Proto.ChangeCharacter, IUpperSentCmd
    {
        public ChangeCharacter(int newCharacterCode) : base(newCharacterCode)
        {
        }

        public int CmdType
        {
            get { return 2211; }
        }
    }
}