using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{

    //Ma5 > Ma10 > Ma20
    public class Ma20_Ma5_Filter : IFilter
    {
        public Ma20_Ma5_Filter(List<StockData> _stockDataList) : base(_stockDataList)
        {

        }

        public override void Execute()
        {
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 0; j < currentData.Date.Length; j++)
                {
                    if(currentData.IsFilter[j])
                        continue;

                    if ( (currentData.MA5[j] <= currentData.MA20[j]*0.97) || (currentData.MA5[j]*0.97 >= currentData.MA20[j]))
                        currentData.IsFilter[j] = true;
                }
               
            }
        }
    }
}
