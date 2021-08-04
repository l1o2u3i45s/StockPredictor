using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPredictCore.AverageLine;
using StockPredictCore.Filter;
using StockPredictCore.Filter.AverageLine;
using StockPredictCore.Filter.RSI;
using StockPredictCore.Filter.Value;
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
                case FilterType.Ma5LMa120Filter:
                    info.Param = new Ma5LMa120Filter().GetParam();
                    break;
                case FilterType.Ma5HMa120Filter:
                    info.Param = new Ma5HMa120Filter().GetParam();
                    break;
                case FilterType.Ma60LowerPriceFilter:
                    info.Param = new Ma60LowerPriceFilter().GetParam();
                    break;

                    
                case FilterType.RSI5DecreaseFilter:
                    info.Param = new RSI5DecreaseFilter().GetParam();
                    break;
                case FilterType.RSI5HRSI10Filter:
                    info.Param = new RSI5HRSI10Filter().GetParam();
                    break;
                case FilterType.RSI5IncreaseFilter:
                    info.Param = new RSI5IncreaseFilter().GetParam();
                    break;
                case FilterType.RSI5LowerThanValueFilter:
                    info.Param = new RSI5LowerThanValueFilter().GetParam();
                    break;
                case FilterType.RSI5LRSI10Filter:
                    info.Param = new RSI5LRSI10Filter().GetParam();
                    break;
                case FilterType.RSIHigherThanValueFilter:
                    info.Param = new RSIHigherThanValueFilter().GetParam();
                    break;



                case FilterType.PriceDecreaseFilter:
                    info.Param = new PriceDecreaseFilter().GetParam();
                    break;
                case FilterType.PriceIncreaseFilter:
                    info.Param = new PriceIncreaseFilter().GetParam();
                    break;
                case FilterType.PriceMostHighInDaysFilter:
                    info.Param = new PriceMostHighInDaysFilter().GetParam();
                    break;
                case FilterType.PriceMostLowinDaysFilter:
                    info.Param = new PriceMostLowinDaysFilter().GetParam();
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
                default:
                    throw new NotImplementedException();
                

            } 
            return info; 
        }
    }
}
