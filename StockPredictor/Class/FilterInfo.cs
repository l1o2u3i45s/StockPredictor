using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictor.Class
{
    public class FilterInfo
    {
        public FilterInfo()
        {
            IsSelected = false;
        }
        public FilterType Type { get; set; }

        public string Name
        {
            get => Type.GetDescriptionText();
        }

        public bool IsSelected { get; set; }
    }


    public enum FilterType
    {
        [Description("MA5跟MA20重疊")]
        Ma20_Ma5_Filter,
        [Description("MA5跟MA60重疊")]
        Ma60_Ma5_Filter, 
        [Description("MA5小於MA20跟MA60且MA5 Tan = 0")]
        MA5low20low60AndTanis0,
        [Description("MA5向上強烈")]
        Ma5Increase,
        [Description("交易量爆增")]
        VolumnIncreaseMore,
        [Description("交易量超過3000")]
        VolumnThresholdFilter,
        [Description("股票收紅")]
        ClosedMoreThanOpen,
        [Description("收盤低於前高")]
        LowThanLastHighestPrice,
    }


}
