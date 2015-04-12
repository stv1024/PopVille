using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel.ManageSkill;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class ManageSkillPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static ManageSkillPanel _instance;

        public static ManageSkillPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 ManageSkillPanel instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/ManageSkillPanel") as GameObject;
                return go;
            }
        }

        public static void Load()
        {
            if (Instance)
            {
                MainRoot.FocusPanel(Instance);
            }
            else
            {
                if (!Prefab) return;
                MainRoot.ShowPanel(Prefab);
            }
            if (Instance) Instance.Initialize();
        }

        public static void UnloadInterface()
        {
            if (Instance) Instance.OnConfirmClick();
        }

        #endregion


        public UIGrid Grid;

        public GameObject SlotTemplate;

        SkillSlot[] _slotList;

        protected override void Initialize()
        {
            Refresh();
        }

        public void Refresh()
        {
            var skillConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.SkillConfig) as SkillConfig;
            if (skillConfig != null)
            {
                if (_slotList == null)
                {
                    _slotList = new SkillSlot[skillConfig.SkillList.Count];
                }
                else if (_slotList.Length != skillConfig.SkillList.Count)
                {
                    foreach (var slot in _slotList.Where(slot => slot != null))
                    {
                        Destroy(slot.gameObject);
                    }
                    _slotList = new SkillSlot[skillConfig.SkillList.Count];
                }
                for (var i = 0; i < skillConfig.SkillList.Count; i++)
                {
                    if (_slotList[i] == null)
                    {
                        var slot = PrefabHelper.InstantiateAndReset<SkillSlot>(SlotTemplate, Grid.transform);
                        _slotList[i] = slot;
                    }
                    _slotList[i].name = i.ToString(CultureInfo.InvariantCulture);

                    _slotList[i].SetAndRefresh(skillConfig.SkillList[i]);
                }
            }
            SlotTemplate.SetActive(false);
        }
    }
}