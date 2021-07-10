using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
  public  class VolumnThresholdFilter : IFilter
    {
        public VolumnThresholdFilter(List<StockData> _stockDataList) : base(_stockDataList)
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
                     
                    if (currentData.Volumn[j] <= 3000*1000)
                        currentData.IsFilter[j] = true;
                } 
            }
        }
    }
}
