using System;
using System.Collections.Generic; 
using InfraStructure;
using StockPredictCore.AverageLine;
using StockPredictCore.Filter;
using StockPredictCore.Filter.AverageLine;
using StockPredictCore.Filter.RSI;
using StockPredictCore.Filter.Value;
using StockPredictCore.Filter.ValueDiff;
using StockPredictCore.Filter.Volumn;
using StockPredictCore.ValueDiff; 

namespace StockPredictor.Class
{
    public static class FilterFactory
    {
        public static IFilter CreatFilterByFilterType(FilterType _type,double param, IEnumerable<StockData> datas)
        {
            switch (_type)
            {
                case FilterType.Ma5IncreaseFilter:
                   return new Ma5IncreaseFilter(datas, param);
                case FilterType.Ma5DecreaseFilter:
                    return new Ma5DecreaseFilter(datas, param);
                case FilterType.Ma5LMa20Filter:
                    return new Ma5LMa20Filter(datas, param);
                case FilterType.Ma5HMa20Filter:
                    return new Ma5HMa20Filter(datas, param);
                case FilterType.Ma5LMa60Filter:
                    return new Ma5LMa60Filter(datas, param);
                case FilterType.Ma5HMa60Filter:
                    return new Ma5HMa60Filter(datas, param);
                case FilterType.Ma5LMa120Filter:
                    return new Ma5LMa120Filter(datas, param);
                case FilterType.Ma5HMa120Filter:
                    return new Ma5HMa120Filter(datas, param);
                case FilterType.Ma60LowerPriceFilter:
                    return new Ma60LowerPriceFilter(datas, param);

                case FilterType.PriceDecreaseFilter:
                    return new PriceDecreaseFilter(datas, param); 
                case FilterType.PriceIncreaseFilter:
                    return new PriceIncreaseFilter(datas, param); 
                case FilterType.PriceMostHighInDaysFilter:
                    return new PriceMostHighInDaysFilter(datas, param);
                case FilterType.PriceMostLowinDaysFilter:
                    return new PriceMostLowinDaysFilter(datas, param);

                case FilterType.VolumnMoreThanFilter:
                    return new VolumnMoreThanFilter(datas, param);  
                case FilterType.VolumnLessThanFilter:
                    return new VolumnLessThanFilter(datas, param); 
                case FilterType.VolumnIncreaseFilter:
                    return new VolumnIncreaseFilter(datas, param);
                case FilterType.VolumnDecreaseFilter:
                    return new VolumnDecreaseFilter(datas, param);


                case FilterType.RSI5DecreaseFilter:
                    return new RSI5DecreaseFilter(datas, param);
                case FilterType.RSI5IncreaseFilter:
                    return new RSI5IncreaseFilter(datas, param);
                case FilterType.RSI5HRSI10Filter:
                    return new RSI5HRSI10Filter(datas, param);
                case FilterType.RSI5LRSI10Filter:
                    return new RSI5LRSI10Filter(datas, param);
                case FilterType.RSI5LowerThanValueFilter:
                    return new RSI5LowerThanValueFilter(datas, param);
                case FilterType.RSIHigherThanValueFilter:
                    return new RSIHigherThanValueFilter(datas, param);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}  