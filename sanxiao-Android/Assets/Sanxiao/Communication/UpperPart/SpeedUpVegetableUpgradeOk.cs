using Assets.Sanxiao.UI.Panel;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class SpeedUpVegetableUpgradeOk : Proto.SpeedUpVegetableUpgradeOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            var ind = CommonData.MyVegetableList.FindIndex(x => x.VegetableCode == CurrentVegetable.VegetableCode);
            if (ind >= 0) CommonData.MyVegetableList[ind] = CurrentVegetable;
            else CommonData.MyVegetableList.Add(CurrentVegetable);
            ind = CommonData.MyVegetableMatureInfoList.FindIndex(x => x.Code == CurrentVegetable.VegetableCode);
            if (ind >= 0) CommonData.MyVegetableMatureInfoList.RemoveAt(ind);

            if (GardenPanel.Instance) GardenPanel.Instance.Execute(this);
        }
    }
}