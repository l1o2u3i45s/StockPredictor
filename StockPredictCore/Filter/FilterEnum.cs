using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore.Filter
{
    public enum FilterType
    {
        [Description("Ma5比昨天漲了幾%上")]
        Ma5IncreaseFilter,
        [Description("Ma5比昨天跌了幾%上")]
        Ma5DecreaseFilter,
        [Description("Ma5小於Ma20幾%以上")]
        Ma5LMa20Filter,
        [Description("Ma5大於Ma20幾%以上")]
        Ma5HMa20Filter,
        [Description("Ma5小於Ma60幾%以上")]
        Ma5LMa60Filter,
        [Description("Ma5大於Ma60幾%以上")]
        Ma5HMa60Filter,
        [Description("Ma5小於Ma120幾%以上")]
        Ma5LMa120Filter,
        [Description("Ma5大於Ma120幾%以上")]
        Ma5HMa120Filter,
        [Description("Ma60小於收盤價幾%以內")]
        Ma60LowerPriceFilter,


        [Description("股價跌幾%以上")]
        PriceDecreaseFilter,
        [Description("股價漲幾%以上")]
        PriceIncreaseFilter,
        [Description("股價N日內最高")]
        PriceMostHighInDaysFilter,
        [Description("股價N日內最低")]
        PriceMostLowinDaysFilter,

        [Description("成交量大於幾張")]
        VolumnMoreThanFilter,
        [Description("成交量小於幾張")]
        VolumnLessThanFilter,
        [Description("成交量比昨日增加幾倍")]
        VolumnIncreaseFilter,
        [Description("成交量比昨日減少幾倍")]
        VolumnDecreaseFilter,
        [Description("成交量N日內最高")]
        VolumnMostHighInDayFilter,
        [Description("成交量N日內最低")]
        VolumnMostLowInDayFilter,


        [Description("RSI5比昨天少幾%")]
        RSI5DecreaseFilter,
        [Description("RSI5比昨天多幾%")]
        RSI5IncreaseFilter,
        [Description("RSI5大於RSI10幾%")]
        RSI5HRSI10Filter,
        [Description("RSI5小於RSI10幾%")]
        RSI5LRSI10Filter,
        [Description("RSI5小於多少")]
        RSI5LowerThanValueFilter,
        [Description("RSI5大於多少")]
        RSIHigherThanValueFilter,




        [Description("特殊算法_RSI5回測買進")]
        Other_RSI5L20_HigherThanLastLowFilter
    }
}
