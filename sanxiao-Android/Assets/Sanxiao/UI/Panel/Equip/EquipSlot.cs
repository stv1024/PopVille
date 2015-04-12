using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Scripts;
using UnityEngine;
using UseEquip = Assets.Sanxiao.Communication.UpperPart.UseEquip;

namespace Assets.Sanxiao.UI.Panel.Equip
{
    /// <summary>
    /// 一个装备按钮
    /// </summary>
    public class EquipSlot : MonoBehaviour
    {
        public EquipPanel EquipPanel;

        public UISprite SprEquip;
        public UILabel LblCount;

        private Communication.Proto.Equip _equip;
        private bool _worn;
        private int _count;
        public void SetAndRefresh(Communication.Proto.Equip equip, bool worn, int count)
        {
            _equip = equip;
            _worn = worn;
            _count = count;

            if (_equip != null)
            {
                //var lbl = GetComponentInChildren<UILabel>();
                //if (lbl) lbl.text = _equip.EquipCode.ToString() + " " + (_worn ? "√" : "×");
                LblCount.text = "×" + _count;

                SprEquip.atlas = MorlnDownloadResources.Load<UIAtlas>("ResourcesForDownload/Equip/EquipIcon/Atlas-EquipIcons");
                var sprName = EquipUtil.GetEquipSpriteName(_equip.EquipCode, _equip.Type);

                //var spr = Resources.Load<Sprite>("Sprites/Equip/" + sprName);
                //SprEquip.sprite = spr;
                SprEquip.spriteName = sprName;
                SprEquip.MakePixelPerfect();
            }
            else
            {
                LblCount.text = null;
                SprEquip.enabled = false;
            }
        }

        void OnDoubleClick()
        {
            UMengPlugin.UMengEvent(!_worn ? EventId.EQUIP_WEAR : EventId.EQUIP_UNWEAR, new Dictionary<string, object> { { "code", _equip.EquipCode } });//发送统计事件

            var userCharacter = CommonData.MyCharacterList.Find(x => x.CharacterCode == EquipPanel.CurCharacterCode);
            if (userCharacter != null)
            {
                var worn = userCharacter.WearEquipList.Exists(x => x == _equip.EquipCode);
                Requester.Instance.Send(new UseEquip(EquipPanel.CurCharacterCode, _equip.EquipCode, !worn));
            }
            MorlnTooltip.ForceHide();//竟然会触发两次OnClick，可以理解
        }
        void OnClick()
        {
            UMengPlugin.UMengEvent(EventId.EQUIP_INTRO, new Dictionary<string, object> {{"code", _equip.EquipCode}});//发送统计事件

            if (_equip == null) return;
            var equipIntroTextConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.EquipIntroTextConfig) as EquipIntroTextConfig;
            if (equipIntroTextConfig != null)
            {
                var equipIntro = equipIntroTextConfig.IntroList.Find(x => x.EquipCode == _equip.EquipCode);
                var text = equipIntro.IntroContent + (_worn ? "\n[FFFF00]双击脱下[-]" : "\n[FFFF00]双击穿上[-]");
                MorlnTooltip.Show(text, MainRoot.InverseTransformPoint(transform.position));
            }
        }
    }
}