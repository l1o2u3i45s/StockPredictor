using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.RSI
{
   public class RSI5LowerThanValueFilter:FilterBase
    {
        public RSI5LowerThanValueFilter()
        {
            parameter = 1;
        }

        public RSI5LowerThanValueFilter(double initParam) : base(initParam)
        {

        }
        public RSI5LowerThanValueFilter(IEnumerable<StockData> _stockDataList, double param) : base(_stockDataList, param)
        {
        }

        public override void Execute()
        {
            double ratio = parameter;

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.RSI5[j] <=  ratio;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
