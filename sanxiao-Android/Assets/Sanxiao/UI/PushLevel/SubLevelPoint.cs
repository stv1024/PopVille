using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;
using RequestChallenge = Assets.Sanxiao.Communication.UpperPart.RequestChallenge;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 小关点
    /// </summary>
    public class SubLevelPoint : MonoBehaviour
    {
        public GameObject GrpToHideWhenUnlockEffect;

        public GameObject StandOnArrowPrefab;
        private GameObject _standOnArrow;

        public UILabel LblNum;
        public SpriteRenderer SprPoint;
        public GameObject[] Stars;

        public int SubLevelId
        {
            get { return _subLevelData.SubLevelId; }
        }

        private int _majorId;
        /// <summary>
        /// 可以为null
        /// </summary>
        private SubLevelUnlockInfo _subLevelUnlockInfo;
        private SubLevelData _subLevelData;
        public void SetAndRefresh(int majorId, SubLevelUnlockInfo subLevelUnlockInfo, SubLevelData subLevelData)
        {
            _majorId = majorId;
            _subLevelUnlockInfo = subLevelUnlockInfo;
            _subLevelData = subLevelData;
            LblNum.text = _subLevelData.SubLevelId.ToString();
            var unlocked = _subLevelUnlockInfo != null && _subLevelUnlockInfo.Unlocked;
            var starCount = _subLevelUnlockInfo != null ? _subLevelUnlockInfo.CurrentStar : 0;
            SprPoint.color = unlocked ? Color.white : Color.gray;
            for (int i = 0; i < 3; i++)
            {
                Stars[i].SetActive(i < starCount);
            }
            if (unlocked && starCount == 0)
            {
                if (!_standOnArrow) _standOnArrow = PrefabHelper.InstantiateAndReset(StandOnArrowPrefab, transform);
            }
            else
            {
                Destroy(_standOnArrow);
            }
        }

        void OnClick()
        {
            UMengPlugin.UMengEvent(EventId.PUSHLEVEL_CLICK_POINT,
                                   new Dictionary<string, object> { { "major", _majorId }, { "level", _subLevelData.SubLevelId } });

            if (_subLevelUnlockInfo == null || !_subLevelUnlockInfo.Unlocked)
            {
                MorlnFloatingToast.Create("尚未解锁");
                return;
            }
            GameData.LastChallengeMajorLevelID = _subLevelUnlockInfo.MajorLevelId;//记录点击了哪一个小关，用于从MatchUI返回到PushLevelUI
            GameData.LastChallengeSubLevelID = _subLevelUnlockInfo.SubLevelId;
            GameData.Reinforce1 = null;
            GameData.Reinforce1Portrait = null;
            GameData.Reinforce2 = null;
            GameData.Reinforce2Portrait = null;
            Requester.Instance.Send(new RequestChallenge(_subLevelUnlockInfo.MajorLevelId, _subLevelUnlockInfo.SubLevelId));
        }

        public void PlayUnlockEffect()
        {
            StartCoroutine(DelayPlayUnlockEffect());
        }

        private IEnumerator DelayPlayUnlockEffect()
        {
            yield return new WaitForSeconds(1);
            var effectPrefab = MorlnResources.Load("UI/PushLevelUI/Point_Animation", typeof (GameObject)) as GameObject;
            if (effectPrefab)
            {
                var go = PrefabHelper.InstantiateAndReset(effectPrefab, transform);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(0.5f, 0.5f, 1);

                yield return new WaitForSeconds(2);

                Destroy(go);
            }
        }
    }
}