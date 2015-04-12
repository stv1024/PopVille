using Assets.Sanxiao.UI;
using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class ChangeCharacterOk : Proto.ChangeCharacterOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.MyUser.CharacterCode = NewCharacterCode;

            //一堆刷新
            if (EquipPanel.Instance) EquipPanel.Instance.Refresh();
            if (MenuUI.Instance) MenuUI.Instance.RefreshMyInfo();
        }
    }
}