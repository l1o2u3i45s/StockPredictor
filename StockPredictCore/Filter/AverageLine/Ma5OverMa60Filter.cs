using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
    //Ma5穿過Ma60
    public class Ma5OverMa60Filter : FilterBase
    {

        public Ma5OverMa60Filter()
        {
            parameter = 1;
        }


        public Ma5OverMa60Filter(double initParam) : base(initParam)
        {

        }

        public Ma5OverMa60Filter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 60; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.MA5[j] > currentData.MA60[j] && currentData.MA5[j-1] <= currentData.MA60[j-1];

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
