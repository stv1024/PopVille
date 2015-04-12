using System;
using System.Collections.Generic;
using Assets.Sanxiao;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

/// <summary>
/// 一些到处都能用到的按钮
/// </summary>
public class CommonButton : MonoBehaviour
{
    public enum ButtonTypeEnum
    {
        BuyHeart,
        BuyCoin,
        BuyDiamond
    }

    public ButtonTypeEnum ButtonType;

    private void OnClick()
    {
        switch (ButtonType)
        {
            case ButtonTypeEnum.BuyHeart:

                UMengPlugin.UMengEvent(EventId.GOTO_HEART,
                                       new Dictionary<string, object>
                                           {
                                               {"from", MainRoot.Instance.CurrentViewStateName},
                                               {"heart", CommonData.HeartData.Count}
                                           });

                ShopPanel.Load();
                if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.ExchangeHeart);
                break;
            case ButtonTypeEnum.BuyCoin:

                UMengPlugin.UMengEvent(EventId.GOTO_COIN,
                                       new Dictionary<string, object>
                                           {
                                               {"from", MainRoot.Instance.CurrentViewStateName},
                                               {"coin", CommonData.MyUser.Money10}
                                           });

                ShopPanel.Load();
                if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.ExchangeMoney10);
                break;
            case ButtonTypeEnum.BuyDiamond:

                UMengPlugin.UMengEvent(EventId.GOTO_DIAMOND,
                                       new Dictionary<string, object>
                                           {
                                               {"from", MainRoot.Instance.CurrentViewStateName},
                                               {"diamond", CommonData.MyUser.Money1}
                                           });

                ShopPanel.Load();
                if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.Recharge);
                break;
            default:
                Debug.LogError("超出范围", this);
                break;
        }
    }
}