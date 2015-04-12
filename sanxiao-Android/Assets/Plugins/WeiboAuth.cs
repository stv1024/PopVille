using System.Runtime.InteropServices;
using SimpleJson;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 新浪微博，腾讯微博授权，人人授权。
/// 
/// 该脚本必须放在物体AndroidReceiver和IOSReceiver上面
/// </summary>
public class WeiboAuth : MonoBehaviour {

    /// <summary>
    /// 授权类型
    /// </summary>
    public enum AuthType
    {
        SinaWeibo = 1,

        TecentWeibo = 2,
        
        Renren
    }

    /// <summary>
    /// 授权返回信息
    /// </summary>
    public enum AuthResponseType
    {
        Code = 1,
        AccessToken = 2
    }

   

#if UNITY_IPHONE
     [DllImport("__Internal")]
    private static extern void _weiboAuth(string appKey,string redirectUrl,int authType,int responseType);
#endif
    /// <summary>
    /// 开始授权。
    /// </summary>
    /// <param name="appKey">开发者中心注册应用时获得的API KEY</param>
    /// <param name="redirectUrl">授权流程结束后要跳转回的URL地址，该地址必须在开发者中心注册的网站地址之后</param>
    /// <param name="authType"></param>
    /// <param name="responseType"></param>
    public static void Auth(string appKey, string redirectUrl, AuthType authType = AuthType.SinaWeibo, AuthResponseType responseType = AuthResponseType.Code)
    {
#if UNITY_ANDROID
        AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("bindInfo", appKey, redirectUrl, (int)authType, (int)responseType);
#elif UNITY_IPHONE
       _weiboAuth(appKey, redirectUrl, (int)authType, (int)responseType);
#endif

    }
    /// <summary>
    /// json:{"uid":"1858307962","expires_in":"7751971","authType":1,"remind_in":"7751971","access_token":"2.00EnQlBCgTJYRD22eb73a2f6RG7bQC"}
    /// or
    /// json:{"authType":1,"code":"xxxxxxx"}
    /// </summary>
    /// <param name="result"></param>
    public void WeiboAuthResult(string result)
    {
        Debug.Log("WeiboAuthResult:   "+result);
        try
        {
            JsonNode root = JsonNode.FromJson(result);

            AuthType authType = (AuthType)(int)root["authType"].Value;


            ;



            var codeNode = root["code"];

            if (codeNode != null)
            {
                string code = (string)codeNode.Value;
                foreach (WeiboAuthListener listener in Listeners)
                {
                    listener.OnResult(authType, code);
                }
            
            }

            var tokenNode = root["access_token"];

            if (tokenNode != null)
            {
                string token = root["access_token"];
                long expireIn = root["expires_in"];
                long uid = root["uid"];
                foreach (WeiboAuthListener listener in Listeners)
                {
                    listener.OnResult(authType, uid, expireIn, token);
                }
            }




        }
        catch (SimpleJson.SimpleJsonException e)
        {
            Debug.Log(e.Message);
        }

    }

    private static readonly List<WeiboAuthListener>  Listeners = new List<WeiboAuthListener>();
 
    public static void AddWeiboAuthListener(WeiboAuthListener listener)
    {
        if (Listeners.Contains(listener))
        {
            
        }
        else
        {
            Listeners.Add(listener);
        }
    }
    public static void RemoveWeiboAuthListener(WeiboAuthListener listener)
    {
        bool find = false;
        foreach (WeiboAuthListener authListener in Listeners)
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

public interface WeiboAuthListener
{
    void OnResult(WeiboAuth.AuthType authType,string code);

    void OnResult(WeiboAuth.AuthType authType,long uid,long expiresIn,string accessToken);
}
