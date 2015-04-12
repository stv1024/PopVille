using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class BuyCharacterOk : Proto.BuyCharacterOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.MyUser = CurrentUser;
            if (!CommonData.MyCharacterList.Contains(NewCharacter)) CommonData.MyCharacterList.Add(NewCharacter);
            if (EquipPanel.Instance) EquipPanel.Instance.Execute(this);
            MorlnFloatingToast.Create("购买新角色成功");
        }
    }
}