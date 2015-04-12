using UnityEngine;

namespace Assets.Sanxiao.UI
{
    /// <summary>
    /// 加载mask
    /// </summary>
    public class LoadingMask : MonoBehaviour
    {
        public static LoadingMask Instance { get; private set; }
        void Awake()
        {
            Instance = this;
            transform.localPosition = StartPos;
            gameObject.SetActive(false);
        }

        public Vector3 StartPos, EndPos;
        /// <summary>
        /// t∈[0,1], y:0~1
        /// </summary>
        public AnimationCurve Curve;

        public AudioClip DownSound, UpSound;
        private bool _shouldEnd;
        void _StartLoading()
        {
            _shouldEnd = false;
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                _t = 0;
                AudioManager.PlayOneShot(DownSound);
            }
            else
            {
                if (_t > 1) _t = 2 - _t;
            }
        }

        public float Length = 0.5f;
        public float TimeToUnload = 0.5f;
        private float _t;
        void Update()
        {
            var lastT = _t;
            _t += Time.deltaTime*1/Length;
            if (_t > 2)
            {
                _t = 0;
                gameObject.SetActive(false);
                return;
            }
            if (_t > 1)
            {
                if (_shouldEnd)
                {
                    if (lastT <= 1)
                    {
                        AudioManager.PlayOneShot(UpSound);
                    }
                }
                else
                {
                    _t = 1;
                }
            }
            var t = _t <= 1 ? _t : 2 - _t;
            var y = Curve.Evaluate(t);
            transform.localPosition = StartPos*(1 - y) + EndPos*t;
        }

        void _EndLoading()
        {
            _shouldEnd = true;
        }

        public static void StartLoading()
        {
            Instance._StartLoading();
        }
        public static void EndLoading()
        {
            Instance._EndLoading();
        }
    }
}