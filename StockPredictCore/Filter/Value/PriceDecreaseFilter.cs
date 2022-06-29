using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.ValueDiff
{
   public class PriceDecreaseFilter:FilterBase
    {
        public PriceDecreaseFilter()
        {
            parameter = 2;
        }

        public PriceDecreaseFilter(double initParam) : base(initParam)
        {

        }

        public PriceDecreaseFilter(IEnumerable<StockData> _stockDataList, double _param ) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            double ratio = parameter/100;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 1; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;


                    bool isCorrespond = (currentData.ClosePrice[j] - currentData.ClosePrice[j - 1] ) / currentData.ClosePrice[j - 1] <= ratio*-1;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
