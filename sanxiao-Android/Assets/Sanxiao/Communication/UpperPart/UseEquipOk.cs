using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UseEquipOk : Proto.UseEquipOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            var userCharacter = CommonData.MyCharacterList.Find(x => x.CharacterCode == CharacterCode);
            if (userCharacter == null)
            {
                Debug.LogError("ERROR。MyCharacterList缺了这个Code:" + CharacterCode);
                userCharacter = new UserCharacter(CharacterCode);
                CommonData.MyCharacterList.Add(userCharacter);
            }
            if (UseOrNot)
            {
                //卸下所有同type装备
                var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
                var curType = equipConfig.EquipList.Find(x => x.EquipCode == EquipCode).Type;
                userCharacter.WearEquipList.RemoveAll(
                    x => equipConfig.EquipList.Find(y => y.EquipCode == x).Type == curType);

                if (!userCharacter.WearEquipList.Contains(EquipCode)) userCharacter.WearEquipList.Add(EquipCode);
            }
            else
            {
                userCharacter.WearEquipList.RemoveAll(x => x == EquipCode);
            }
            if (EquipPanel.Instance) EquipPanel.Instance.Refresh();
        }
    }
}