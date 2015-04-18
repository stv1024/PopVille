using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fairwood.Util;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// 图片下载读取缓存管理器
    /// </summary>
    public class ImageResourcesManager : MonoBehaviour
    {
        private static ImageResourcesManager _instance;

        public static ImageResourcesManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new GameObject("ImageResources Manager").AddComponent<ImageResourcesManager>();
                _instance.Initialize();
                return _instance;
            }
        }

        public static readonly string CacheFolder = Application.persistentDataPath + "/Images/";

        public delegate void OnImageDownloaded(Texture2D texture);

        public static void LoadImageAndWait(string url, OnImageDownloaded onImageDownloaded)
        {
            Instance.StartCoroutine(Instance.LoadImageAndWaitCoroutine(url, onImageDownloaded));
            Instance._flushAndReleaseNow = true;
        }
        IEnumerator LoadImageAndWaitCoroutine(string url, OnImageDownloaded onImageDownloaded)
        {
            var filename = WWW.EscapeURL(url);
            var filePath = CacheFolder + filename;
            
            //记录时间戳
            if (ImageTimeStampDict.ContainsKey(filename))
            {
                ImageTimeStampDict[filename] = DateTime.Now.Ticks;
            }
            else
            {
                ImageTimeStampDict.Add(filename, DateTime.Now.Ticks);
            }
            _flushAndReleaseNow = true;

            var dirInfo = new DirectoryInfo(CacheFolder);
            if (!dirInfo.Exists) dirInfo.Create();

            if (File.Exists(filePath))
            {
                if (onImageDownloaded != null)
                {
                    var bytes = new byte[1];// File.ReadAllBytes(filePath);
                    var texture = new Texture2D(1, 1);
                    texture.LoadImage(bytes);
                    onImageDownloaded(texture);
                }
            }
            else
            {
                var www = new WWW(url);
                yield return www;
                if (www.error == null)
                {
                    Debug.Log("下载图片，大小(B):" + www.size);
                    //File.WriteAllBytes(filePath, www.bytes);
                    if (onImageDownloaded != null) onImageDownloaded(www.texture);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }

        #region 缓存清理机制

        static readonly Dictionary<string, long> ImageTimeStampDict = new Dictionary<string, long>();
        /// <summary>
        /// 初始化，可重复调用，不影响性能。
        /// </summary>
        public static ImageResourcesManager Init(int daysToCache)
        {
            var tmp = Instance;
            _maxCacheTimespanTicks = new TimeSpan(daysToCache, 0, 0, 0).Ticks;
            return tmp;
        }

        private static long _maxCacheTimespanTicks = long.MaxValue;

        private void Initialize()
        {
            //从info文件读取图片时间戳信息
            var dirInfo = new DirectoryInfo(CacheFolder);
            if (!dirInfo.Exists) dirInfo.Create();
            var filePath = CacheFolder + "imageinfo.txt";
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(' ');
                    if (parts.Length >= 2)
                    {
                        var imageFilename = parts[0].Trim();
                        var part1 = parts[1].Trim();
                        long timestamp;
                        if (long.TryParse(part1, out timestamp))
                        {
                            if (!ImageTimeStampDict.ContainsKey(imageFilename))
                                ImageTimeStampDict.Add(imageFilename, timestamp);
                        }
                        else
                        {
                            Debug.LogError("时间戳格式不对:" + part1);
                        }
                    }
                    else
                    {
                        Debug.LogError("行格式不对:" + line);
                    }
                }
            }
            //Dict已经可用并且持续最新了
        }

        static void SmartRelease()
        {
            var dirInfo = new DirectoryInfo(CacheFolder);
            if (!dirInfo.Exists) return;//就不需要释放硬盘了
            var fileInfos = new FileInfo[1];// dirInfo.GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                var nowTimeStamp = DateTime.Now.Ticks;
                if (ImageTimeStampDict.ContainsKey(fileInfo.Name))
                {
                    if (nowTimeStamp - ImageTimeStampDict[fileInfo.Name] > _maxCacheTimespanTicks)//文件很久没有用过了，就删除
                    {
                        //fileInfo.Delete();
                        ImageTimeStampDict.Remove(fileInfo.Name);
                    }
                }
                else//没有记录的统统删除
                {
                    //fileInfo.Delete();
                }
            }
        }

        private bool _flushAndReleaseNow; 
        void Update()
        {
            if (_flushAndReleaseNow)
            {
                _flushAndReleaseNow = false;
                //Flush To HD

                var dirInfo = new DirectoryInfo(CacheFolder);
                if (!dirInfo.Exists) dirInfo.Create();

                var lines = new string[ImageTimeStampDict.Count];
                var i = 0;
                foreach (var line in ImageTimeStampDict.Select(pair => string.Format("{0} {1}", pair.Key, pair.Value)))
                {
                    lines[i] = line;
                    i++;
                }

                SmartRelease();
            }
        }

        #endregion
    }
}