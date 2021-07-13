using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;
using StockPredictCore.Filter;

namespace StockPredictCore.AverageLine
{
    public class Ma5IncreaseFilter : IFilter
    {
        public Ma5IncreaseFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
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

                    bool isCorrespond = currentData.MA5[j] - currentData.MA5[j - 1] > currentData.MA5[j-1] * parameter / 100;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
