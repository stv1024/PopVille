using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 岛屿掉落装备面板
    /// </summary>
    public class DropEquipPanel : MonoBehaviour
    {
        public Transform Focus;
        public PushLevelUI PushLevelUI;
        public UIGrid Grid;
        public GameObject EquipSlotTemplate;
        public UILabel LblEquipDescription;
        public UILabel LblCollectState;
        private readonly List<DropEquipSlot> _dropEquipSlots = new List<DropEquipSlot>();
        private int _focusIndex;
        private List<int> _equipCodes;
        readonly EquipIntroTextConfig _equipIntroTextConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipIntroTextConfig) as EquipIntroTextConfig;
        public void Refresh(List<int> equipCodes)
        {
            _equipCodes = equipCodes;
            for (int i = _dropEquipSlots.Count; i < _equipCodes.Count; i++)
            {
                var slot = PrefabHelper.InstantiateAndReset<DropEquipSlot>(EquipSlotTemplate, Grid.transform);
                slot.name = "Slot " + i;
                _dropEquipSlots.Add(slot);
            }
            while (_dropEquipSlots.Count > _equipCodes.Count)
            {
                _dropEquipSlots.RemoveRange(_equipCodes.Count, _dropEquipSlots.Count - _equipCodes.Count);
            }
            var collectedCount = 0;
            var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;

            for (int i = 0; i < _equipCodes.Count; i++)
            {
                _dropEquipSlots[i].gameObject.SetActive(true);
                var userEquip = CommonData.MyEquipList.Find(x => x.EquipCode == _equipCodes[i]);
                if (userEquip != null && userEquip.Count > 0)
                {
                    collectedCount++;
                    if (equipConfig != null)
                    {
                        var equip = equipConfig.EquipList.Find(x => _equipCodes[i] == x.EquipCode);
                        if (equip != null) _dropEquipSlots[i].SetAndRefresh(equip);
                    }
                }
                else
                {
                    _dropEquipSlots[i].SetAndRefresh(null);
                }
            }
            EquipSlotTemplate.SetActive(false);
            Grid.repositionNow = true;

            FocusOnIndex(_focusIndex);

            LblCollectState.text = string.Format("已经收集：{0} / {1}", collectedCount, _equipCodes.Count);
        }

        public void Show(MajorLevelIntro majorLevelIntro)//TODO:填入装备列表或关卡信息
        {
            gameObject.SetActive(true);
            Refresh(majorLevelIntro.HiddenEquipList);
        }

        public void FocusOnIndex(int index)
        {
            if (_equipCodes.Count > 0)
            {
                _focusIndex = Mathf.Clamp(index, 0, _equipCodes.Count - 1);
                FocusOnSlot(_dropEquipSlots[_focusIndex]);
            }
            else
            {
                FocusOnSlot(null);
            }
        }
        public void FocusOnSlot(DropEquipSlot slot)
        {
            if (slot)
            {
                Focus.gameObject.SetActive(true);
                Focus.position = slot.transform.position;
                if (_equipIntroTextConfig != null)
                {
                    var equipIntro = _equipIntroTextConfig.IntroList.Find(x => x.EquipCode == slot.Equip.EquipCode);
                    LblEquipDescription.text = equipIntro != null ? equipIntro.IntroContent : null;
                }
            }
            else
            {
                Focus.gameObject.SetActive(false);
                LblEquipDescription.text = null;
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}