using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using StockPredictCore.Filter;

namespace StockPredictor.Class.FilterInfo
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
    

}
