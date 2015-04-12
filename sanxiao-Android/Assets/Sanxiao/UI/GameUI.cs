using Assets.Sanxiao.Game;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI
{
    public class GameUI : BaseUI
    {
        #region 单例UI通用

        private static GameUI _instance;

        public static GameUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 GameUI instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        protected override void Release()
        {

        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/GameUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static GameUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<GameUI>(prefab, MainRoot.UIParent);//创建并成为单例
            Instance.OnStage();
            return Instance;
        }

        public override void OffStage()
        {
            Instance = null;//成为孤立
            base.OffStage();
        }

        public static void ExitStage()
        {
            if (Instance)
            {
                Instance.OffStage();
            }
        }

        #endregion

        public Transform SceneBg;

        public UILabel LblTimer;

        void Awake()
        {
            //var height =SceneBg.parent.InverseTransformPoint(MainRoot.Instance.MyCamera.ViewportToWorldPoint(new Vector3(0,1))).y;
            //SceneBg.localScale =
            //    SceneBg.localScale.SetV3Y(height/SceneBg.GetComponent<SpriteRenderer>().sprite.rect.height);
        }

        #region 技能使用面板

        public SkillButton[] SkillButtons;

        #endregion

        #region 4个槽

        public UISprite PgrMyHealth;
        public UISprite PgrMyEnergy;
        public UISprite PgrRivalHealth;
        public UISprite PgrRivalEnergy;

        public UILabel LblMyHealth, LblRivalHealth;

        #endregion

        public GameObject BtnPause;
        public GameObject PausePanel;

        public override bool OnEscapeClick()
        {
            if (PausePanel.activeSelf)
            {
                GameManager.Instance.Resume();
            }
            else
            {
                GameManager.Instance.Pause();
            }
            return true;
        }
    }
}