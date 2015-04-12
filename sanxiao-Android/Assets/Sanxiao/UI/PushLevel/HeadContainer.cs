using System.Collections.Generic;
using System.Linq;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 推图界面的头像容器
    /// </summary>
    public class HeadContainer : MonoBehaviour
    {
        public GameObject HeadTemplate;
        
        public void Reset()
        {
            if (CommonData.SnsFriendUnlockInfoList == null) return;

            var planet = PushLevelUI.Instance.Planet;

            var islandList =
                planet.MajorLevelSectorList.Select(x => x.GetComponentInChildren<Island>()).Where(x => x).ToList();

            Heads.Clear();
            HeadsOnCenter.Clear();
            HeadsOnSide.Clear();

            foreach (var snsFriendUnlockInfo in CommonData.SnsFriendUnlockInfoList)
            {
                var island = islandList.Find(x => x.MajorLevelId == snsFriendUnlockInfo.UnlockedMajorLevelId);
                if (island != null)
                {
                    var subLevelPoint = island.SubLevelPointList.Find(x => x.SubLevelId == snsFriendUnlockInfo.UnlockedSubLevelId);
                    if (subLevelPoint != null)
                    {
                        var head = PrefabHelper.InstantiateAndReset<Head>(HeadTemplate, transform);
                        head.gameObject.SetActive(true);
                        head.name = "Head:" + snsFriendUnlockInfo.Nickname;
                        head.SetAndRefresh(snsFriendUnlockInfo, subLevelPoint.transform);
                        var panel = head.GetComponent<UIPanel>();
                        panel.alpha = 1;
                        panel.depth = Heads.Count;
                        Heads.Add(head);
                        HeadsOnSide.Add(head);
                    }
                }
            }

            HeadTemplate.SetActive(false);
        }

        public readonly List<Head> Heads = new List<Head>();
        public readonly List<Head> HeadsOnCenter = new List<Head>();
        public readonly List<Head> HeadsOnSide = new List<Head>();
        readonly List<Head> _headsOnLeftSide = new List<Head>();
        readonly List<Head> _headsOnRightSide = new List<Head>();

        void Update()
        {
            if (!PushLevelUI.Instance) return;
            var planet = PushLevelUI.Instance.Planet;
            var curVisualIndexOfPlanet = planet.TotalAngle/Planet.OffsetAngle;
            //先确定每个head应该在哪个列表里
            for (var i = 0; i < HeadsOnCenter.Count;)
            {
                var head = HeadsOnCenter[i];
                if (Mathf.Abs(head.MajorId-1 - curVisualIndexOfPlanet) >= 1)
                {
                    HeadsOnCenter.RemoveAt(i);
                    HeadsOnSide.Add(head);
                    continue;
                }
                var stdX = transform.InverseTransformPoint(head.FollowTarget.position).x;
                if (Mathf.Abs(stdX) >= MainRoot.StdCameraRange.x/2 - 30)
                {
                    HeadsOnCenter.RemoveAt(i);
                    HeadsOnSide.Add(head);
                    continue;
                }
                i++;
            }
            for (var i = 0; i < HeadsOnSide.Count; )
            {
                var head = HeadsOnSide[i];
                var stdX = transform.InverseTransformPoint(head.FollowTarget.position).x;
                if (Mathf.Abs(head.MajorId-1 - curVisualIndexOfPlanet) < 1 && Mathf.Abs(stdX) < MainRoot.StdCameraRange.x / 2 - 30)
                {
                    HeadsOnSide.RemoveAt(i);
                    HeadsOnCenter.Add(head);
                    continue;
                }
                i++;
            }

            _headsOnLeftSide.Clear();
            _headsOnRightSide.Clear();
            var leftCount = 0;
            var rightCount = 0;
            foreach (var head in HeadsOnSide)
            {
                if (head.FollowTarget.position.x < 0)
                {
                    _headsOnLeftSide.Add(head);
                    var tarPos = new Vector3(-MainRoot.StdCameraRange.x/2 + 30, 200 + leftCount*20);
                    head.SpringPosition.target = tarPos;
                    leftCount++;
                }
                else
                {
                    _headsOnRightSide.Add(head);
                    var tarPos = new Vector3(MainRoot.StdCameraRange.x/2 - 30, 200 + rightCount*20);
                    head.SpringPosition.target = tarPos;
                    rightCount++;
                }
                head.SpringPosition.enabled = true;
            }
            foreach (var head in HeadsOnCenter)
            {
                var tarPos = transform.InverseTransformPoint(head.FollowTarget.position).SetV3Z(0).AddV3Y(40);
                head.SpringPosition.target = tarPos;
                head.SpringPosition.enabled = true;
            }
        }
    }
}