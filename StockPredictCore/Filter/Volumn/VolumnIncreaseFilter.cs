using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public class VolumnIncreaseFilter : FilterBase
    {
        public VolumnIncreaseFilter()
        {
            parameter = 2;
        }

        public VolumnIncreaseFilter(double initParam) : base(initParam)
        {

        }

        public VolumnIncreaseFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        { 

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 2; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    if (currentData.Date[j] == new DateTime() || currentData.Date[j - 1] == new DateTime())
                    {
                        currentData.IsFilter[j] = true;
                        continue;
                    }

                    if (currentData.Date[j].AddDays(-1) != currentData.Date[j - 1])
                        currentData.IsFilter[j] = true;

                    bool isCorrespond = currentData.Volumn[j] > currentData.Volumn[j - 1] * parameter;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }

        }
    }
}
