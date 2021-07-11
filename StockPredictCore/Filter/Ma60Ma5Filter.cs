using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class Ma60Ma5Filter : IFilter
    {
        public Ma60Ma5Filter(List<StockData> _stockDataList) : base(_stockDataList)
        {
        }

        public override void Execute()
        {
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];
                double minus = 0.01;

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    //if ((currentData.MA5[j] <= currentData.MA60[j] - currentData.MA60[j] * minus) || (currentData.MA5[j] >= currentData.MA60[j] + currentData.MA60[j] * minus))
                    //    currentData.IsFilter[j] = true;

                    if (currentData.MA5[j] > currentData.MA60[j] - currentData.MA60[j] * minus || currentData.MA5[j] < currentData.MA60[j] - currentData.MA60[j] * minus * 2)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
