using System;
using SimpleJson;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 友盟插件。
/// 没有必要绑定任何物体。
/// </summary>
public class UMengPlugin 
{

    
    /// <summary>
    /// 友盟事件统计
    /// </summary>
    /// <param name="newVsEvent"></param>
    public static void UMengEvent(EventId eventId,Dictionary<string,object> dic)
    {
        try
        {
#if UNITY_ANDROID
            string json = null;
            if (dic != null && dic.Count > 0)
            {
                JsonNode root = new JsonNode(NodeType.Object);

                foreach (KeyValuePair<string, object> pair in dic)
                {
                    string value = pair.Value.ToString();
                    root.AddSubNode(pair.Key, new JsonNode(pair.Value.ToString()));

                }
                json = root.ToJson();
            }
            else
            {
                json = "{}";
            }
            Debug.Log(json);
            AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("umengEvent", eventId.ToString().ToLower(), json);
#elif UNITY_IPHONE

#endif

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// 友盟用户反馈
    /// </summary>
    public static void UMengFeedback()
    {
        try
        {
#if UNITY_ANDROID
        AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("umengFeedback");
#elif UNITY_IPHONE

#endif

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    /// <summary>
    /// 友盟分享。主要包括新浪微博，腾讯微博，短信。QQ空间，微信朋友圈。
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="picPath">分享图片的绝对地址</param>
    /// <param name="url">微信朋友圈的url:如果是null或""则没有微信朋友圈分享</param>
    public static void UMengShare(string content,string picPath,string url)
    {
        try
        {
#if UNITY_ANDROID
        AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("umengShare",content,picPath,url);
#elif UNITY_IPHONE

#endif
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }



}
