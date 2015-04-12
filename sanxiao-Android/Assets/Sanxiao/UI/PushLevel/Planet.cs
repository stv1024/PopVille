using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.PushLevel
{
    public class Planet : MonoBehaviour
    {
        public PushLevelUI PushLevelUI;

        public GameObject MajorLevelSectorTemplate;
        public List<GameObject> MajorLevelSectorList = new List<GameObject>();

        private ChallengeLevelConfig _challengeLevelConfig;

        public void Refresh()
        {
            _challengeLevelConfig =
                ConfigManager.GetConfig(ConfigManager.ConfigType.ChallengeLevelConfig) as ChallengeLevelConfig;
            if (_challengeLevelConfig == null)
            {
                Debug.LogError("没有ChallengeLevelConfig，玩个毛");
                return;
            }
            while (MajorLevelSectorList.Count < _challengeLevelConfig.MajorLevelList.Count)
            {
                MajorLevelSectorList.Add(null);
            }
            for (int i = 0; i < _challengeLevelConfig.MajorLevelList.Count; i++)
            {
                if (MajorLevelSectorList[i] == null)
                {
                    MajorLevelSectorList[i] = PrefabHelper.InstantiateAndReset(MajorLevelSectorTemplate, transform);
                    MajorLevelSectorList[i].name = "MajorLevelSector " + i;
                    MajorLevelSectorList[i].SetActive(true);
                    MajorLevelSectorList[i].transform.localEulerAngles = new Vector3(0, 0, -i*OffsetAngle);
                }

                var majorLevelId = _challengeLevelConfig.MajorLevelList[i].MajorLevelId;
                var cui = CommonData.ChallengeUnlockInfoList.Find(
                    x => x.MajorLevelId == majorLevelId);

                MajorLevelSectorList[i].GetComponentInChildren<Island>()
                                        .SetAndRefresh(PushLevelUI, majorLevelId, cui,
                                                       _challengeLevelConfig.MajorLevelList[i]);

            }
            //LastNotOpenSign.name = "MajorLevelButton " + CommonData.ChallengeUnlockInfoList.Count;

            MajorLevelSectorTemplate.SetActive(false);
        }

        void OnClick()
        {
            var curStdTouchPos =
                PushLevelUI.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(UICamera.lastTouchPosition));
            if (-180 <= curStdTouchPos.y && curStdTouchPos.y <= 250)
            {
                if (0 <= CurIndex && CurIndex < MajorLevelSectorList.Count)
                {
                    PushLevelUI.TranslationPanel.GotoIslandView(-curStdTouchPos);
                    var island = MajorLevelSectorList[CurIndex].GetComponentInChildren<Island>();
                    island.DidGotoThisIslandView();
                }
            }
        }

        #region 旋转

        public const int OffsetAngle = 20;
        public Transform PlanetTra;

        public float MomentumAmount = 70;
        private bool _pressed = false;
        public float mMomentum = 0;
        int mDragID = -10;

        private void OnPress(bool pressed)
        {
            if (!pressed && mDragID == UICamera.currentTouchID) mDragID = -10;

            if (pressed) Destroy(GetComponent<TweenRotation>());
            _pressed = pressed;
        }

        public void OnDrag(Vector2 delta)
        {
            if (mDragID == -10) mDragID = UICamera.currentTouchID;
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

            var worldDrag =
                (Camera.main.ScreenToWorldPoint(delta) - Camera.main.ScreenToWorldPoint(Vector3.zero)).SetV3Z(0);
            var curWorldTouchPos =
                Camera.main.ScreenToWorldPoint(UICamera.lastTouchPosition) - transform.position;
            var preWorldTouchPos = curWorldTouchPos - worldDrag;
            var dTheta = (Mathf.Atan2(curWorldTouchPos.y, curWorldTouchPos.x) -
                          Mathf.Atan2(preWorldTouchPos.y, preWorldTouchPos.x))*Mathf.Rad2Deg;
            PlanetTra.localEulerAngles = PlanetTra.localEulerAngles.AddV3Z(dTheta);

            mMomentum = Mathf.Lerp(mMomentum, mMomentum + dTheta*(0.01f*MomentumAmount), 0.67f);
        }

        private void LateUpdate()
        {
            float delta = RealTime.deltaTime;
            bool willButNotStop = false;
            bool doDampen = true;
            if (_pressed)
            {

            }
            else//已经松开
            {
                if (Mathf.Abs(mMomentum) > 0.5f)//动得很快
                {
                    if (Mathf.Abs(mMomentum) > 0.0001f)
                    {
                        PlanetTra.eulerAngles += new Vector3(0, 0, SpringDampen(ref mMomentum, 9f, delta));//控制旋转

                        doDampen = false;
                    }
                }
                else//快停了
                {
                    if (Mathf.Abs(mMomentum) > 0.0001f)//没停
                    {
                        willButNotStop = true;
                        mMomentum = 0;
                    }
                }
            }

            // Dampen the momentum
            if (doDampen) SpringDampen(ref mMomentum, 9f, delta);

            //计算_totalAngle
            var curEulerZ = PlanetTra.localEulerAngles.z;
            if (_lastEulerZ < 90 && curEulerZ > 270)
            {
                _totalAngle -= _lastEulerZ + 360 - curEulerZ;
            }
            else if (_lastEulerZ > 270 && curEulerZ < 90)
            {
                _totalAngle += curEulerZ + 360 - _lastEulerZ;
            }
            else
            {
                _totalAngle += curEulerZ - _lastEulerZ;
            }
            _lastEulerZ = curEulerZ;

            if (willButNotStop)
            {
                var index = Mathf.RoundToInt(_totalAngle / OffsetAngle);
                index = Mathf.Clamp(index, 0, MajorLevelSectorList.Count - 1);
                var displacementAng = OffsetAngle * index - PlanetTra.localEulerAngles.z;
                var duration = Mathf.Clamp(Mathf.Abs(displacementAng)/90 + 0.1f, 0.1f, 0.5f);
                var tr = MorlnTweenRotation.Begin(gameObject, duration, new Vector3(0, 0, OffsetAngle * index));
                tr.from = new Vector3(0, 0, _totalAngle);
                EventDelegate.Add(tr.onFinished, RotationFinish);
            }

            // 根据角度判断所有岛屿显示与隐藏
            for (int i = 0; i < MajorLevelSectorList.Count; i++)
            {
                if (Mathf.Abs(i * OffsetAngle - _totalAngle) <= OffsetAngle * 1.1f)
                {
                    if (!MajorLevelSectorList[i].activeSelf) MajorLevelSectorList[i].SetActive(true);
                }
                else
                {
                    if (MajorLevelSectorList[i].activeSelf) MajorLevelSectorList[i].SetActive(false);
                }
            }

            //根据角度判断名称Label的显示
            var floatIndex = _totalAngle/OffsetAngle;
            var closestIndex = Mathf.RoundToInt(floatIndex);
            if (closestIndex != _closestIndex)
            {
                PushLevelUI.LblMajorLevelName.text = _challengeLevelConfig != null && closestIndex >= 0 &&
                                                     closestIndex < _challengeLevelConfig.MajorLevelList.Count
                                                         ? _challengeLevelConfig.MajorLevelList[closestIndex].Title
                                                         : null;
                _closestIndex = closestIndex;
            }
            var d = Mathf.Abs(floatIndex - closestIndex);
            PushLevelUI.LblMajorLevelName.alpha = 1 - 4*d*d;
        }

        private int _closestIndex;

        private float _lastEulerZ;

        float _totalAngle;

        public float TotalAngle
        {
            get { return _totalAngle; }
            set
            {
                _totalAngle = value;
                transform.localEulerAngles = new Vector3(0, 0, _totalAngle);
                _lastEulerZ = _totalAngle;
            }
        }

        public static float SpringDampen(ref float velocity, float strength, float deltaTime)
        {
            if (deltaTime > 1f) deltaTime = 1f;
            float dampeningFactor = 1f - strength*0.001f;
            int ms = Mathf.RoundToInt(deltaTime*1000f);
            float totalDampening = Mathf.Pow(dampeningFactor, ms);
            float vTotal = velocity*((totalDampening - 1f)/Mathf.Log(dampeningFactor));
            velocity = velocity*totalDampening;
            return vTotal*0.06f;
        }

        public void RotationFinish()
        {
            CurIndex = Mathf.RoundToInt(_totalAngle / OffsetAngle);
            PushLevelUI.BtnGoLeft.isEnabled = PushLevelUI.Planet.CurIndex > 0;
            PushLevelUI.BtnGoRight.isEnabled = PushLevelUI.Planet.CurIndex < PushLevelUI.Planet.MajorLevelSectorList.Count - 1;
            if (_challengeLevelConfig != null)
            {
                PushLevelUI.LblMajorLevelName.text = _challengeLevelConfig.MajorLevelList[CurIndex].Title;
            }
        }

        public int CurIndex;
        public void OnGoLeftClick()
        {
            if (CurIndex > 0)
            {
                var tr = MorlnTweenRotation.Begin(gameObject, 0.322f, new Vector3(0, 0, OffsetAngle*(CurIndex - 1)));
                tr.from = new Vector3(0, 0, _totalAngle);
                EventDelegate.Add(tr.onFinished, RotationFinish);
            }
        }
        public void OnGoRightClick()
        {
            if (CurIndex < MajorLevelSectorList.Count - 1)
            {
                var tr = MorlnTweenRotation.Begin(gameObject, 0.322f, new Vector3(0, 0, OffsetAngle * (CurIndex + 1)));
                tr.from = new Vector3(0, 0, _totalAngle);
                EventDelegate.Add(tr.onFinished, RotationFinish);
            }
        }

        public void GoToIndex(int tarInd)
        {
            tarInd = Mathf.Clamp(tarInd, 0, MajorLevelSectorList.Count);
            var tr = MorlnTweenRotation.Begin(gameObject, 0.322f, new Vector3(0, 0, OffsetAngle * tarInd));
            tr.from = new Vector3(0, 0, _totalAngle);
            EventDelegate.Add(tr.onFinished, RotationFinish);
        }
        #endregion
    }
}