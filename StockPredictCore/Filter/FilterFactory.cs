﻿using System;
using System.Collections.Generic; 
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

                case FilterType.PriceDecreaseFilter:
                    return new PriceDecreaseFilter(datas, param); 
                case FilterType.PriceIncreaseFilter:
                    return new PriceIncreaseFilter(datas, param); 



                case FilterType.VolumnMoreThanFilter:
                    return new VolumnMoreThanFilter(datas, param);  
                case FilterType.VolumnLessThanFilter:
                    return new VolumnLessThanFilter(datas, param); 
                case FilterType.VolumnIncreaseFilter:
                    return new VolumnIncreaseFilter(datas, param);
                   
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}