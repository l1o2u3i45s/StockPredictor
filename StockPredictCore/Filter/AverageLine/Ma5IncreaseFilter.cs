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
        public Ma5IncreaseFilter(IEnumerable<StockData> _stockDataList, double[] _param = null) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        { 

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 2; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.MA5[j] > currentData.MA5[j-1]  && currentData.MA5[j-1] > currentData.MA5[j - 2];

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
