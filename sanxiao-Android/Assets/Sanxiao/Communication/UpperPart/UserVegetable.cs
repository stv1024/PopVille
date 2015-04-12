using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UserVegetable : Proto.UserVegetable, IUpperReceivedCmd
    {
        public void Execute()
        {
            var index = CommonData.MyVegetableList.FindIndex(x => x.VegetableCode == VegetableCode);
            var upgraded = false;
            if (index >= 0)
            {
                if (CurrentLevel > CommonData.MyVegetableList[index].CurrentLevel) upgraded = true;
                CommonData.MyVegetableList[index] = this;
            }
            else CommonData.MyVegetableList.Add(this);

            CommonData.MyVegetableMatureInfoList.RemoveAll(x => x.Code == VegetableCode);

            if (GardenPanel.Instance)
            {
                GardenPanel.Instance.Refresh();
                if (upgraded) GardenPanel.Instance.ShowVegetableUpgradeEffect(VegetableCode);
            }
        }
    }
}