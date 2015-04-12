using System.Collections;
using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.Game;
using Assets.Sanxiao.UI;
using Assets.Sanxiao.UI.Panel;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao
{
    /// <summary>
    /// 新手引导
    /// </summary>
    public class FreshmanGuide : MonoBehaviour
    {
        public static FreshmanGuide Instance { get; private set; }

        public static FreshmanGuide StartGuide()
        {
            if (Instance)
            {
                Debug.LogError("不应该有第二个实例");
                return Instance;
            }
            var holder = new GameObject("FreshmanGuide");
            Instance = holder.AddComponent<FreshmanGuide>();
            Instance.StartCoroutine(Instance.GuideCoroutine());
            return Instance;
        }
        /// <summary>
        /// 标识是否可以进行下一环节
        /// </summary>
        private bool _next;
        IEnumerator GuideCoroutine()
        {
            transform.localPosition = new Vector3(0, 0, -500);
            gameObject.layer = 8;
            var boxCollider = GetComponent<BoxCollider>();
            if (!boxCollider) boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(2000, 2000, 0);
            boxCollider.center = Vector3.zero;
            boxCollider.isTrigger = true;

            //选择昵称
            boxCollider.enabled = false;
            SelectNicknamePanel.Load();
            while (!SelectNicknamePanel.NicknameSelectOk) yield return new WaitForEndOfFrame();

            UMengPlugin.UMengEvent(EventId.NICKNAME_FINISH,
                                   new Dictionary<string, object>
                                       {
                                           {"random_times", SelectNicknamePanel.RandomTimes}
                                       });

            MusicManager.Instance.CrossFadeOut();

            //加载GameUI
            MainRoot.Goto(MainRoot.UIStateName.Game);
            while (!GameManager.Instance) yield return new WaitForEndOfFrame();
            SelectNicknamePanel.UnloadInterface();//此时才能销毁SelectNicknamePanel，不然会突变
            GameUI.Instance.gameObject.SetActive(false);

            #region 故事板

            _next = false;

            var mangaPrefab = MorlnResources.Load("UI/FreshmanGuide/Prefabs/Manga") as GameObject;
            var manga = PrefabHelper.InstantiateAndReset(mangaPrefab, transform);
            manga.transform.SetSortingLayer("Foreground");
            manga.GetComponentInChildren<Animator>().enabled = true;
            Destroy(manga, 24f);
            yield return new WaitForSeconds(22.5f);
            _next = true;
            //while (!_next) yield return new WaitForEndOfFrame();

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "故事板完"}
                                       });

            GameUI.Instance.gameObject.SetActive(true);

            #region 三消教学

            _next = false;
            boxCollider.enabled = false;

            GameData.RivalBossData = new DefenseData("鸡妈妈", 1, 90005, 100, 100);

            GameManager.Instance.ClearDefenserSetAIAndCameraAndTimeScale();

            var grid = GameManager.Instance.MyGrid;

            grid.ResetCells(5, 5, null);
            GameData.InitVegetableTypeCount = 4;
            //填入预设的蔬菜
            const string text = @"0 1 0 3 4
