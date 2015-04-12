using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class RandomTeamMemberList : Proto.RandomTeamMemberList, IUpperReceivedCmd
    {
        public void Execute()
        {
            GameData.RandomTeamMemberList = TeamMemberList;

            if (ReinforcePanel.Instance) ReinforcePanel.Instance.Execute(this);
        }
    }
}