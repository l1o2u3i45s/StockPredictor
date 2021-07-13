using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StockPredictor.Class
{
    public class FilterInfo:ObservableObject
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

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set { Set(() => IsSelected, ref isSelected, value); }
        }
     

        private double param;

        public double Param
        {
            get => param;
            set { Set(() => Param, ref param, value); }
        }
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
        [Description("Ma5比昨天漲了幾%內")]
        Ma5IncreaseFilter,
        [Description("Ma5比昨天跌了幾%內")]
        Ma5DecreaseFilter,
        [Description("Ma5小於Ma20幾%以上")]
        Ma5LMa20Filter,
        [Description("Ma5小於Ma60幾%以上")]
        Ma5LMa60Filter,



        [Description("股價跌幾%以上")]
        PriceDecreaseFilter,
        [Description("股價漲幾%以上")]
        PriceIncreaseFilter,



        [Description("成交量大於幾張")]
        VolumnMoreThanFilter,
        [Description("成交量小於幾張")]
        VolumnLessThanFilter,
        [Description("成交量比昨日增加幾倍")]
        VolumnIncreaseFilter

    }

}
