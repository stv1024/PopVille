using System;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.UI.Widget
{
    /// <summary>
    /// 显示爱心信息的控件
    /// </summary>
    public class HeartInfoBar : MonoBehaviour, IRefreshable
    {
        #region 数据Label通用
        void Awake()
        {
            CommonData.HeartData.CountListenerList.Add(this);
            Refresh();
        }
        void OnDestroy()
        {
            CommonData.HeartData.CountListenerList.Remove(this);
        }
        #endregion

        public UISprite[] SprHearts;

        /// <summary>
        /// 刷新，采用CommonData的数据
        /// </summary>
        public void Refresh()
        {
            for (int i = 0; i < SprHearts.Length; i++)
            {
                SprHearts[i].enabled = i < CommonData.HeartData.Count;
            }
        }
    }
}