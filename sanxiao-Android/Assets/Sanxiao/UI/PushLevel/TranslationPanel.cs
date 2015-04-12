using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    /// <summary>
    /// 平移缩放层
    /// </summary>
    public class TranslationPanel : MonoBehaviour
    {
        private const float ZoomInFactor = 2;

        public PushLevelUI PushLevelUI;
        public UIButton BtnReturn;

        public Bounds PanBounds;

        /// <summary>
        /// 严格的处于Planet View，此时的返回按钮才会退到MenuUI
        /// </summary>
        public bool IsStrictlyInPlanetView
        {
            get { return Mathf.Abs(transform.localScale.x - 1) < 0.01f; }
        }

        private void LateUpdate()
        {
            if (transform.localPosition.x < PanBounds.min.x) transform.localPosition = transform.localPosition.SetV3X(PanBounds.min.x);
            if (transform.localPosition.y < PanBounds.min.y) transform.localPosition = transform.localPosition.SetV3Y(PanBounds.min.y);
            if (transform.localPosition.x > PanBounds.max.x) transform.localPosition = transform.localPosition.SetV3X(PanBounds.max.x);
            if (transform.localPosition.y > PanBounds.max.y) transform.localPosition = transform.localPosition.SetV3Y(PanBounds.max.y);
        }

        public void GotoIslandView(Vector2 panPosition)
        {
            TweenPosition.Begin(gameObject, 0.3f, panPosition);
            TweenScale.Begin(gameObject, 0.3f, new Vector3(ZoomInFactor, ZoomInFactor, 1));

            PushLevelUI.Planet.collider.enabled = false;
            collider.enabled = true;
            GetComponent<UIDragObject>().enabled = true;

            BtnReturn.isEnabled = true;
            PushLevelUI.BtnGoLeft.isEnabled = false;
            PushLevelUI.BtnGoRight.isEnabled = false;
        }

        public void OnReturnClick()
        {
            if (IsStrictlyInPlanetView)
            {
                PushLevelUI.OnReturnClick();
            }
            else
            {
                GotoPlanetView();
            }
        }

        public void GotoPlanetView()
        {
            TweenPosition.Begin(gameObject, 0.3f, Vector3.zero);
            TweenScale.Begin(gameObject, 0.3f, Vector3.one);

            PushLevelUI.Planet.collider.enabled = true;
            collider.enabled = false;
            GetComponent<UIDragObject>().enabled = false;

            BtnReturn.isEnabled = false;
            PushLevelUI.Planet.RotationFinish();

            var islands = GetComponentsInChildren<Island>();
            foreach (var island in islands)
            {
                island.DidGotoPlanetView();
            }
        }

        void Awake()
        {
            BtnReturn.defaultColor = Color.white;//让放大镜按钮一开始就disabled。这个调用时为了调用UIButtonColor.Start()
        }
    }
}