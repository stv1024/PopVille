using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using Assets.Sanxiao.UI.Panel.Shop;
using Fairwood.Math;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel
{
    public class ShopPanel : BaseTempSingletonPanel
    {
        #region 单例面板通用

        private static ShopPanel _instance;

        public static ShopPanel Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance && value)
                {
                    Debug.LogError("more than 1 ShopPanel instance now!");
                    Destroy(_instance.gameObject);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
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
                var go = Resources.Load("UI/ShopPanel") as GameObject;
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

        public enum ShopState
        {
            Recharge,
            ExchangeMoney10,
            ExchangeHeart
        }

        private ShopState _state;

        public GameObject BtnHeart, BtnCoin, BtnDiamond;
        public UISprite SprHeart, SprCoin, SprDiamond;

        public GameObject ExchangeSlotTemplate;
        public GameObject RechargeSlotTemplate;

        public UIGrid Grid;

        private readonly List<ShopExchangeSlot> _shopExchangeSlotList = new List<ShopExchangeSlot>();
        private readonly List<ShopRechargeSlot> _shopRechargeSlotList = new List<ShopRechargeSlot>();

        public void RefreshToState(ShopState state)
        {
            _state = state;
            BtnHeart.GetComponent<Collider>().enabled = (_state != ShopState.ExchangeHeart);
            BtnCoin.GetComponent<Collider>().enabled = (_state != ShopState.ExchangeMoney10);
            BtnDiamond.GetComponent<Collider>().enabled = (_state != ShopState.Recharge);

            SprHeart.enabled = (_state == ShopState.ExchangeHeart);
            SprCoin.enabled = (_state == ShopState.ExchangeMoney10);
            SprDiamond.enabled = (_state == ShopState.Recharge);

            foreach (var slot in _shopExchangeSlotList)
            {
                slot.gameObject.SetActive(false);
            }
            foreach (var slot in _shopRechargeSlotList)
            {
                slot.gameObject.SetActive(false);
            }
            switch (_state)
            {
                case ShopState.Recharge:
                    var rechargeConfig =
                        ConfigManager.GetConfig(ConfigManager.ConfigType.RechargeConfig) as RechargeConfig;
                    if (rechargeConfig != null)
                    {
                        for (int i = 0; i < rechargeConfig.RechargeList.Count; i++)
                        {
                            while (i >= _shopRechargeSlotList.Count)
                            {
                                _shopRechargeSlotList.Add(null);
                            }
                            if (_shopRechargeSlotList[i] == null)
                            {
                                var go = PrefabHelper.InstantiateAndReset(RechargeSlotTemplate, Grid.transform);
                                go.name = "Recharge " + i;
                                _shopRechargeSlotList[i] = go.GetComponent<ShopRechargeSlot>();
                            }
                            if (_shopRechargeSlotList[i])
                            {
                                _shopRechargeSlotList[i].gameObject.SetActive(true);
                                _shopRechargeSlotList[i].SetAndRefresh(rechargeConfig.RechargeList[i]);
                            }
                            else
                            {
                                Debug.LogError("出错了");
                            }
                        }
                    }
                    break;
                case ShopState.ExchangeMoney10:
                case ShopState.ExchangeHeart:
                    List<Exchange> curTypeExchangeList = null;

                    var exchangeConfig =
                        ConfigManager.GetConfig(ConfigManager.ConfigType.ExchangeConfig) as ExchangeConfig;
                    if (exchangeConfig != null)
                    {
                        if (_state == ShopState.ExchangeHeart)
                        {
                            curTypeExchangeList =
                                exchangeConfig.ExchangeList.Where(x => x.ToType == (int)CurrencyType.Heart).ToList();
                        }
                        else if (_state == ShopState.ExchangeMoney10)
                        {
                            curTypeExchangeList =
                                exchangeConfig.ExchangeList.Where(x => x.ToType == (int)CurrencyType.Coin).ToList();
                        }
                    }
                    if (curTypeExchangeList != null)
                    {
                        for (int i = 0; i < curTypeExchangeList.Count; i++)
                        {
                            while (i >= _shopExchangeSlotList.Count)
                            {
                                _shopExchangeSlotList.Add(null);
                            }
                            if (_shopExchangeSlotList[i] == null)
                            {
                                var go = PrefabHelper.InstantiateAndReset(ExchangeSlotTemplate, Grid.transform);
                                go.name = "Exchange " + i;
                                _shopExchangeSlotList[i] = go.GetComponent<ShopExchangeSlot>();
                            }
                            if (_shopExchangeSlotList[i])
                            {
                                _shopExchangeSlotList[i].gameObject.SetActive(true);
                                _shopExchangeSlotList[i].SetAndRefresh(curTypeExchangeList[i]);
                            }
                            else
                            {
                                Debug.LogError("出错了");
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ExchangeSlotTemplate.SetActive(false);
            RechargeSlotTemplate.SetActive(false);
            Grid.repositionNow = true;
        }

        public void OnCoinTabClick()
        {
            RefreshToState(ShopState.ExchangeMoney10);
        }

        public void OnDiamondTabClick()
        {
            RefreshToState(ShopState.Recharge);
        }

        public void OnHeartTabClick()
        {
            RefreshToState(ShopState.ExchangeHeart);
        }

        public void OnHelpClick()
        {
            string text = null;
            switch (_state)
            {
                case ShopState.Recharge:
                    text = @"在商店，您可以用钻石兑换大量金币和爱心。
充值可以得到大量钻石。
充值遇到问题，可以联系客服：
QQ群：123***241
也可以直接点击屏幕下方[未到账查询]按钮";//客服信息
                    break;
            }
            if (text != null) MorlnTooltip.ShowCentered(text);
        }
    }
}