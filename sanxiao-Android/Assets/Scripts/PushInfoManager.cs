using System;
using UnityEngine;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using BindUserPushInfo = Assets.Sanxiao.Communication.UpperPart.BindUserPushInfo;

namespace Assets.Scripts
{
    public class PushInfoManager : PushInfoListener
    {
        private static PushInfoManager _instance;
        /// <summary>
        /// 会自动创建单例
        /// </summary>
        public static PushInfoManager Instance
        {
            get { return _instance ?? (_instance = new PushInfoManager()); }
        }

        private PushInfoManager()
        {
            Push.AddPushInfoListener(this);//注册监听器，让两个接口可收到push信息，从而上传给我服务器
        }
        /// <summary>
        /// 初始化，不要频繁调用。连上我服务器时调用即可
        /// </summary>
        public void Init()
        {
            try
            {
                Push.PushInfo();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void OnBaiduPushInfo(string appId, string userId, string channelId)
        {
            Requester.Instance.Send(new BindUserPushInfo {BaiduPushInfo = new BaiduPushInfo(appId, channelId, userId)});
        }

        public void OnIOSPushInfo(string deviceToken)
        {
            Requester.Instance.Send(new BindUserPushInfo {ApnsDeviceToken = deviceToken});
        }
    }
}