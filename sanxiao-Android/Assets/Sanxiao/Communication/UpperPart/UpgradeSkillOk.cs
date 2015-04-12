using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UpgradeSkillOk : Proto.UpgradeSkillOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (CurrentSkill != null)
            {
                var index = CommonData.MySkillList.FindIndex(x => x.Code == CurrentSkill.Code);
                if (index < 0)
                {
                    CommonData.MySkillList.Add(CurrentSkill);
                }
                else
                {
                    CommonData.MySkillList[index] = CurrentSkill;
                }
                if (ManageSkillPanel.Instance) ManageSkillPanel.Instance.Refresh();
            }
            if (CurrentUser != null)
            {
                CommonData.MyUser = CurrentUser;
            }
        }
    }
}