4 3 2 1 0
0 2 4 2 1
3 1 0 4 3
0 1 4 3 2";
            grid.CreateCandysAsPreset(text);

            GameManager.Instance.ResetAndRefreshAndStartFreshmanGuideRound();

            #region 母子对话

            yield return new WaitForSeconds(1.5f);

            var lines = new[] { new Line(false, "慢着，老娘我先陪你练练", 2.5f), new Line(true, "妈，我怕伤着你", 1.1f), new Line(false, "少废话，接招", 2f) };
            var bubblePrefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/Bubble");
            var bubbleGo = PrefabHelper.InstantiateAndReset(bubblePrefab, transform);
            var bubbleSpr = bubbleGo.GetComponentInChildren<UISprite>();
            var talkLabel = bubbleGo.GetComponentInChildren<UILabel>();
            foreach (var line in lines)
            {
                bubbleSpr.transform.localScale = new Vector3(line.Left ? -1 : 1, 1, 1);
                talkLabel.text = line.Text;
                yield return new WaitForSeconds(line.Length);
            }
            Destroy(bubbleGo);

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "母子对话完"}
                                       });

            //开始遮罩教学
            var prefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/GuideMask-Sanxiao");
            GameObject go = null;
            if (prefab)
            {
                go = PrefabHelper.InstantiateAndReset(prefab, MainRoot.Instance.transform);
            }
            else
            {
                Debug.LogError("不能没有这个Prefab！");
            }

            while (!_next)
            {
                //监测是否完成操作，完成则_next = true;
                if (GameData.MyEnergy > 0) _next = true;
                yield return new WaitForEndOfFrame();
            }
            Destroy(go);
            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "三消教学完"}
                                       });

            #region 蓄力值

            _next = false;
            boxCollider.enabled = true;

            prefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/GuideMask-Energy");
            go = null;
            if (prefab)
            {
                go = PrefabHelper.InstantiateAndReset(prefab, MainRoot.Instance.transform);
            }
            else
            {
                Debug.LogError("不能没有这个Prefab！");
            }

            while (!_next) yield return new WaitForEndOfFrame();
            Destroy(go);

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "教蓄力值完"}
                                       });
            
            #region 使用技能

            while (true)
            {
                if (GameData.MyEnergy >= GameData.MyEnergyCapacity) break;
                yield return new WaitForEndOfFrame();
            }

            _next = false;
            boxCollider.enabled = false;

            prefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/GuideMask-UseSkill");
            go = null;
            if (prefab)
            {
                go = PrefabHelper.InstantiateAndReset(prefab, MainRoot.Instance.transform);
            }
            else
            {
                Debug.LogError("不能没有这个Prefab！");
            }

            while (!_next)
            {
                if (GameData.MyEnergy < 120) _next = true;//证明使用过技能了
                yield return new WaitForEndOfFrame();
            }
            Destroy(go);

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "使用技能完"}
                                       });

            #region 造成了伤害
            while (true)
            {
                if (GameData.RivalHealthList[1] <= 0) break;
                yield return new WaitForEndOfFrame();
            }

            _next = false;

            yield return new WaitForSeconds(2);

            boxCollider.enabled = true;

            prefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/GuideMask-Damage");
            go = null;
            if (prefab)
            {
                go = PrefabHelper.InstantiateAndReset(prefab, MainRoot.Instance.transform);
            }
            else
            {
                Debug.LogError("不能没有这个Prefab！");
            }

            while (!_next) yield return new WaitForEndOfFrame();
            Destroy(go);

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "对战完"}
                                       });

            #region 结局
            _next = false;
            boxCollider.enabled = false;

            while (true)
            {
                if (GameData.RivalHealthList[0] <= 0 || _next) break;
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2);

            GameData.LastRoundWin = true;

            MainRoot.Goto(MainRoot.UIStateName.EndRound);
            while (!EndRoundUI.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            EndRoundUI.Instance.PlayEndRoundProcess_FreshmanGuide(3,
                                                                  new List<Currency>
                                                                      {
                                                                          new Currency((int) CurrencyType.Coin,
                                                                                       CommonData.MyUser.Money10),
                                                                          new Currency((int) CurrencyType.Diamond,
                                                                                       CommonData.MyUser.Money1)
                                                                      }, null);

            prefab = MorlnResources.Load<GameObject>("UI/FreshmanGuide/Prefabs/GuideMask-KillFellow");
            go = null;
            if (prefab)
            {
                go = PrefabHelper.InstantiateAndReset(prefab, MainRoot.Instance.transform);
            }
            else
            {
                Debug.LogError("不能没有这个Prefab！");
            }

            while (!_next) yield return new WaitForEndOfFrame();
            Destroy(go);

            #endregion

            UMengPlugin.UMengEvent(EventId.GUIDE_PROGRESS,
                                   new Dictionary<string, object>
                                       {
                                           {"state", "引导完"}
                                       });
        }

        struct Line
        {
            public bool Left { get; private set; }
            public string Text { get; private set; }
            public float Length { get; private set; }
            public Line(bool left, string text, float length) : this()
            {
                Left = left;
                Text = text;
                Length = length;
            }
        }

        void OnClick()
        {
            _next = true;
        }

        public static void GotoNext()
        {
            if (Instance) Instance._next = true;
        }
    }
}