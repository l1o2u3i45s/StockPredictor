using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Other
{
    public class Other_RSI5L20_HigherThanLastLowFilter:FilterBase
    {
        public Other_RSI5L20_HigherThanLastLowFilter()
        {
           
        }

        public Other_RSI5L20_HigherThanLastLowFilter(double initParam) : base(initParam)
        {

        }

        public Other_RSI5L20_HigherThanLastLowFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
           
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 1; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j]) 
                        continue; 

                    bool rsi5L20 = currentData.RSI5[j] <= 20;

                    if(rsi5L20 == false)
                    {
                        currentData.IsFilter[j] = true;
                        continue;
                    }

                    double d = currentData.RSI5[j];

                    int index = 0;
                    for (int h = j-10; h > 1; h--)
                    {
                        if (currentData.ClosePrice[h-1] > currentData.ClosePrice[h] &&
                            currentData.ClosePrice[h+1] > currentData.ClosePrice[h] &&
                            currentData.RSI5[h] <= 30
                            )
                        {
                            index = h;
                            break;
                        }
                           
                    }

                    if (index == 0)
                    {
                        currentData.IsFilter[j] = true;
                        continue;
                    }
                       


                    bool isCurrentHigher = currentData.ClosePrice[j] > currentData.ClosePrice[index];

                    bool isCorrespond = isCurrentHigher && rsi5L20;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
