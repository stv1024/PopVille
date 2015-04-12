using System;
using Assets.Sanxiao.Communication.Proto;

namespace Assets.Sanxiao.Data
{
    public enum CurrencyType
    {
        Diamond = 1,
        Coin = 10,
        Exp = 100,
        Heart = 1000,
        PkCount
    }

    public static class CurrencyExtension
    {
        /// <summary>
        /// 用中文表示货币种类的文本
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="inverse">货币名称在前数字在后</param>
        /// <returns></returns>
        public static string GetCurrencyLabelChinese(this Currency currency, bool inverse = false)
        {
            return string.Format(inverse ? "{1} {0}" : "{0} {1}", currency.Amount,
                                 ((CurrencyType)currency.Type).GetChineseName());
        }

        /// <summary>
        /// 用图标表示货币种类的文本
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="inverse">图标在前数字在后</param>
        /// <returns></returns>
        public static string GetCurrencyLabelWithIcon(this Currency currency, bool inverse = false)
        {
            return string.Format(inverse ? "{1}{0}" : "{0}{1}", currency.Amount,
                                 ((CurrencyType) currency.Type).GetIcon());
        }

        public static bool DoIAfford(this Currency currency)
        {
            return GetMyCurrencyAmount((CurrencyType) currency.Type) >= currency.Amount;
        }
        public static Currency GetMyCurrency(this CurrencyType currencyType)
        {
            if (CommonData.MyUser != null)
            {
                switch (currencyType)
                {
                    case CurrencyType.Diamond:
                        return new Currency((int) currencyType, CommonData.MyUser.Money1);
                    case CurrencyType.Coin:
                        return new Currency((int) currencyType, CommonData.MyUser.Money10);
                    case CurrencyType.Exp:
                        return new Currency((int) currencyType, CommonData.MyUser.Exp);
                    case CurrencyType.Heart:
                        return new Currency((int) currencyType, CommonData.HeartData.Count);
                    case CurrencyType.PkCount:
                        return new Currency((int) currencyType, CommonData.PkData.Count);
                }
            }
            return new Currency((int)currencyType, 0);
        }

        public static long GetMyCurrencyAmount(this CurrencyType currencyType)
        {
            if (CommonData.MyUser == null) return 0;
            switch (currencyType)
            {
                case CurrencyType.Diamond:
                    return CommonData.MyUser.Money1;
                case CurrencyType.Coin:
                    return CommonData.MyUser.Money10;
                case CurrencyType.Exp:
                    return CommonData.MyUser.Exp;
                case CurrencyType.Heart:
                    return CommonData.HeartData.Count;
                case CurrencyType.PkCount:
                    return CommonData.PkData.Count;
                default:
                    return 0;
            }
        }
    }

    public static class CurrencyEnumExtension
    {
        public static string GetChineseName(this CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Diamond:
                    return "钻石";
                case CurrencyType.Coin:
                    return "金币";
                case CurrencyType.Exp:
                    return "经验";
                case CurrencyType.Heart:
                    return "爱心";
                default:
                    return "";
            }
        }
        public static string GetIcon(this CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Diamond:
                    return "[diamond]";
                case CurrencyType.Coin:
                    return "[coin]";
                case CurrencyType.Exp:
                    return "[exp]";
                case CurrencyType.Heart:
                    return "[heart]";
                default:
                    return "";
            }
        }
    }
}