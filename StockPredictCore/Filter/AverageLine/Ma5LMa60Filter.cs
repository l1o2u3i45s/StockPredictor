using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
    //Ma5小於MA60的幾倍
    public class Ma5LMa60Filter:FilterBase
    {
        public Ma5LMa60Filter()
        {
            parameter = 1;
        }

        public Ma5LMa60Filter(double initParam) : base(initParam)
        {

        }
        public Ma5LMa60Filter(IEnumerable<StockData> _stockDataList,double param) : base(_stockDataList, param)
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

                    bool isCorrespond = currentData.MA5[j] < currentData.MA60[j] - currentData.MA60[j] * ratio;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                } 
            }
        }
    }
}
