using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
     //Ma5大於Ma60 幾%以內
    public class Ma5HMa60InRatioFilter : IFilter
    {

        public Ma5HMa60InRatioFilter()
        {
            parameter = 1;
        }


        public Ma5HMa60InRatioFilter(double initParam) : base(initParam)
        {

        }

        public Ma5HMa60InRatioFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
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

                    bool isCorrespond = currentData.MA5[j] - currentData.MA60[j]  <=  currentData.MA60[j] * ratio;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
