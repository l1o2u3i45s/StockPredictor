using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Value
{
    public class PriceMostHighInDaysFilter :IFilter
    {
        public PriceMostHighInDaysFilter()
        {
            parameter = 20;
        }

        public PriceMostHighInDaysFilter(double initParam) : base(initParam)
        {

        }

        public PriceMostHighInDaysFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            int days = (int)parameter;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = days ; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    double mostHightClosePrice = 0;
                    for (int k = j - days; k < j; k++)
                    {
                        if (currentData.HightestPrice[k] > mostHightClosePrice)
                            mostHightClosePrice = currentData.HightestPrice[k];
                    }

                    bool isCorrespond = currentData.ClosePrice[j] >= mostHightClosePrice;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
