using UnityEngine;

namespace Assets.Sanxiao.Game.Skill
{
    /// <summary>
    /// 地震效果
    /// </summary>
    public class UIShake : MonoBehaviour
    {
        /// <summary>
        /// 地震技能
        /// </summary>
        /// <param name="amplitude">初振幅，~屏幕百分比</param>
        /// <param name="time">时间</param>
        public static void ShakeAUI(GameObject ui, float amplitude, float time)
        {
            var cs = ui.GetComponent<UIShake>();
            if (cs) cs.ForceEnd();
            Destroy(cs);
            
            cs = ui.AddComponent<UIShake>();
            cs._oriPos = ui.transform.localPosition;
            cs._shakeDirNormalized = Random.insideUnitCircle.normalized;
            cs._amplitudeStd = amplitude*640;
            cs._startTime = Time.time;
            cs._duration = time;
        }

        private Vector2 _oriPos;
        private Vector2 _shakeDirNormalized;
        private float _amplitudeStd;
        private const float Freq = 5;
        private float _startTime;
        private float _duration;
        void Update()
        {
            var t = Time.time - _startTime;
            if (t >= _duration)
            {
                ForceEnd();//复位
                return;
            }
            var f = (1 - t/_duration);
            f *= f;
            transform.localPosition = _shakeDirNormalized*_amplitudeStd*f*Mathf.Sin(2*Mathf.PI*Freq*t);
        }

        void ForceEnd()
        {
            transform.localPosition = _oriPos;//复位
            Destroy(this);
        }
    }
}