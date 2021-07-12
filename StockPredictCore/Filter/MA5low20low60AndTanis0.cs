using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class MA5low20low60AndTanis0: IFilter
    {
        public MA5low20low60AndTanis0(IEnumerable<StockData> _stockDataList) : base(_stockDataList)
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

                    if (currentData.MA5[j] > currentData.MA20[j] || currentData.MA5[j] > currentData.MA60[j])
                        currentData.IsFilter[j] = true;

                    if (currentData.MA5[j - 1] - currentData.MA5[j]   < currentData.MA5[j-1] * 0.04)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
