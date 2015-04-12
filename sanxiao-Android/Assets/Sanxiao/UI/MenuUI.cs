using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel;
using Fairwood.Math;
using UnityEngine;
using UserMailList = Assets.Sanxiao.Communication.UpperPart.UserMailList;

namespace Assets.Sanxiao.UI
{
    public class MenuUI : BaseUI
    {
        #region 单例UI通用

        private static MenuUI _instance;

        public static MenuUI Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 MenuUI instance now!");
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
                var go = Resources.Load("UI/MenuUI") as GameObject;
                return go;
            }
        }

        /// <summary>
        /// 进场
        /// </summary>
        public static MenuUI EnterStage()
        {
            if (Instance)
            {
                Instance.gameObject.SetActive(true);//确保激活
                return Instance;
            }
            var prefab = Prefab;//加载进内存
            if (!prefab) return null;
            Instance = PrefabHelper.InstantiateAndReset<MenuUI>(prefab, MainRoot.UIParent);//创建并成为单例
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

        public Character MyCharacter;
        public UILabel LblNickname, LblLevel, LblExp;
        public UISlider SldExp;
        public UILabel LblAttack, LblDefense, LblEnergy;
        public UISprite SprEnergy;
        public NoticeNumberButton NtcMail;
        public UIButton BtnGarden, BtnSkill, BtnRealTimeFighting, BtnAdventure;
        public static bool HighlightAdventureBtn;

        void Start()
        {
            BtnGarden.UpdateColor(true, true);
            BtnSkill.UpdateColor(true, true);
            BtnRealTimeFighting.UpdateColor(true, true);
            BtnAdventure.UpdateColor(true, true);

            if (!_hasRefreshed) RefreshMyInfo();//确保加载完成后一定会刷新至少一次

            if (HighlightAdventureBtn)
            {
                BtnAdventure.GetComponent<Animator>().enabled = true;
                HighlightAdventureBtn = false;
            }
            else
            {
                BtnAdventure.GetComponent<Animator>().enabled = false;
            }
        }

        void OnEnable()
        {
            RefreshMyInfo();
        }

        private bool _hasRefreshed = false;
        public void RefreshMyInfo()
        {
            _hasRefreshed = true;
            if (CommonData.MyUser == null)
            {
                MyCharacter.ClearToEmpty();

                LblNickname.text = null;
                LblLevel.text = null;
                LblExp.text = null;
                SldExp.value = 0;
                LblAttack.text = null;
                LblDefense.text = null;
                LblEnergy.text = null;
            }
            else
            {
                MyCharacter.CharacterCode = CommonData.MyUser.CharacterCode;
                MyCharacter.Refresh();
                
                var sprs = MyCharacter.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var spriteRenderer in sprs)
                {
                    spriteRenderer.sortingLayerName = "Foreground";
                }
                MyCharacter.TakeOffAllEquip();

                LblNickname.text = CommonData.MyUser.Nickname;
                LblLevel.text = string.Format("{0}", CommonData.MyUser.Level);
                LblExp.text = string.Format("{0}/{1}", CommonData.MyUser.Exp, CommonData.MyUser.ExpCeil);
                SldExp.value = 1f*CommonData.MyUser.Exp/CommonData.MyUser.ExpCeil;

                var attack = 0;
                var health = CommonData.MyUser.RoundInitHealth;
                var equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
                if (equipConfig != null)
                {
                    var myUserCharacter =
                        CommonData.MyCharacterList.Find(x => x.CharacterCode == CommonData.MyUser.CharacterCode);
                    if (myUserCharacter != null)
                    {
                        foreach (var equip in myUserCharacter.WearEquipList.Select(equipCode => equipConfig.EquipList.Find(x => x.EquipCode == equipCode)).Where(equip => equip != null))
                        {
                            attack += (int)equip.AttackAdd;
                            health += (int)equip.HealthAdd;
                            MyCharacter.WearEquip(equip.Type, equip.EquipCode);//角色穿装备
                        }
                    }
                }
                LblAttack.text = attack.ToString();
                LblDefense.text = health.ToString();

                SprEnergy.fillAmount = 0.618f;
                LblEnergy.text = CommonData.MyUser.EnergyCapacity.ToString();
            }

            NtcMail.Number = CommonData.MailList != null ? CommonData.MailList.Count(x => !x.IsRead) : 0;
        }

        public void OnSkillClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_SKILL,
                                   new Dictionary<string, object> { { "level", CommonData.MyUser.Level } });

            ManageSkillPanel.Load();
        }
        public void OnGardenClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_CAIYUAN,
                                   new Dictionary<string, object> { { "level", CommonData.MyUser.Level } });

            GardenPanel.Load();
        }

        public void OnEquipmentClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_EQUIP,
                                   new Dictionary<string, object> {{"level", CommonData.MyUser.Level}});

            EquipPanel.Load();
        }
        public void OnAdventureClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_ADVENTURE, null);

            StartCoroutine(_GotoPushLevel());
        }
        IEnumerator _GotoPushLevel()
        {
            MainRoot.Goto(MainRoot.UIStateName.PushLevel);
            while (!PushLevelUI.Instance)
            {
                yield return new WaitForEndOfFrame();
            }
            var unlocks = CommonData.ChallengeUnlockInfoList.Where(x => x.Unlocked);
            MajorLevelUnlockInfo max = null;
            foreach (var majorLevelUnlockInfo in unlocks)
            {
                if (max == null || majorLevelUnlockInfo.MajorLevelId > max.MajorLevelId) max = majorLevelUnlockInfo;
            }
            //PushLevelUI.Instance.EnterMajorLevel(max);
            if (max != null && max.MajorLevelId > 1)
            {
                PushLevelUI.Instance.Planet.GoToIndex(max.MajorLevelId - 1);
            }
        }
        public void OnRealTimeFightingClick()
        {
            CommonData.RivalUser = null;
            GameData.LastChallengeID = null;
            Requester.Instance.Send(new NewMatch());//发送消息
            MainRoot.Goto(MainRoot.UIStateName.Match);
            MatchUI.Instance.RefreshBeforeMatch();

            UMengPlugin.UMengEvent(EventId.HOME_MULTI,null);
        }
        public void OnLeaderboardClick()
        {
            LeaderboardPanel.Load();
            UMengPlugin.UMengEvent(EventId.HOME_RANK,null);
        }
        public void OnSettingsClick()
        {
            SettingsPanel.Load();
            UMengPlugin.UMengEvent(EventId.HOME_SETTING, new Dictionary<string, object> { { "exp", CommonData.MyUser.Exp } });
        }

        public void OnMailBoxClick()
        {   
            MailBoxPanel.Load();
            UMengPlugin.UMengEvent(EventId.HOME_MAIL,
                                   new Dictionary<string, object>
                                       {
                                           {"all", CommonData.MailList.Count},
                                           {"unread", CommonData.MailList.Count(x => !x.IsRead)}
                                       });
        }

        public void OnShopClick()
        {
            UMengPlugin.UMengEvent(EventId.HOME_SHOP, null);

            ShopPanel.Load();
            if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.Recharge);
        }

        public void Execute(UserMailList cmd)
        {
            NtcMail.Number = CommonData.MailList != null ? CommonData.MailList.Count(x => !x.IsRead) : 0;
        }
    }
}