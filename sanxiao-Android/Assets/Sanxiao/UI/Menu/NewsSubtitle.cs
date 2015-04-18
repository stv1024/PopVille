using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using HTMLEngine;
using HTMLEngine.Unity3D;
using UnityEngine;

namespace Assets.Sanxiao.UI.Menu
{
    /// <summary>
    /// 菜单UI新闻字幕
    /// </summary>
    public class NewsSubtitle : MonoBehaviour
    {
        public static readonly List<News> NewsQueue = new List<News>();

        public UILabel LblHtml;

        private WaitHintTextConfig _waitHintTextConfig;
        void Start()
        {
            _waitHintTextConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.WaitHintTextConfig) as WaitHintTextConfig;
            StartCoroutine(Rolling());
        }

        IEnumerator Rolling()
        {
            var i = 0;
            while (gameObject)
            {
                if (NewsQueue.Count > 0)
                {
                    if (i >= NewsQueue.Count) i = 0;
                    var text = NewsQueue[i].Content;
                    LblHtml.text = text;
                }
                else if (_waitHintTextConfig != null)
                {
                    if (i >= _waitHintTextConfig.HintList.Count) i = 0;
                    var text = _waitHintTextConfig.HintList[i];
                    LblHtml.text = text;
                }
                else
                {
                    LblHtml.gameObject.SetActive(false);
                    yield return new WaitForEndOfFrame();
                    continue;
                }
                LblHtml.gameObject.SetActive(true);
                LblHtml.GetComponent<Animation>().Rewind();
                LblHtml.GetComponent<Animation>().Play();
                yield return new WaitForSeconds(LblHtml.GetComponent<Animation>().clip.length);
                i++;
            }
        }
    }
}