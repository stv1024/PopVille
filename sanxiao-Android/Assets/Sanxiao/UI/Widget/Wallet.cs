using Assets.Sanxiao;
using Assets.Sanxiao.UI.Panel;
using UnityEngine;

/// <summary>
/// 钱包控件，自行监听数值，附有进入商店按钮。可根据实际情况删除衬底，删除碰撞器
/// </summary>
public class Wallet : MonoBehaviour
{

    public UILabel LblCoin, LblDiamond;

    private long? _coin, _diamond;

    private void Update()
    {
        long? num;

        num = CommonData.MyUser != null ? (long?) CommonData.MyUser.Money10 : null;
        if (num != _coin)
        {
            _coin = num;
            LblCoin.text = _coin != null ? string.Format("{0}", _coin) : null;
        }

        num = CommonData.MyUser != null ? (long?) CommonData.MyUser.Money1 : null;
        if (num != _diamond)
        {
            _diamond = num;
            LblDiamond.text = _diamond != null ? string.Format("{0}", _diamond) : null;
        }
    }

    public void OnBuyCoinClick()
    {
        ShopPanel.Load();
        if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.ExchangeMoney10);
    }

    public void OnBuyDiamondClick()
    {
        ShopPanel.Load();
        if (ShopPanel.Instance != null) ShopPanel.Instance.RefreshToState(ShopPanel.ShopState.Recharge);
    }
}