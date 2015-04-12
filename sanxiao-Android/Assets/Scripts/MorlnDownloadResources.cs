using System;
using System.Collections;
using Fairwood.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public static class MorlnDownloadResources
    {
        /// <summary>
        /// 请确保先ForceLoadAssetBundle()，再调用这个
        /// </summary>
        public static AssetBundle DownloadAssetBundle { get; private set; }

        /// <summary>
        /// 可以通过这个查询下载进度
        /// </summary>
        public static WWW DownloadAssetBundleWWW;
        public static IEnumerator ForceLoadAssetBundle()
        {
            var url = new Uri(Application.streamingAssetsPath + "/dlres_Android_01.unity3d").AbsoluteUri;
            //var url = UriUtil.GetUrlFromLocalPath(Application.streamingAssetsPath + "/dlres_Android_01.unity3d");
            DownloadAssetBundleWWW = new WWW(url);
            yield return DownloadAssetBundleWWW;
            if (DownloadAssetBundleWWW.error == null)
            {
                DownloadAssetBundle = DownloadAssetBundleWWW.assetBundle;
            }
            else
            {
                Debug.LogError(DownloadAssetBundleWWW.error);
            }
        }


        public static T Load<T>(string path) where T : Object
        {
            var slashInd = path.LastIndexOf('/');
            if (slashInd >= 0) path = path.Substring(slashInd + 1);
            if (DownloadAssetBundle != null) return DownloadAssetBundle.Load(path, typeof (T)) as T;
            return null;
        }

        public static Object Load(string path)
        {
            var slashInd = path.LastIndexOf('/');
            if (slashInd >= 0) path = path.Substring(slashInd + 1);
            if (DownloadAssetBundle != null) return DownloadAssetBundle.Load(path);
            return null;
        }

        public static Object Load(string path, Type systemTypeInstance)
        {
            var slashInd = path.LastIndexOf('/');
            if (slashInd >= 0) path = path.Substring(slashInd + 1);
            if (DownloadAssetBundle != null) return DownloadAssetBundle.Load(path, systemTypeInstance);
            return null;
        }
    }
}