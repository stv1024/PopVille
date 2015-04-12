using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 岛——美术填写
    /// 可利用ButtonMessage脚本向此组件发送OnDescriptionClick事件，通知上层显示大关描述
    /// </summary>
    public class IslandInfo : MonoBehaviour
    {
        /// <summary>
        /// 美术填写Route
        /// </summary>
        public Route Route;
        public Island Island { get; set; }
        /// <summary>
        /// 可利用ButtonMessage脚本向此组件发送OnDescriptionClick事件，通知上层显示大关描述
        /// </summary>
        public void OnDescriptionClick()
        {
            UMengPlugin.UMengEvent(EventId.PUSHLEVEL_CLICK_ME,
                                   new Dictionary<string, object> {{"major", Island.MajorLevelId}});

            if (Island) Island.ShowDescription();
        }

        #region 云

        /// <summary>
        /// 美术填写美术调用
        /// </summary>
        public Animator[] CloudAnimators;

        /// <summary>
        /// 所有云的父物体/容器——美术填写，程序使用根据数据激活失活所有云
        /// </summary>
        public GameObject CloudsContainer;

        /// <summary>
        /// 播放解锁大关效果。只能调用一次
        /// </summary>
        public void PlayUnlockEffect()
        {
            foreach (var c in CloudAnimators)
            {
                c.SetTrigger("Dissipate");
            }
        }

        #endregion

    }
}