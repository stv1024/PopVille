using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 岛
    /// </summary>
    public class Island : MonoBehaviour
    {
        public PushLevelUI PushLevelUI;

        public int MajorLevelId { get; private set; }
        private MajorLevelUnlockInfo _majorLevelUnlockInfo;
        private MajorLevelData _majorLevelData;

        public IslandInfo IslandInfo;
        public Route Route { get { return IslandInfo.Route; } }

        public GameObject SubLevelPointTemplate;
        public Transform SubLevelPointContainer;
        public readonly List<SubLevelPoint> SubLevelPointList = new List<SubLevelPoint>();
        private GameObject _artContent;

        /// <summary>
        /// 第一次调用时创建新的岛屿内容，之后调用不会再改动。
        /// </summary>
        /// <param name="pushLevelUI"></param>
        /// <param name="majorLevelId"></param>
        /// <param name="majorLevel"></param>
        /// <param name="majorLevelData"></param>
        public void SetAndRefresh(PushLevelUI pushLevelUI, int majorLevelId, MajorLevelUnlockInfo majorLevel, MajorLevelData majorLevelData)
        {
            PushLevelUI = pushLevelUI;
            MajorLevelId = majorLevelId;
            _majorLevelUnlockInfo = majorLevel;
            _majorLevelData = majorLevelData;

            if (!_artContent)
            {
                var prefab = MorlnDownloadResources.Load<GameObject>("ResourcesForDownload/Island/Island" + (majorLevelId - 1));
                _artContent = PrefabHelper.InstantiateAndReset(prefab, transform);
                //_artContent.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                IslandInfo = _artContent.GetComponent<IslandInfo>();
                if (IslandInfo)
                {
                    IslandInfo.Island = this;
                    IslandInfo.transform.SetSortingLayer("Planet");
                }
            }
            if (!IslandInfo)
            {
                Debug.LogError("没有岛屿内容则不能玩游戏，请检查");
                return;
            }

            if (_majorLevelData == null)
            {
                Debug.LogError("没有_majorLevelData。不用显示了，玩不了！");
                return;
            }
            if (_majorLevelData.SubLevelList == null)
            {
                Debug.LogError("没有_majorLevelData.SubLevelList。不用显示了，玩不了！");
                return;
            }

            while (SubLevelPointList.Count < _majorLevelData.SubLevelList.Count)//确保 小关点 数量足够
            {
                var subLevelPoint = PrefabHelper.InstantiateAndReset<SubLevelPoint>(SubLevelPointTemplate, SubLevelPointContainer);
                subLevelPoint.name = "SubLevel " + SubLevelPointList.Count;
                SubLevelPointList.Add(subLevelPoint);
            }
            var subLevelPointPosList = Route.GetPointsUniformly(_majorLevelData.SubLevelList.Count);
            for (int subId = 1; subId <= SubLevelPointList.Count; subId++)
            {
                var pointI = subId - 1;
                var dataI = _majorLevelData != null
                                ? _majorLevelData.SubLevelList.FindIndex(x => x.SubLevelId == subId)
                                : -1;
                var unlockI = _majorLevelUnlockInfo != null
                                  ? _majorLevelUnlockInfo.SubLevelUnlockInfoList.FindIndex(x => x.SubLevelId == subId)
                                  : -1;
                if (dataI >= 0) //后者必然true，只为彻底保险
                {
                    var go = SubLevelPointList[pointI].gameObject;
                    go.SetActive(true);
                    go.transform.localPosition = subLevelPointPosList[pointI];
                    SubLevelPointList[pointI].SetAndRefresh(_majorLevelData.MajorLevelId,
                        unlockI >= 0 ? _majorLevelUnlockInfo.SubLevelUnlockInfoList[unlockI] : null,
                        dataI >= 0 ? _majorLevelData.SubLevelList[dataI] : null);//指数大于0则一定不为null，放心，别管波浪线
                }
                else
                {
                    SubLevelPointList[pointI].gameObject.SetActive(false);
                }
            }
            SubLevelPointTemplate.SetActive(false);

            var locked = _majorLevelUnlockInfo == null || !_majorLevelUnlockInfo.Unlocked;
            IslandInfo.CloudsContainer.SetActive(locked);
        }

        //void OnClick()
        //{
        //    PushLevelUI.TranslationPanel.GotoIslandView();
        //    DidGotoIslandView();
        //}

        public void DidGotoThisIslandView()
        {
            //GetComponent<UIForwardEvents>().enabled = false;
            //collider.enabled = false;
        }
        public void DidGotoPlanetView()
        {
            //GetComponent<UIForwardEvents>().enabled = true;
            //collider.enabled = true;
        }

        void OnDrag(Vector2 delta)
        {
            //Debug.Log("Island.OnDrag(" + delta);
            PushLevelUI.Planet.OnDrag(delta);
        }

        public void ShowDescription()
        {
            //MorlnFloatingToast.Create(_majorLevelData.Description);
            var levelConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.MajorLevelIntroTextConfig) as MajorLevelIntroTextConfig;
            if (levelConfig != null)
            {
                var majorLevelIntro = levelConfig.IntroList.Find(x => x.MajorLevelId == MajorLevelId);
                if (majorLevelIntro != null) PushLevelUI.DropEquipPanel.Show(majorLevelIntro);
            }
        }
    }
}