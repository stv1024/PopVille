//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script that shows how to download a remote texture and assign it to be used by a UITexture.
/// Modify by Stephen Zhou @ 2013-12-27
/// </summary>

[RequireComponent(typeof(UITexture))]
public class MorlnDownloadTexture : MonoBehaviour
{
	public string Url = null;

    private UITexture _uiTexture;
    public UITexture UITexture { get { return _uiTexture ?? (_uiTexture = GetComponent<UITexture>()); } }

	Texture2D mTex;
    /// <summary>
    /// 如果是null则隐藏，Empty则变成白板
    /// </summary>
    /// <param name="url"></param>
    public void SetDownloadShowPicture(string url)
    {
        Url = url;

        if (mTex != null) Destroy(mTex);//销毁旧的
        if (Url == null)
        {
            UITexture.enabled = false;
        }
        else
        {
            StartCoroutine(DownloadAndShow());
        }
    }
	IEnumerator DownloadAndShow ()
    {
		WWW www = new WWW(Url);
		yield return www;
		mTex = www.texture;

		if (mTex != null)
		{
            UITexture ut = UITexture;
		    ut.enabled = true;
			ut.mainTexture = mTex;
			//ut.MakePixelPerfect();
		}
		www.Dispose();
	}
}