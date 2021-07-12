using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.AverageLine
{
    public class Ma5LMa20Filter:IFilter
    {
        public Ma5LMa20Filter(List<StockData> _stockDataList,double[] _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            double ratio = parameter[0];

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    if (currentData.MA5[j] > currentData.MA20[j] - currentData.MA20[j] * ratio)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
