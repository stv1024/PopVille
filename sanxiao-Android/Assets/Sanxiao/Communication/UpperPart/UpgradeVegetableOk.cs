using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UpgradeVegetableOk : Proto.UpgradeVegetableOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.MyUser = CurrentUser;
            var index = CommonData.MyVegetableList.FindIndex(x => x.VegetableCode == CurrentVegetable.VegetableCode);
            if (index < 0)
            {
                CommonData.MyVegetableList.Add(CurrentVegetable);
            }
            else
            {
                CommonData.MyVegetableList[index] = CurrentVegetable;
            }
            if (CurrentVegetable.HasMatureTime)
            {
                CommonData.MyVegetableMatureInfoList.RemoveAll(x => x.Code == CurrentVegetable.VegetableCode);
                CommonData.MyVegetableMatureInfoList.Add(
                    new CommonData.VegetableMatureInfo(CurrentVegetable.VegetableCode,
                                                       Time.realtimeSinceStartup + CurrentVegetable.MatureTime * 0.001f));
            }

            if (GardenPanel.Instance != null) GardenPanel.Instance.Execute(this);
        }
    }
}