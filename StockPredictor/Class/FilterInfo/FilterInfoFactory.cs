using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPredictCore.AverageLine;
using StockPredictCore.Filter;
using StockPredictCore.Filter.AverageLine;
using StockPredictCore.Filter.ValueDiff;
using StockPredictCore.Filter.Volumn;
using StockPredictCore.ValueDiff;

namespace StockPredictor.Class.FilterInfo
{
    public static class FilterInfoFactory
    {
        public static FilterInfo CreatFilterInfoByFilterType(FilterType type)
        {
            FilterInfo info = new FilterInfo();
            info.Type = type;
            switch (type)
            {
                case FilterType.Ma5IncreaseFilter:
                    info.Param = new Ma5IncreaseFilter().GetParam();
                    break;
                case FilterType.Ma5DecreaseFilter:
                    info.Param = new Ma5DecreaseFilter().GetParam();
                    break;
                case FilterType.Ma5LMa20Filter:
                    info.Param = new Ma5LMa20Filter().GetParam();
                    break;
                case FilterType.Ma5HMa20Filter:
                    info.Param = new Ma5HMa20Filter().GetParam();
                    break;
                case FilterType.Ma5LMa60Filter:
                    info.Param = new Ma5LMa60Filter().GetParam();
                    break;
                case FilterType.Ma5HMa60Filter:
                    info.Param = new Ma5HMa60Filter().GetParam();
                    break;



                case FilterType.PriceDecreaseFilter:
                    info.Param = new PriceDecreaseFilter().GetParam();
                    break;
                case FilterType.PriceIncreaseFilter:
                    info.Param = new PriceIncreaseFilter().GetParam();
                    break;




                case FilterType.VolumnMoreThanFilter:
                    info.Param = new VolumnMoreThanFilter().GetParam();
                    break;
                case FilterType.VolumnLessThanFilter:
                    info.Param = new VolumnLessThanFilter().GetParam();
                    break;
                case FilterType.VolumnIncreaseFilter:
                    info.Param = new VolumnIncreaseFilter().GetParam();
                    break;
                case FilterType.VolumnDecreaseFilter:
                    info.Param = new VolumnDecreaseFilter().GetParam();
                    break;

              
            } 
            return info; 
        }
    }
}
