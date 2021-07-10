using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class OverLastHighestPriceFilter: IFilter
    {
        public OverLastHighestPriceFilter(List<StockData> _stockDataList) : base(_stockDataList)
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

                    double lastHight = 0;

                    double yesHeight = currentData.HightestPrice[j - 1];

                    for (int k = j-2; k > 0; k--)
                    {
                        var currentHigh = currentData.HightestPrice[k];
                        if (currentHigh > yesHeight)
                            lastHight = currentHigh;
                    }

                    if (currentData.HightestPrice[j] > lastHight)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
