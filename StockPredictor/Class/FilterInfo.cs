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
      

        public string TypeName
        {
            get => Type.GetDescriptionText();
        }

          

        public FilterType Type { get; set; }

        public bool IsSelected { get; set; }
    }


    public enum FilterCategory
    {
        [Description("均線")]
        AverageLine,
        [Description("股價")]
        Value, 
        [Description("成交量")]
        Volumn,
       
    }

    public enum FilterType
    {
        [Description("Ma5上升")]
        Ma5IncreaseFilter,
        [Description("Ma5小於Ma20幾%")]
        Ma5LMa20Filter,
        [Description("Ma5小於Ma60幾%")]
        Ma5LMa60Filter,



        [Description("股價跌幾%")]
        PriceDecreaseFilter,
        [Description("股價漲幾%")]
        PriceIncreaseFilter,



        [Description("成交量大於幾張")]
        VolumnMoreThanFilter,
        [Description("成交量小於幾張")]
        VolumnLessThanFilter,
        [Description("成交量比昨日增加幾倍")]
        VolumnIncreaseFilter

    }

}
