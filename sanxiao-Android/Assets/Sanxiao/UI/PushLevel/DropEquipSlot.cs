using Assets.Sanxiao.Data;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 一个装备按钮
    /// </summary>
    public class DropEquipSlot : MonoBehaviour
    {
        public DropEquipPanel DropEquipPanel;

        public UISprite SprEquip;

        public Communication.Proto.Equip Equip;

        public void SetAndRefresh(Communication.Proto.Equip equip)
        {
            Equip = equip;
            if (Equip != null)
            {
                SprEquip.atlas = MorlnDownloadResources.Load<UIAtlas>("ResourcesForDownload/Equip/EquipIcon/Atlas-EquipIcons");
                var sprName = EquipUtil.GetEquipSpriteName(Equip.EquipCode, Equip.Type);

                SprEquip.spriteName = sprName;
            }
            else
            {
                SprEquip.atlas = MorlnResources.Load<UIAtlas>("atlases/Atlas-PushLevelUI");
                SprEquip.spriteName = "未获得装备";
            }
        }

        void OnClick()
        {
            //显示细节
            DropEquipPanel.FocusOnSlot(this);
        }
    }
}