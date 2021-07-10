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
        [Description("交易量爆增")]
        VolumnIncreaseMore
    }


}
