using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class VolumnIncreaseMore : IFilter
    {
        public VolumnIncreaseMore(List<StockData> _stockDataList) : base(_stockDataList)
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
                    if ( currentData.Date[j].AddDays(-1) != currentData.Date[j-1])
                        currentData.IsFilter[j] = true;

                    if (currentData.Volumn[j] <= currentData.Volumn[j-1] * 2)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
