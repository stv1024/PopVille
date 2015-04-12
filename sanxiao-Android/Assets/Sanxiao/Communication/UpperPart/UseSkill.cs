using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UseSkill : Proto.UseSkill, IUpperSentCmd
    {
        public UseSkill
            (
            int skillcode,
            int physicaldamage
            )
            : base(skillcode, physicaldamage)
        {
        }

        public int CmdType
        {
            get { return 10031; }
        }
    }
}