using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
    public class Ma5LMa20Filter:FilterBase
    {
        public Ma5LMa20Filter()
        {
            parameter = 1;
        }

        public Ma5LMa20Filter(double initParam) : base(initParam)
        {

        }
        public Ma5LMa20Filter(IEnumerable<StockData> _stockDataList,double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            double ratio = parameter/100;

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.MA5[j] < currentData.MA20[j] - currentData.MA20[j] * ratio;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
