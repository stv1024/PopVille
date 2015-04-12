using System.Linq;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class LoginOk : Proto.LoginOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            CommonData.MyUser = MyUserInfo;

            CommonData.MySkillList = MySkillList;

            CommonData.MyVegetableList = MyVegetableList;
            CommonData.MyVegetableMatureInfoList.Clear();
            foreach (var userVegetable in CommonData.MyVegetableList.Where(userVegetable => userVegetable.HasMatureTime))
            {
                CommonData.MyVegetableMatureInfoList.Add(
                    new CommonData.VegetableMatureInfo(userVegetable.VegetableCode,
                                                       Time.realtimeSinceStartup + userVegetable.MatureTime*0.001f));
            }

            CommonData.ChallengeUnlockInfoList = ChallengeUnlockInfoList;

            CommonData.MyCharacterList = MyCharacterList;

            CommonData.MyEquipList = MyEquipList;

            CommonData.SnsFriendUnlockInfoList = SnsFriendUnlockInfoList;

            MainController.Instance.Execute(this);
        }
    }
}