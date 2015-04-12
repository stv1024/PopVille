using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UploadChallengeOk : Proto.UploadChallengeOk, IUpperReceivedCmd
    {
        public void Execute()
        {
            //更新关卡解锁信息
            if (HasUnlockElement)
            {
                foreach (var mluInfo in UnlockElement.MajorLevelUnlockList)
                {
                    Debug.LogWarning("mluInfo:"+mluInfo.MajorLevelId);
                    var index = CommonData.ChallengeUnlockInfoList.FindIndex(x => x.MajorLevelId == mluInfo.MajorLevelId);
                    if (index >= 0)
                    {
                        if (!CommonData.ChallengeUnlockInfoList[index].Unlocked && mluInfo.Unlocked)
                        {
                            CommonData.JustUnlockedMajorLevelId = mluInfo.MajorLevelId;
                            if (mluInfo.SubLevelUnlockInfoList.Count > 0 && mluInfo.SubLevelUnlockInfoList[0].Unlocked &&
                                mluInfo.SubLevelUnlockInfoList[0].CurrentStar == 0)//第一个小关解锁效果
                            {
                                CommonData.JustUnlockedSubLevelId = new IntVector2(mluInfo.MajorLevelId,
                                                                                   mluInfo.SubLevelUnlockInfoList[0]
                                                                                       .SubLevelId);
                            }
                        }
                        CommonData.ChallengeUnlockInfoList[index] = mluInfo;
                    }
                    else
                    {
                        if (mluInfo.Unlocked)
                        {
                            CommonData.JustUnlockedMajorLevelId = mluInfo.MajorLevelId;
                            if (mluInfo.SubLevelUnlockInfoList.Count > 0 && mluInfo.SubLevelUnlockInfoList[0].Unlocked &&
                                mluInfo.SubLevelUnlockInfoList[0].CurrentStar == 0)//第一个小关解锁效果
                            {
                                CommonData.JustUnlockedSubLevelId = new IntVector2(mluInfo.MajorLevelId,
                                                                                   mluInfo.SubLevelUnlockInfoList[0]
                                                                                       .SubLevelId);
                            }
                        }
                        CommonData.ChallengeUnlockInfoList.Add(mluInfo);
                    }
                }
                foreach (var subLevelUnlockInfo in UnlockElement.SubLevelUnlockList)
                {
                    var mluInfo = CommonData.ChallengeUnlockInfoList.Find(x => x.MajorLevelId == subLevelUnlockInfo.MajorLevelId);
                    if (mluInfo != null)
                    {
                        var index =
                            mluInfo.SubLevelUnlockInfoList.FindIndex(x => x.SubLevelId == subLevelUnlockInfo.SubLevelId);
                        if (index >= 0)
                        {
                            if (!mluInfo.SubLevelUnlockInfoList[index].Unlocked && subLevelUnlockInfo.Unlocked)
                            {
                                CommonData.JustUnlockedSubLevelId = new IntVector2(mluInfo.MajorLevelId, subLevelUnlockInfo.SubLevelId);
                            }
                            mluInfo.SubLevelUnlockInfoList[index] = subLevelUnlockInfo;
                        }
                        else
                        {
                            if (subLevelUnlockInfo.Unlocked)
                            {
                                CommonData.JustUnlockedSubLevelId = new IntVector2(mluInfo.MajorLevelId, subLevelUnlockInfo.SubLevelId);
                            }
                            mluInfo.SubLevelUnlockInfoList.Add(subLevelUnlockInfo);
                        }
                    }
                    else
                    {
                        Debug.LogError("找不到MajorLevelUnlockInfo. MajorID:" + subLevelUnlockInfo.MajorLevelId);
                    }
                }
            }

            //设置当前关卡的星数
            if (GameData.LastChallengeID == ChallengeId)
            {
                var mul = CommonData.ChallengeUnlockInfoList.Find(x => x.MajorLevelId == GameData.LastChallengeMajorLevelID);
                if (mul != null)
                {
                    var sul = mul.SubLevelUnlockInfoList.Find(x => x.SubLevelId == GameData.LastChallengeSubLevelID);
                    if (sul != null)
                    {
                        sul.CurrentStar = Mathf.Max(sul.CurrentStar, StarCount);
                    }
                }
            }

            MainController.Instance.Execute(this);
        }
    }
}