
using System.Collections.Generic;
using SimpleJson;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// 远程推送。
/// 安卓推送和IOS推送。
/// 该脚本必须放在物体AndroidReceiver和IOSReceiver上面
/// </summary>
public class Push : MonoBehaviour
{

#if UNITY_IPHONE
     [DllImport("__Internal")]
    private static extern void _pushInfo();
#endif

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void PushInfo()
    {

#if UNITY_ANDROID
        AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("pushInfo");
#elif UNITY_IPHONE
       _pushInfo();
#endif
    }

    /// <summary>
    /// json:
    /// {"appId":"userId","":"channelId","":""}
    /// </summary>
    /// <param name="json"></param>
    public void PushInfoResult(string json)
    {
        Debug.Log("PushInfoResult:"+json);

        try
        {
            JsonNode root = JsonNode.FromJson(json);
            var appIdNode = root["appId"];
            var deviceTokenNode = root["deviceToken"];

            if (appIdNode != null)
            {
                string appId = root["appId"];
                string userId = root["userId"];
                string channelId = root["channelId"];
                foreach (PushInfoListener listener in Listeners)
                {

                    listener.OnBaiduPushInfo(appId, userId, channelId);
                }


            }

            if (deviceTokenNode != null)
            {
                string deviceToken = root["deviceToken"];
                foreach (PushInfoListener listener in Listeners)
                {

                    listener.OnIOSPushInfo(deviceToken);
                }
            }




        }
        catch (SimpleJson.SimpleJsonException e)
        {
            
            Debug.Log(e.Message);
        }
         
    }

    private static readonly List<PushInfoListener> Listeners = new List<PushInfoListener>();

    public static void AddPushInfoListener(PushInfoListener listener)
    {
        if (Listeners.Contains(listener))
        {

        }
        else
        {
            Listeners.Add(listener);
        }
    }
    public static void RemovePushInfoListener(PushInfoListener listener)
    {
        bool find = false;
        foreach (PushInfoListener authListener in Listeners)
        {
            if (authListener.Equals(listener))
            {
                find = true;
                break;
            }
        }
        if (find)
        {
            Listeners.Remove(listener);
        }
    }


}

public interface PushInfoListener
{
    void OnBaiduPushInfo(string appId,string userId,string channelId);

    void OnIOSPushInfo(string deviceToken);
}
