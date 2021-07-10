using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;
using StockPredictCore.Filter;

namespace StockPredictor.Class
{
    public static class FilterFactory
    {
        public static IFilter CreatFilterByFilterType(FilterType _type,List<StockData> datas)
        {
            switch (_type)
            {
                case FilterType.Ma20_Ma5_Filter:
                    return new Ma20_Ma5_Filter(datas);
                case FilterType.VolumnIncreaseMore:
                    return new VolumnIncreaseMore(datas);
                case FilterType.VolumnThresholdFilter:
                    return new VolumnThresholdFilter(datas);
                case FilterType.ClosedMoreThanOpen:
                    return new ClosedMoreThanOpenFilter(datas);
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
            } 
        }
    }
}
