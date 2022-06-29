using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Volumn
{
    public class VolumnMostLowInDayFilter: FilterBase
    {
        public VolumnMostLowInDayFilter()
        {
            parameter = 20;
        }

        public VolumnMostLowInDayFilter(double initParam) : base(initParam)
        {

        }

        public VolumnMostLowInDayFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            int days = (int)parameter;
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = days; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    double mostlowVolumn = Int32.MaxValue;
                    for (int k = j - days; k < j; k++)
                    {
                        if (currentData.Volumn[k] < mostlowVolumn)
                            mostlowVolumn = currentData.Volumn[k];
                    }

                    bool isCorrespond = currentData.Volumn[j] <= mostlowVolumn;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }

        }
    }
}
