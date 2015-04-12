using Assets.Sanxiao.Communication.Proto;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UpgradeSkill : Proto.UpgradeSkill, IUpperSentCmd
    {

        public UpgradeSkill
            (
            int skillcode
            ) : base(skillcode)
        {
            
        }

        public int CmdType
        {
            get { return 2021; }
        }
    }
}