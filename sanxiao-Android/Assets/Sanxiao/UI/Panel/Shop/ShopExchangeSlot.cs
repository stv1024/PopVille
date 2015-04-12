using System.Collections.Generic;
using Assets.Sanxiao.Communication;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Communication.UpperPart;
using Assets.Sanxiao.Data;
using UnityEngine;
using RequestExchange = Assets.Sanxiao.Communication.UpperPart.RequestExchange;

namespace Assets.Sanxiao.UI.Panel.Shop
{
    /// <summary>
    /// 商店里，兑换包大按钮
    /// </summary>
    public class ShopExchangeSlot : BaseShopSlot
    {
        private Exchange _exchange;

        public void SetAndRefresh(Exchange exchange)
        {
            _exchange = exchange;

            //if (_exchange.ToType == (int) CurrencyType.Coin) SprFrame.spriteName = "金币兑换包";
            //else if (_exchange.ToType == (int)CurrencyType.Heart) SprFrame.spriteName = "爱心兑换包";

            LblSource.text = new Currency(exchange.FromType, exchange.FromAmount).GetCurrencyLabelWithIcon();
            LblDisplayName.text = new Currency(exchange.ToType, exchange.ToAmount).GetCurrencyLabelWithIcon();
        }

        public override void OnBuyClick()
        {
            UMengPlugin.UMengEvent(
                (CurrencyType) _exchange.ToType == CurrencyType.Coin ? EventId.BUY_COIN : EventId.BUY_HEART,
                new Dictionary<string, object> {{"name", _exchange.Name}});

            Requester.Instance.Send(new RequestExchange(_exchange.Name, 1));
        }
    }
}