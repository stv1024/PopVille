using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using UnityEngine;
using RequestChallenge = Assets.Sanxiao.Communication.UpperPart.RequestChallenge;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 大关卡入口按钮
    /// </summary>
    public class SubLevelButton : MonoBehaviour
    {
        private int _majorLevelID;

        private SubLevelUnlockInfo _subLevelUnlockInfo;

        public UILabel LblNumber, LblName;

        public GameObject GrpStars;
        public GameObject[] Stars;
        public GameObject SprNewStart, SprLock;

        public void SetAndRefresh(int majorLevelID, SubLevelUnlockInfo subLevelUnlockInfo)
        {
            if (subLevelUnlockInfo == null)
            {
                Debug.LogError("刷新不能没有subLevelUnlockInfo");
                return;
            }

            _majorLevelID = majorLevelID;
            _subLevelUnlockInfo = subLevelUnlockInfo;

            #region 显示星星

            if (_subLevelUnlockInfo.Unlocked)
            {
                SprLock.SetActive(false);
                if (_subLevelUnlockInfo.CurrentStar <= 0)
                {
                    GrpStars.SetActive(false);
                    SprNewStart.SetActive(true);
                }
                else
                {
                    GrpStars.SetActive(true);
                    for (int i = 0; i < Stars.Length; i++)
                    {
                        Stars[i].SetActive(i < _subLevelUnlockInfo.CurrentStar);
                    }
                    SprNewStart.SetActive(false);
                }
            }
            else
            {
                GrpStars.SetActive(false);
                SprNewStart.SetActive(false);
                SprLock.SetActive(true);
            }

            #endregion

            LblNumber.text = string.Format("{0}-{1}", _majorLevelID, _subLevelUnlockInfo.SubLevelId);
            
            var challengeLevelConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.ChallengeLevelConfig) as ChallengeLevelConfig;
            if (challengeLevelConfig != null)
            {
                var mlc =
                    challengeLevelConfig.MajorLevelList.Find(
                        x => x.MajorLevelId == _majorLevelID);
                if (mlc == null)
                {
                    Debug.LogError("没有找到Major关卡文本配置:" + _majorLevelID);
                }
                else
                {
                    var slc = mlc.SubLevelList.Find(x => x.SubLevelId == _subLevelUnlockInfo.SubLevelId);
                    if (slc == null)
                    {
                        Debug.LogError("没有找到Sub关卡文本配置:" + _majorLevelID + "," + _subLevelUnlockInfo.SubLevelId);
                    }
                    else
                    {
                        LblName.text = slc.Title;
                    }
                }
            }
            else
            {
                Debug.LogError("没有关卡配置，请检查");
            }
        }

        public void OnClick()
        {
            GameData.LastChallengeMajorLevelID = _subLevelUnlockInfo.MajorLevelId;
            GameData.LastChallengeSubLevelID = _subLevelUnlockInfo.SubLevelId;
            Requester.Instance.Send(new RequestChallenge(_majorLevelID, _subLevelUnlockInfo.SubLevelId));
        }
    }
}