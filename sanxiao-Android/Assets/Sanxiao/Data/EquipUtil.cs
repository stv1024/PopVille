using System.Linq;
using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Data
{
    public class EquipUtil
    {
        public static string GetEquipSpriteName(int code, int type)
        {
            var sprName = "";
            switch (type)
            {
                case 0:
                    sprName = "Helmet";
                    break;
                case 1:
                    sprName = "Armor";
                    break;
                case 2:
                    sprName = "Weapon";
                    break;
                case 3:
                    sprName = "Shield";
                    break;
                case 4:
                    sprName = "Boots";
                    break;
            }
            sprName += "1";
            return "EquipIcon" + code;
        }
        public static int GetEquipType(int code)
        {
            var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
            if (equipConfig != null)
            {
                var equip = equipConfig.EquipList.Find(x => x.EquipCode == code);
                if (equip != null) return equip.Type;
            }
            return -1;
        }
    }
}