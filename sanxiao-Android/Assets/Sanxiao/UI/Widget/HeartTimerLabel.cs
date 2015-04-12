using System;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

namespace Assets.Sanxiao.UI.Widget
{
    /// <summary>
    /// 显示爱心信息的控件
    /// </summary>
    [RequireComponent(typeof(UILabel))]
    public class HeartTimerLabel : MonoBehaviour
    {
        UILabel _label;

        void Start()
        {
            ForceRefresh();
        }

        /// <summary>
        /// >0表示，下一个要多少秒领取；0不显示；<0,绝对值表示多出来多少个
        /// </summary>
        private long _nextTimeSpanSeconds;

        void Update()
        {
            long nextTimeSpanSeconds;
            if (CommonData.HeartData.Count < CommonData.HeartData.MaxCount)//<
            {
                nextTimeSpanSeconds = Mathf.CeilToInt(CommonData.HeartData.NextHeartRealTime - Time.realtimeSinceStartup);
                if (nextTimeSpanSeconds < 0) nextTimeSpanSeconds = 0;
            }
            else if (CommonData.HeartData.Count == CommonData.HeartData.MaxCount)//==
            {
                nextTimeSpanSeconds = 0;
            }
            else//>
            {
                nextTimeSpanSeconds = -(CommonData.HeartData.Count - CommonData.HeartData.MaxCount);
            }

            if (nextTimeSpanSeconds == _nextTimeSpanSeconds) return;

            _nextTimeSpanSeconds = nextTimeSpanSeconds;

            ForceRefresh();
        }

        void ForceRefresh()
        {
            if (!_label) _label = GetComponent<UILabel>() ?? gameObject.AddComponent<UILabel>();
            if (_nextTimeSpanSeconds > 0)//显示倒计时
            {
                var str = string.Format("{0:00}:{1:00}", _nextTimeSpanSeconds / 60, _nextTimeSpanSeconds % 60);
                _label.text = str;
            }
            else if (_nextTimeSpanSeconds == 0)//空白
            {
                _label.text = "满";
            }
            else//显示多余数量
            {
                _label.text = string.Format("+{0}", -_nextTimeSpanSeconds);
            }
        }
    }
}