using UnityEngine;

namespace Assets.Sanxiao.UI.Panel.Shop
{
    public abstract class BaseShopSlot : MonoBehaviour
    {
        public UISprite SprIcon;
        public UILabel LblSource;
        public UILabel LblDisplayName;

        public abstract void OnBuyClick();
    }
}