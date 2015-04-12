using System.Linq;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.UI.Panel.Garden;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class GardenPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static GardenPanel _instance;

        public static GardenPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 GardenPanel instance now!");
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
                var go = Resources.Load("UI/GardenPanel") as GameObject;
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

        VegetableSlot[] _slotList;

        protected override void Initialize()
        {
            Refresh();
        }
        public void Refresh()
        {
            if (_slotList == null)
            {
                _slotList = new VegetableSlot[CommonData.MyVegetableList.Count];
            }
            else if (_slotList.Length != CommonData.MyVegetableList.Count)
            {
                foreach (var slot in _slotList.Where(slot => slot != null))
                {
                    Destroy(slot.gameObject);
                }
                _slotList = new VegetableSlot[CommonData.MyVegetableList.Count];
            }
            for (int i = 0; i < CommonData.MyVegetableList.Count; i++)
            {
                if (_slotList[i] == null)
                {
                    var slot = PrefabHelper.InstantiateAndReset<VegetableSlot>(SlotTemplate, Grid.transform);
                    slot.gameObject.SetActive(true);
                    _slotList[i] = slot;
                }
                _slotList[i].name = i.ToString();
                _slotList[i].SetAndRefresh(CommonData.MyVegetableList[i]);
            }
            SlotTemplate.SetActive(false);
            Grid.repositionNow = true;
        }

        public void Execute(UpgradeVegetableOk cmd)
        {
            Refresh();
        }
        public void Execute(UserVegetable cmd)
        {
            Refresh();
            ShowVegetableUpgradeEffect(cmd.VegetableCode);
        }
        public void Execute(SpeedUpVegetableUpgradeOk cmd)
        {
            Refresh();
            ShowVegetableUpgradeEffect(cmd.CurrentVegetable.VegetableCode);
        }

        public GameObject VegetableUpgradeEffectTemplate;
        public void ShowVegetableUpgradeEffect(int code)
        {
            var index = CommonData.MyVegetableList.FindIndex(x => x.VegetableCode == code);
            var localPos = transform.InverseTransformPoint(_slotList[index].transform.position);
            var go = PrefabHelper.InstantiateAndReset(VegetableUpgradeEffectTemplate, transform);
            go.transform.localPosition = localPos;
            go.SetActive(true);
            Destroy(go, 2);

            _slotList[index].PlayUpgradeEffect();
        }

    }
}