using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.UI.Panel;
using Assets.Sanxiao.UI.PushLevel;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI
{
    public class PushLevelUI : BaseUI
    {
        #region 单例UI通用

        private static PushLevelUI _instance;

        public static PushLevelUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 PushLevelUI instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        protected override void Release()
        {

        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/PushLevelUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static PushLevelUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<PushLevelUI>(prefab, MainRoot.UIParent);//创建并成为单例
            Instance.OnStage();
            return Instance;
        }

        public override void OffStage()
        {
            Instance = null;//成为孤立
            base.OffStage();
        }

        public static void ExitStage()
        {
            if (Instance)
            {
                Instance.OffStage();
            }
        }

        #endregion

        public TranslationPanel TranslationPanel;

        public HeadContainer HeadContainer;

        private void Awake()
        {
            BtnGoLeft.defaultColor = Color.white;
            BtnGoRight.defaultColor = Color.white;
            BtnGoLeft.UpdateColor(false, true);
            BtnGoRight.UpdateColor(false, true);
            Refresh();
        }

        void Start()
        {
            HeadContainer.Reset();
        }

        #region 选大关

        public Planet Planet;

        public void Refresh()
        {
            Planet.Refresh();
            TranslationPanel.GotoPlanetView();
        }

        public void OnReturnClick()
        {
            MainRoot.Goto(MainRoot.UIStateName.Menu);
        }

        public void OnShopClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_SHOP, null);

            ShopPanel.Load();
            if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.Recharge);
        }
        public void OnEquipClick()
        {
            EquipPanel.Load();
        }
        public void OnGardenClick()
        {
            GardenPanel.Load();
        }
        public void OnSkillClick()
        {
            ManageSkillPanel.Load();
        }
        #endregion

        #region 切换大小关状态

        //public GameObject TweenParent;
        public void EnterMajorLevel(MajorLevelUnlockInfo majorLevelUnlockInfo)
        {
            //Debug.LogWarning("EML:");
            //Planet.CurIndex = majorLevelUnlockInfo.MajorLevelId - 1;
            //Planet.TotalAngle = Planet.CurIndex*Planet.OffsetAngle;
            StartCoroutine(_EnterMajorLevel(majorLevelUnlockInfo));
        }
        IEnumerator _EnterMajorLevel(MajorLevelUnlockInfo majorLevelUnlockInfo)
        {
            yield return new WaitForSeconds(0.5f);
            Planet.GoToIndex(majorLevelUnlockInfo.MajorLevelId - 1);

            var island = Planet.MajorLevelSectorList[majorLevelUnlockInfo.MajorLevelId - 1].GetComponentInChildren<Island>();
            if (CommonData.JustUnlockedMajorLevelId == majorLevelUnlockInfo.MajorLevelId)
            {
                island.IslandInfo.CloudsContainer.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                island.IslandInfo.PlayUnlockEffect();
                CommonData.JustUnlockedMajorLevelId = null;
            }
            else
            {
                TranslationPanel.GotoIslandView(Vector2.zero);//TODO:移动到新解锁小关上面
                island.DidGotoThisIslandView();
                if (CommonData.JustUnlockedSubLevelId != null &&
                    CommonData.JustUnlockedSubLevelId.Value.i == majorLevelUnlockInfo.MajorLevelId)
                {
                    var subInd = CommonData.JustUnlockedSubLevelId.Value.j - 1;
                    if (0 <= subInd && subInd < island.SubLevelPointList.Count)
                    {
                        yield return new WaitForSeconds(0.5f);
                        island.SubLevelPointList[subInd].GetComponent<SubLevelPoint>().PlayUnlockEffect();
                    }
                    CommonData.JustUnlockedSubLevelId = null;
                }
            }
        }

        public void ExitMajorLevel()
        {
            TranslationPanel.GotoPlanetView();
        }
        #endregion

        #region 选小关

        private MajorLevelUnlockInfo _majorLevelUnlockInfo;

        public SubLevelButton SubLevelButtonTemplate;

        public UIGrid SubGrid;

        private readonly List<SubLevelButton> _subLevelButtonList = new List<SubLevelButton>();

        public void SetAndRefreshSub(MajorLevelUnlockInfo majorLevelUnlockInfo)
        {
            _majorLevelUnlockInfo = majorLevelUnlockInfo;

            while (_subLevelButtonList.Count < _majorLevelUnlockInfo.SubLevelUnlockInfoList.Count)
            {
                _subLevelButtonList.Add(null);
            }
            for (int i = 0; i < _majorLevelUnlockInfo.SubLevelUnlockInfoList.Count; i++)
            {
                if (_subLevelButtonList[i] == null)
                {
                    _subLevelButtonList[i] = PrefabHelper.InstantiateAndReset<SubLevelButton>(
                        SubLevelButtonTemplate.gameObject, SubGrid.transform);
                    _subLevelButtonList[i].name = "SubLevelButton " + i;
                    _subLevelButtonList[i].gameObject.SetActive(true);
                }
                _subLevelButtonList[i].SetAndRefresh(_majorLevelUnlockInfo.MajorLevelId, _majorLevelUnlockInfo.SubLevelUnlockInfoList[i]);
            }
            SubGrid.repositionNow = true;

            SubLevelButtonTemplate.gameObject.SetActive(false);
        }

        public void OnSubReturnClick()
        {
            ExitMajorLevel();
        }

        #endregion

        public UIButton BtnGoLeft, BtnGoRight;
        public UILabel LblMajorLevelName;

        #region 掉落装备面板

        public DropEquipPanel DropEquipPanel;
        
        #endregion

        public override bool OnEscapeClick()
        {
            TranslationPanel.OnReturnClick();
            return true;
        }
    }
}