using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Volumn
{
    public class VolumnMostHighInDayFilter:FilterBase
    {
        public VolumnMostHighInDayFilter()
        {
            parameter = 20;
        }

        public VolumnMostHighInDayFilter(double initParam) : base(initParam)
        {

        }

        public VolumnMostHighInDayFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
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

                    double mostHightVolumn = 0;
                    for (int k = j - days; k < j; k++)
                    {
                        if (currentData.Volumn[k] > mostHightVolumn)
                            mostHightVolumn = currentData.Volumn[k];
                    }

                    bool isCorrespond = currentData.Volumn[j] >= mostHightVolumn;

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
