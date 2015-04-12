using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel.Equip;
using Assets.Scripts;
using Fairwood.Math;
using UnityEngine;
using BuyCharacter = Assets.Sanxiao.Communication.UpperPart.BuyCharacter;
using BuyCharacterOk = Assets.Sanxiao.Communication.UpperPart.BuyCharacterOk;
using ChangeCharacter = Assets.Sanxiao.Communication.UpperPart.ChangeCharacter;

namespace Assets.Sanxiao.UI.Panel
{
    public class EquipPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static EquipPanel _instance;

        public static EquipPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 EquipPanel instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;

            RefreshData();
        }

        protected override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }

        private static GameObject Prefab
        {
            get
            {
                var go = Resources.Load("UI/EquipPanel") as GameObject;
                return go;
            }
        }

        public static void Load()
        {
            if (Instance)
            {
                MainRoot.FocusPanel(Instance);
            }
            else
            {
                if (!Prefab) return;
                MainRoot.ShowPanel(Prefab);
            }
            if (Instance) Instance.Initialize();
        }

        public static void UnloadInterface()
        {
            if (Instance) Instance.OnConfirmClick();
        }

        #endregion

        private EquipConfig _equipConfig;
        private CharacterConfig _characterConfig;

        private UserCharacter _userCharacter;

        public Character MyCharacter;

        private readonly List<int> _characterList = new List<int>();

        protected override void Initialize()
        {
            Refresh();
        }

        private bool _hasInitialized;
        void Start()
        {
            if (_hasInitialized) return;//如果外部调用时直接指定了装备type，就不要再刷新成默认了
            _hasInitialized = true;
            BottomRefresh(0);//TODO:记住上一次的位置
        }

        void RefreshData()
        {
            _characterList.Clear();
            _equipConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.EquipConfig) as EquipConfig;
            _characterConfig = ConfigManager.GetConfig(ConfigManager.ConfigType.CharacterConfig) as CharacterConfig;
            if (_equipConfig == null || _characterConfig == null) MorlnFloatingToast.Create("数据错误");
            else
            {
                _characterList.AddRange(CommonData.MyCharacterList.Select(x => x.CharacterCode));
                _characterList.AddRange(
                    _characterConfig.CharacterList.Select(x => x.CharacterCode)
                                    .Where(x => !CommonData.MyCharacterList.Exists(y => y.CharacterCode == x)));
            }
        }

        public void Refresh()
        {
            if (CommonData.MyCharacterList.Count <= 0)
            {
                Debug.LogError("怎么能没有角色。CommonData.MyCharacterList.Count == 0");
                return;
            }

            if (_equipConfig == null) return;

            UpperSetAndRefresh(_index);

            EquipSlotTemplate.SetActive(false);
        }

        public GameObject BtnLeft, BtnRight;

        /// <summary>
        /// 在MyCharacterList里的index，确保正确
        /// </summary>
        private int _index;
        public int CurCharacterCode
        {
            get { return _characterList[_index]; }
        }

        private CharacterIntroTextConfig _characterIntroTextConfig;

        public void GotoPreviousCharacter()
        {
            UMengPlugin.UMengEvent(EventId.SWITCH_CHARACTER_VIEW,
                                   new Dictionary<string, object> {{"from_ind", _index}, {"to_ind", (_index-1)}});//发送统计事件
            if (_index <= 0)
            {
                BtnLeft.SetActive(false);
                return;
            }
            _index--;
            UpperSetAndRefresh(_index);
        }
        public void GotoNextCharacter()
        {
            UMengPlugin.UMengEvent(EventId.SWITCH_CHARACTER_VIEW,
                                   new Dictionary<string, object> { { "from_ind", _index }, { "to_ind", (_index+1) } });//发送统计事件
            if (_index >= _characterList.Count - 1)
            {
                BtnRight.SetActive(false);
                return;
            }
            _index++;
            UpperSetAndRefresh(_index);
        }

        public GameObject GrpUnlockedCharacter, GrpLockedCharacter;

        public UILabel LblCharacterDescription, LblCharacterTalent;

        public UISprite[] SprEquips;
        public Transform Focus;
        public MorlnUIButtonScale BtnChuzhan;
        public UISprite Tick;
        public UILabel LblLevel, LblExp;
        public UISlider SldExp;
        public UILabel LblAttack, LblDefense;

        private void UpperSetAndRefresh(int index)
        {
            LblLevel.text = string.Format("{0}", CommonData.MyUser.Level);
            LblExp.text = string.Format("{0}/{1}", CommonData.MyUser.Exp, CommonData.MyUser.ExpCeil);
            SldExp.value = 1f * CommonData.MyUser.Exp / CommonData.MyUser.ExpCeil;

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


            if (_characterList.Count == 0)
            {
                Debug.LogError("_characterList.Count == 0 ERROR");
                return;
            }
            if (_equipConfig == null) return;

            _index = Mathf.Clamp(index, 0, _characterList.Count - 1);
            MyCharacter.CharacterCode = CurCharacterCode;
            MyCharacter.Refresh();
            MyCharacter.TakeOffAllEquip();
            var sprs = MyCharacter.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (var spriteRenderer in sprs)
            {
                spriteRenderer.sortingLayerName = "Foreground";
            }
            var curUserCharacter = CommonData.MyCharacterList.Find(x => x.CharacterCode == CurCharacterCode);

            if (curUserCharacter == null) //未解锁
            {
                GrpLockedCharacter.SetActive(true);
                GrpUnlockedCharacter.SetActive(false);

                if (_characterIntroTextConfig == null)
                {
                    _characterIntroTextConfig =
                        ConfigManager.GetConfig(ConfigManager.ConfigType.CharacterIntroTextConfig) as
                        CharacterIntroTextConfig;
                }
                if (_characterIntroTextConfig != null)
                {
                    var intro = _characterIntroTextConfig.IntroList.Find(x => x.CharacterCode == CurCharacterCode);
                    if (intro != null)
                    {
                        LblCharacterDescription.text = intro.Description;
                        LblCharacterTalent.text = intro.TalentIntro;
                    }
                    else
                    {
                        Debug.LogError("没有CharacterIntro。Code:" + CurCharacterCode);
                        LblCharacterDescription.text = null;
                        LblCharacterTalent.text = null;
                    }
                }
                else
                {
                    Debug.LogError("没有CharacterIntroTextConfig");
                    LblCharacterDescription.text = null;
                    LblCharacterTalent.text = null;
                }
            }
            else//已解锁
            {
                GrpUnlockedCharacter.SetActive(true);
                GrpLockedCharacter.SetActive(false);

                //出战按钮
                if (CommonData.MyUser.CharacterCode == CurCharacterCode)
                {
                    BtnChuzhan.isEnabled = false;
                    Tick.enabled = true;
                }
                else
                {
                    BtnChuzhan.isEnabled = true;
                    Tick.enabled = false;
                }

                for (int type = 0; type < 5; type++)
                {
                    SprEquips[type].enabled = false;
                }
                foreach (var code in curUserCharacter.WearEquipList)
                {
                    var equip = _equipConfig.EquipList.Find(x => x.EquipCode == code);

                    if (equip == null)
                    {
                        Debug.LogError("找不到配置Equip:" + code);
                        continue;
                    }

                    var type = equip.Type;
                    SprEquips[type].atlas =
                        MorlnDownloadResources.Load<UIAtlas>("ResourcesForDownload/Equip/EquipIcon/Atlas-EquipIcons");
                    var sprName = EquipUtil.GetEquipSpriteName(code, type);
                    //var spr = Resources.Load<Sprite>("Sprites/Equip/" + sprName);
                    SprEquips[type].enabled = true;
                    //SprEquips[type].sprite = spr;
                    SprEquips[type].spriteName = sprName;

                    MyCharacter.WearEquip(type, code);
                }
            }

            BtnLeft.SetActive(_index > 0);
            BtnRight.SetActive(_index < _characterList.Count - 1);
        }
        
        public void OnChuzhanClick()
        {
            UMengPlugin.UMengEvent(EventId.USE_CHARACTER, new Dictionary<string, object> { { "code", CurCharacterCode } });//发送统计事件
            Requester.Instance.Send(new ChangeCharacter(CurCharacterCode));
        }

        public void GotoHelmetView()
        {
            BottomRefresh(0);
        }
        public void GotoArmorView()
        {
            BottomRefresh(1);
        }
        public void GotoWeaponView()
        {
            BottomRefresh(2);
        }
        public void GotoShieldView()
        {
            BottomRefresh(3);
        }
        public void GotoShoesView()
        {
            BottomRefresh(4);
        }

        public UIGrid Grid;
        public GameObject EquipSlotTemplate;
        private readonly List<EquipSlot> _equipSlotList = new List<EquipSlot>();

        public void BottomRefresh(int equipType)
        {
            if (!_hasInitialized) _hasInitialized = true;//Initialize干的就是这个

            if (0 <= equipType && equipType < SprEquips.Length)
            {
                //Focus.position = SprEquips[equipType].transform.position;//焦点框
                SpringPosition.Begin(Focus.gameObject,
                                     Focus.parent.InverseTransformPoint(SprEquips[equipType].transform.position), 10);//焦点框
            }

            if (_equipConfig == null) return;

            var curList = _equipConfig == null
                              ? new List<Communication.Proto.Equip>()
                              : _equipConfig.EquipList.Where(
                                  x =>
                                  x.Type == equipType && CommonData.MyEquipList.Exists(y => y.EquipCode == x.EquipCode))
                                            .ToList();
            var slotCount = Mathf.Max(10, (curList.Count + 1)/2*2);
            while (_equipSlotList.Count < slotCount)
            {
                var slot = PrefabHelper.InstantiateAndReset<EquipSlot>(EquipSlotTemplate, Grid.transform);
                slot.name = "EquipSlot " + _equipSlotList.Count;
                slot.EquipPanel = this;
                slot.gameObject.SetActive(true);
                _equipSlotList.Add(slot);
            }

            for (int i = 0; i < _equipSlotList.Count; i++)
            {
                if (i < curList.Count)
                {
                    _equipSlotList[i].gameObject.SetActive(true);
                    var equip = curList[i];
                    var curCharacter = CommonData.MyCharacterList[_index];
                    var userEquip = CommonData.MyEquipList.Find(x => x.EquipCode == equip.EquipCode);
                    _equipSlotList[i].SetAndRefresh(curList[i], curCharacter.WearEquipList.Contains(equip.EquipCode), userEquip.Count);
                }
                else
                {
                    _equipSlotList[i].SetAndRefresh(null, false, 0);
                }
            }

            EquipSlotTemplate.SetActive(false);
        }

        public void OnBuyUnlockClick()
        {
            UMengPlugin.UMengEvent(EventId.BUY_CHARACTER, new Dictionary<string, object> {{"code", CurCharacterCode}});//发送统计事件
            Requester.Instance.Send(new BuyCharacter(CurCharacterCode));
        }

        public void Execute(BuyCharacterOk cmd)
        {
            RefreshData();
            _index = _characterList.FindIndex(x => x == cmd.NewCharacter.CharacterCode);
            if (_index < 0) _index = 0;
            Refresh();
        }
    }
}