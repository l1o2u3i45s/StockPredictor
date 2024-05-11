using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore.Filter.Value
{
    public class PriceLowerYearFilter : FilterBase
    {
        public PriceLowerYearFilter()
        {
            parameter = 2;
        }

        public PriceLowerYearFilter(double initParam) : base(initParam)
        {

        }
        public PriceLowerYearFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
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

                    var closedPrice = currentData.ClosePrice[j];
                    var ma360 = currentData.MA360[j];
                    bool isCorrespond = closedPrice < ma360;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;

                }

            }
        }
    }
}
