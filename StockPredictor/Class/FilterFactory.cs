using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;
using StockPredictCore.AverageLine;
using StockPredictCore.Filter;
using StockPredictCore.Filter.AverageLine;
using StockPredictCore.Filter.ValueDiff;
using StockPredictCore.Filter.Volumn;
using StockPredictCore.ValueDiff;

namespace StockPredictor.Class
{
    public static class FilterFactory
    {
        public static IFilter CreatFilterByFilterType(FilterInfo filterInfo, IEnumerable<StockData> datas)
        {
            switch (filterInfo.Type)
            {
                case FilterType.Ma5IncreaseFilter:
                   return new Ma5IncreaseFilter(datas, filterInfo.Param);
                case FilterType.Ma5DecreaseFilter:
                    return new Ma5DecreaseFilter(datas, filterInfo.Param);
                case FilterType.Ma5LMa20Filter:
                    return new Ma5LMa20Filter(datas, filterInfo.Param); 
                case FilterType.Ma5LMa60Filter:
                    return new Ma5LMa60Filter(datas, filterInfo.Param); 
                case FilterType.PriceDecreaseFilter:
                    return new PriceDecreaseFilter(datas, filterInfo.Param); 
                case FilterType.PriceIncreaseFilter:
                    return new PriceIncreaseFilter(datas, filterInfo.Param); 
                case FilterType.VolumnMoreThanFilter:
                    return new VolumnMoreThanFilter(datas, filterInfo.Param);  
                case FilterType.VolumnLessThanFilter:
                    return new VolumnLessThanFilter(datas, filterInfo.Param); 
                case FilterType.VolumnIncreaseFilter:
                    return new VolumnIncreaseFilter(datas, filterInfo.Param);
                   
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
