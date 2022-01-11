using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
  
    //Ma60穿過Ma5
    public class Ma60OverMa5Filter : IFilter
    {

        public Ma60OverMa5Filter()
        {
            parameter = 1;
        }


        public Ma60OverMa5Filter(double initParam) : base(initParam)
        {

        }

        public Ma60OverMa5Filter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
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

                    bool isCorrespond = currentData.MA60[j] > currentData.MA5[j] && currentData.MA60[j - 1] <= currentData.MA5[j - 1];

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
