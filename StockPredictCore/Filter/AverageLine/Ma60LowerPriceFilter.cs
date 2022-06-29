using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Value
{
    public class Ma60LowerPriceFilter:FilterBase
    {
        public Ma60LowerPriceFilter()
        {
            parameter = 2;
        }

        public Ma60LowerPriceFilter(double initParam) : base(initParam)
        {

        }
        public Ma60LowerPriceFilter(IEnumerable<StockData> _stockDataList, double param) : base(_stockDataList, param)
        {
        }

        public override void Execute()
        {
            double ratio = parameter / 100;

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.MA60[j] >= currentData.ClosePrice[j] * (1-ratio) && 
                                        currentData.MA60[j] <= currentData.ClosePrice[j];

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
