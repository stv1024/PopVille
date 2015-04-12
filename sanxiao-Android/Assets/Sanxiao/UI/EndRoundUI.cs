using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel.Garden;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI
{
    public class EndRoundUI : BaseUI
    {
        #region 单例UI通用

        private static EndRoundUI _instance;

        public static EndRoundUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 EndRoundUI instance now!");
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
                var go = Resources.Load("UI/EndRoundUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static EndRoundUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<EndRoundUI>(prefab, MainRoot.UIParent);//创建并成为单例
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

        public AudioClip WinSound, LoseSound;
        public GameObject WinLosePanel, SettlementPanel, SkillUnlockPanel, VegetableUnlockPanel;
        public GameObject BadgeShadow;

        public GameObject Win, Lose;
        public GameObject[] Stars;

        public GameObject BtnNext;
        public UILabel LblOnButton;
        public UIPlayTween PlayTween;

        public float DurationWinLose, DurationSettlement, DurationSkillUnlock, DurationVegetableUnlock;
        public Character MyCharacter;
        public GameObject WinLight;

        public UILabel LblSettlementDescription;
        public UILabel LblLevel, LblExpRate, LblAddExp, LblAddCoin;
        public AudioClip AddCoinSound;
        public UISlider SldExp;
        public UIGrid GridReward;
        public GameObject RewardTemplate;
        public GameObject BtnLevelUpReward;
        public UILabel LblAddDiamond;

        public UIGrid GridUnlockSkill;
        public GameObject UnlockSkillTemplate;

        public UIGrid GridUnlockVegetable;
        public GameObject UnlockVegetableTemplate;

        private int _starCount;
        private List<Currency> _roundRewardList;
        private UnlockElement _unlockElement;
        private bool _gotoPushLevelWhenEnd;

        private bool _gotoNext;
        public void PlayEndRoundProcess(UploadChallengeOk uploadChallengeOk)
        {
            _gotoPushLevelWhenEnd = true;
            PlayEndRoundProcess(uploadChallengeOk.StarCount, uploadChallengeOk.RoundRewardList,
                                uploadChallengeOk.HasUnlockElement ? uploadChallengeOk.UnlockElement : null);
        }
        public void PlayEndRoundProcess(EndRound endRound)
        {
            _gotoPushLevelWhenEnd = false;
            PlayEndRoundProcess(endRound.Win ? 3 : 0, endRound.RoundRewardList, null);
        }

        /// <summary>
        /// 仅新手教程用
        /// </summary>
        /// <param name="starCount"></param>
        /// <param name="roundRewardList"></param>
        /// <param name="unlockElement"></param>
        public void PlayEndRoundProcess_FreshmanGuide(int starCount, List<Currency> roundRewardList, UnlockElement unlockElement)
        {
            _gotoPushLevelWhenEnd = false;
            PlayEndRoundProcess(starCount, roundRewardList, unlockElement);
        }

        void PlayEndRoundProcess(int starCount, List<Currency> roundRewardList, UnlockElement unlockElement)
        {
            _starCount = starCount;
            _roundRewardList = roundRewardList;
            _unlockElement = unlockElement;
            StartCoroutine(EndRoundCoroutine());
        }

        private IEnumerator EndRoundCoroutine()
        {
            //=====================共用控件=====================
            if (GameData.LastRoundWin)
            {
                Win.SetActive(true);
                Lose.SetActive(false);
                AudioManager.PlayOneShot(WinSound);
            }
            else
            {
                Win.SetActive(false);
                Lose.SetActive(true);
                AudioManager.PlayOneShot(LoseSound);
            }
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].SetActive(i < _starCount);//星星数量
            }
            BtnLevelUpReward.SetActive(false);
            BtnNext.SetActive(true);
            BadgeShadow.SetActive(false);

            WinLosePanel.SetActive(false);
            SettlementPanel.SetActive(false);
            SkillUnlockPanel.SetActive(false);
            VegetableUnlockPanel.SetActive(false);
            //=====================胜负面板=====================
            _gotoNext = false;
            EnterStage(WinLosePanel);
            BtnNext.SetActive(false);

            MyCharacter.CharacterCode = CommonData.MyUser.CharacterCode;
            MyCharacter.Refresh();
            var userCharacter = CommonData.CurUserCharacter;
            if (userCharacter == null)
            {
                MyCharacter.TakeOffAllEquip();
            }
            else
            {
                MyCharacter.WearEquip(userCharacter.WearEquipList);
            }

            if (GameData.LastRoundWin)
            {
                //WinLight.SetActive(true);
                LblOnButton.text = "查看战利品";
                MyCharacter.Cheer();
            }
            else
            {
                //WinLight.SetActive(false);
                LblOnButton.text = "再接再厉";
                MyCharacter.Cry();
            }
            yield return new WaitForSeconds(DurationWinLose);
            BtnNext.SetActive(true);
            PlayTween.Play(true);

            while (!_gotoNext)
            {
                yield return new WaitForEndOfFrame();
            }
            ExitStage(WinLosePanel);
            PlayTween.Play(false);

            //======================结算面板=====================
            if (GameData.LastRoundWin)
            {
                _gotoNext = false;
                EnterStage(SettlementPanel);
                BadgeShadow.SetActive(true);
                BtnLevelUpReward.SetActive(false);
                //BtnNext.SetActive(false);
                LblLevel.text = CommonData.MyUser.Level.ToString(CultureInfo.InvariantCulture);
                LblExpRate.text = string.Format("{0}%",
                                                Mathf.RoundToInt(Mathf.InverseLerp(CommonData.MyUser.ExpFloor,
                                                                                   CommonData.MyUser.ExpCeil,
                                                                                   CommonData.MyUser.Exp)));
                var addExp = _roundRewardList.Find(x => x.Type == (int) CurrencyType.Exp);
                var addExpAmount = addExp == null ? 0 : addExp.Amount;
                LblAddExp.text = "+" + addExpAmount;

                if (_unlockElement != null && _unlockElement.HasLevelUp)
                {
                    var energyCapUp = _unlockElement.HasEnergyCapacityUp
                                          ? (_unlockElement.EnergyCapacityUp.ToCapacity -
                                             _unlockElement.EnergyCapacityUp.FromCapacity)
                                          : 0;
                    if (energyCapUp == 0)
                    {
                        LblSettlementDescription.text = "恭喜您升级了！";
                    }
                    else
                    {
                        LblSettlementDescription.text = string.Format("恭喜您升级了！蓄力值上限增加[FFA000]{0}[-]点！", energyCapUp);
                    }
                    //LblAddDiamond.text = "+" + unlockElement.LevelUp.TODO:升级奖励的钻石在哪呢
                }
                else
                {
                    switch (_starCount)
                    {
                        case 0:
                            LblSettlementDescription.text = "下次一定能赢的！";
                            break;
                        case 1:
                            LblSettlementDescription.text = "表现不错哦！";
                            break;
                        case 2:
                            LblSettlementDescription.text = "干得漂亮！";
                            break;
                        case 3:
                            LblSettlementDescription.text = "精彩绝伦的战斗！";
                            break;
                        default:
                            LblSettlementDescription.text = "继续努力！";
                            break;
                    }
                }

                yield return new WaitForSeconds(1.6f); //1.6

                //经验条显示
                var maxValue1 = Mathf.InverseLerp(CommonData.MyUser.ExpFloor, CommonData.MyUser.ExpCeil,
                                                  CommonData.MyUser.Exp - addExpAmount);
                var maxValue2 = Mathf.InverseLerp(CommonData.MyUser.ExpFloor, CommonData.MyUser.ExpCeil,
                                                  CommonData.MyUser.Exp);
                var duration = 0.7f;
                for (float t = 0;;)
                {
                    if (t >= duration)
                    {
                        SldExp.value = maxValue1;
                        break;
                    }
                    SldExp.value = t/duration*maxValue1;
                    yield return new WaitForEndOfFrame();
                    t += Time.deltaTime;
                }
                //2.3
                //经验Add增长
                duration = 1f;
                for (float t = 0;;)
                {
                    if (t >= duration)
                    {
                        LblAddExp.text = "+" + addExpAmount;
                        break;
                    }
                    LblAddExp.text = "+" + Mathf.RoundToInt(addExpAmount*t/duration);
                    yield return new WaitForEndOfFrame();
                    t += Time.deltaTime;
                }
                //3.3
                //经验条增长
                duration = 0.8f;
                for (float t = 0;;)
                {
                    if (t >= duration)
                    {
                        t = duration;
                        SldExp.value = maxValue2;
                        break;
                    }
                    SldExp.value = Mathf.Lerp(maxValue1, maxValue2, t/duration);
                    yield return new WaitForEndOfFrame();
                    t += Time.deltaTime;
                }
                //4.1
                //奖励显现
                RewardTemplate.SetActive(true);
                yield return new WaitForSeconds(0.8f);
                //4.9
                var addCoin = _roundRewardList.Find(x => x.Type == (int) CurrencyType.Coin);
                var addCoinAmount = addCoin == null ? 0 : addCoin.Amount;
                duration = 1.6f;
                for (float t = 0;;)
                {
                    if (t >= duration)
                    {
                        LblAddCoin.text = "+" + addCoinAmount;
                        break;
                    }
                    LblAddCoin.text = "+" + Mathf.RoundToInt(addCoinAmount*t/duration);
                    yield return new WaitForEndOfFrame();
                    t += Time.deltaTime;
                }
                AudioManager.PlayOneShot(AddCoinSound);
                //6.5

                yield return new WaitForSeconds(DurationSettlement);
                if (_unlockElement != null && _unlockElement.HasLevelUp)
                {
                    BtnLevelUpReward.SetActive(true);
                    BtnNext.SetActive(false);
                }
                else
                {
                    BtnLevelUpReward.SetActive(false);
                    BtnNext.SetActive(true);
                }
                LblOnButton.text = "下一步";
                PlayTween.Play(true);

                while (!_gotoNext)
                {
                    yield return new WaitForEndOfFrame();
                }
                ExitStage(SettlementPanel);
                PlayTween.Play(false);
            }
            //======================技能解锁面板=====================
            if (_unlockElement != null && _unlockElement.SkillUnlockList.Count > 0)
            {
                _gotoNext = false;
                EnterStage(SkillUnlockPanel);

                for (int i = 0; i < _unlockElement.SkillUnlockList.Count; i++)
                {
                    var go = PrefabHelper.InstantiateAndReset(UnlockSkillTemplate, GridUnlockSkill.transform);
                    go.name = i.ToString(CultureInfo.InvariantCulture);
                    go.SetActive(true);
                    var spriteName = string.Format("skillicon-{0}", (SkillEnum)_unlockElement.SkillUnlockList[i].SkillCode);
                    var spr = go.GetComponentInChildren<UISprite>();
                    spr.spriteName = spriteName;
                    var skillDisplayName = SkillUtil.GetSkillDisplayName((SkillEnum) _unlockElement.SkillUnlockList[i].SkillCode);
                    var lbl = go.GetComponentInChildren<UILabel>();
                    if (lbl) lbl.text = skillDisplayName;
                }
                GridUnlockSkill.repositionNow = true;
                GridUnlockSkill.transform.localPosition =
                    GridUnlockSkill.transform.localPosition.SetV3X(-GridUnlockSkill.cellWidth*
                                                                   (_unlockElement.SkillUnlockList.Count - 1)*0.5f);
                UnlockSkillTemplate.SetActive(false);

                yield return new WaitForSeconds(DurationSkillUnlock);
                BtnNext.SetActive(true);
                PlayTween.Play(true);
                while (!_gotoNext)
                {
                    yield return new WaitForEndOfFrame();
                }
                ExitStage(SkillUnlockPanel);
                PlayTween.Play(false);
            }

            //======================蔬菜解锁面板=====================
            if (_unlockElement != null && _unlockElement.VegetableUnlockList.Count > 0)
            {
                _gotoNext = false;
                EnterStage(VegetableUnlockPanel);

                for (int i = 0; i < _unlockElement.VegetableUnlockList.Count; i++)
                {
                    var go = PrefabHelper.InstantiateAndReset(UnlockVegetableTemplate, GridUnlockVegetable.transform);
                    go.name = i.ToString(CultureInfo.InvariantCulture);
                    go.SetActive(true);
                    var cs = go.GetComponentInChildren<VegetableSlot>();
                    var userVegetable =
                        CommonData.MyVegetableList.Find(
                            x => x.VegetableCode == _unlockElement.VegetableUnlockList[i].VegetableCode);
                    if (userVegetable != null)
                    {
                        userVegetable.CurrentUpgradeLimit = _unlockElement.VegetableUnlockList[i].NewUpgradeLimit;
                        if (cs) cs.SetAndRefresh(userVegetable);
                    }
                    else
                    {
                        Debug.LogError("竟然找不到UserVegetable.Code:" + _unlockElement.VegetableUnlockList[i].VegetableCode);
                    }
                }
                GridUnlockVegetable.repositionNow = true;
                GridUnlockVegetable.transform.localPosition =
                    GridUnlockVegetable.transform.localPosition.SetV3X(-GridUnlockVegetable.cellWidth *
                                                                   (_unlockElement.VegetableUnlockList.Count - 1) * 0.5f);
                UnlockVegetableTemplate.SetActive(false);

                yield return new WaitForSeconds(DurationVegetableUnlock);
                BtnNext.SetActive(true);
                PlayTween.Play(true);
                while (!_gotoNext)
                {
                    yield return new WaitForEndOfFrame();
                }
                _gotoNext = false;
                PlayTween.Play(false);
            }

            //======================退出结局UI=====================
            MusicManager.Instance.CrossFadeIn();
            if (_gotoPushLevelWhenEnd)
            {
                MainRoot.Goto(MainRoot.UIStateName.PushLevel);
                while (!PushLevelUI.Instance)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (CommonData.JustUnlockedMajorLevelId == null)//未解锁大关
                {
                    var majorLevel =
                        CommonData.ChallengeUnlockInfoList.Find(
                            x => x.MajorLevelId == GameData.LastChallengeMajorLevelID);
                    if (majorLevel != null)
                    {
                        PushLevelUI.Instance.EnterMajorLevel(majorLevel);
                    }
                }
                else//解锁大关
                {
                    var majorLevel =
                        CommonData.ChallengeUnlockInfoList.Find(
                            x => x.MajorLevelId == CommonData.JustUnlockedMajorLevelId);
                    print("justunlock ml:" + majorLevel);
                    if (majorLevel != null)
                    {
                        PushLevelUI.Instance.EnterMajorLevel(majorLevel);
                    }
                }
            }
            else
            {
                MainRoot.Goto(MainRoot.UIStateName.Menu);
            }
        }

        public AnimationCurve EnterCurve, ExitCurve;
        void EnterStage(GameObject panel)
        {
            //panel.transform.localPosition = new Vector3(1000, 0, 0);
            panel.SetActive(true);
            //var tp = TweenPosition.Begin(panel, 0.6f, Vector3.zero);
            //tp.animationCurve = EnterCurve;
        }
        void ExitStage(GameObject panel)
        {
            var tp = TweenPosition.Begin(panel, 0.4f, new Vector3(-1000, 0, 0));
            tp.animationCurve = ExitCurve;
            StartCoroutine(panel.SetActiveDelay(false, 0.4f));
        }

        public void OnConfirmClick()
        {
            if (_gotoNext)
            {
                Debug.LogError("出错了");
                return;
                MusicManager.Instance.CrossFadeIn();
                MainRoot.Goto(MainRoot.UIStateName.PushLevel);
            }
            else
            {
                _gotoNext = true;
            }
        }

        public void OnShowOffClick()
        {
            Debug.Log("炫耀");
        }
    }
}