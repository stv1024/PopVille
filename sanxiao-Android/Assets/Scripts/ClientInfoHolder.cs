using Assets.Sanxiao.Communication.Proto;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// 客户端信息载体，放在Resources/下，独立于场景，更方便更改
    /// </summary>
    public class ClientInfoHolder : MonoBehaviour
    {
        public string IP = "112.124.55.102";
        public int ClientVersion;
        public SaleChannelEnum SaleChannel;
        public string SoftwareVersion;

        private static ClientInfoHolder _instance;

        public static ClientInfoHolder Instance
        {
            get { return _instance ?? (_instance = Resources.Load<ClientInfoHolder>("Data/ClientInfo Holder")); }
        }

        public static ClientInfo GetClientInfo()
        {
#if UNITY_ANDROID
            var os = "Android";
#elif UNITY_IPHONE
            var os = "iOS";
#else
            var os = "Standalone";
#endif
            return new ClientInfo(Instance.ClientVersion, Instance.SaleChannel.ToString(), os);
        }
    }
}