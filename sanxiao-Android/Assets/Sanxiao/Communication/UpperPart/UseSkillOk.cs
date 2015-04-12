using Assets.Sanxiao.Game;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UseSkillOk : Proto.UseSkillOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            GameManager.Instance.Execute(this);
        }
    }
}