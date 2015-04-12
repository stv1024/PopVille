using System;
using Assets.Sanxiao.Data;
using UnityEngine;

namespace Assets.Sanxiao.UI.Widget
{
    /// <summary>
    /// 用于显示数据
    /// </summary>
    [RequireComponent(typeof(UILabel))]
    public class DataLabel : MonoBehaviour
    {
        public CurrencyType Data;

        private UILabel _label;
        UILabel Label { get { return _label ?? (_label = GetComponent<UILabel>()); } }

        private long? _num;

        void Awake()
        {
            Update();
        }
        void Update()
        {
            long? num = null;
            switch (Data)
            {
                case CurrencyType.Coin:
                    if (CommonData.MyUser != null) num = CommonData.MyUser.Money10;
                    break;
                case CurrencyType.Diamond:
                    if (CommonData.MyUser != null) num = CommonData.MyUser.Money1;
                    break;
            }
            if (num != _num)
            {
                _num = num;
                Label.text = _num != null ? string.Format("{0}", _num) : null;
            }
        }

        //void RefreshMoneyCoin(int amount)
        //{
        //    if (Data != DataEnum.MoneyCoin) return;
        //    Label.text = string.Format("{0}", amount);
        //}
        //void RefreshMoneyDiamond(int amount)
        //{
        //    if (Data != DataEnum.MoneyDiamond) return;
        //    Label.text = string.Format("{0}", amount);
        //}
    }
}