using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 大关卡入口按钮
    /// </summary>
    public class MajorLevelButton : MonoBehaviour
    {
        private MajorLevelUnlockInfo _majorLevelUnlockInfo;

        public UILabel LblStarCount;

        public void SetAndRefresh(MajorLevelUnlockInfo majorLevelUnlockInfo)
        {
            if (majorLevelUnlockInfo == null)
            {
                Debug.LogError("刷新不能没有majorLevelUnlockInfo");
                return;
            }
            _majorLevelUnlockInfo = majorLevelUnlockInfo;


            #region 显示文本信息

            var challengeLevelConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.ChallengeLevelConfig) as ChallengeLevelConfig;
            if (challengeLevelConfig != null)
            {
                var mlc =
                    challengeLevelConfig.MajorLevelList.Find(
                        x => x.MajorLevelId == _majorLevelUnlockInfo.MajorLevelId);
                if (mlc == null)
                {
                    Debug.LogError("没有找到Major关卡文本配置:" + _majorLevelUnlockInfo.MajorLevelId);
                }
                else
                {
                    //TODO:显示文本信息

                    #region 显示玩家进程

                    var totalStar = _majorLevelUnlockInfo.SubLevelUnlockInfoList.Sum(x => x.CurrentStar);
                    
                    LblStarCount.text = string.Format("{0}/{1}", totalStar, mlc.SubLevelList.Count*3);

                    #endregion
                }
            }

            #endregion

        }

        public void OnClick()
        {
            if (_majorLevelUnlockInfo == null)
            {
                //TODO:提示何时解锁
                return;
            }
            if (PushLevelUI.Instance) PushLevelUI.Instance.EnterMajorLevel(_majorLevelUnlockInfo);
        }
    }
}