using UnityEngine;

namespace Assets.Sanxiao.Communication.UpperPart
{
    public class UserHeartInfo : Proto.UserHeartInfo, IUpperReceivedCmd
    {
        public void Execute()
        {
            if (Count > CommonData.HeartData.Count)
            {
                //TODO:增长爱心特效
            }

            CommonData.HeartData.Count = Count;
            CommonData.HeartData.NextHeartRealTime = Time.realtimeSinceStartup + NextNeedTime*0.001f;

            if (CommonData.HeartData.Count < CommonData.HeartData.MaxCount)
            {
                MainController.Instance.WaitingCheckHeart(NextNeedTime * 0.001f);
            }
            else
            {
                MainController.Instance.ClearCheckHeartInvoke();
            }
        }
    }
}