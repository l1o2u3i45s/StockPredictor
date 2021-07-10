using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class Ma5IncreaseFilter : IFilter
    {
        public Ma5IncreaseFilter(List<StockData> _stockDataList) : base(_stockDataList)
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

                    if ( currentData.MA5[j - 2] > currentData.MA5[j-1] || currentData.MA5[j - 1] > currentData.MA5[j])
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
