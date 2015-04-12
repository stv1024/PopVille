using System.Collections.Generic;
using Assets.Sanxiao.Communication.Proto;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.UI.Panel.Shop
{
    /// <summary>
    /// 商店里，充值包大按钮
    /// </summary>
    public class ShopRechargeSlot : BaseShopSlot
    {
        private Recharge _recharge;

        public void SetAndRefresh(Recharge recharge)
        {
            _recharge = recharge;

            LblSource.text = string.Format("￥{0:0.00}", recharge.Price);
            LblDisplayName.text = new Currency(recharge.TargetType, recharge.TargetAmount).GetCurrencyLabelWithIcon();
        }

        public override void OnBuyClick()
        {
            UMengPlugin.UMengEvent(EventId.BUY_DIAMOND, new Dictionary<string, object> {{"name", _recharge.Name}});

            AlertDialog.Load("充值功能尚未开放");
        }
    }
}