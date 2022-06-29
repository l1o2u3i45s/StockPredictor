using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.InvestInstitution
{
    //外資連續幾天買進

    public class ForeignContinueBuyInDaysFilter : FilterBase
    {
        public ForeignContinueBuyInDaysFilter()
        {
            parameter = 5;
        }

        public ForeignContinueBuyInDaysFilter(double initParam) : base(initParam)
        {

        }

        public ForeignContinueBuyInDaysFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            int target = (int)parameter;

            for (int i = 0; i < stockDataList.Count; i++)
            { 
                var currentData = stockDataList[i];
                  
                var institutionData = currentData.InstitutionBuyAndSell.Single(_ => _.InstitutionType == eInstitution.外資);

                int continueBuyDayCount = 0;

                if (institutionData.Date.Length == 0)
                {
                    for (int j = 0; j < currentData.Date.Length; j++)
                    {
                        currentData.IsFilter[j] = true;
                    }
                    continue;
                }

                for (int j = 0; j < institutionData.Date.Length; j++)
                {
                    var idx = currentData.Date.ToList().IndexOf(institutionData.Date[j]);

                    if(idx < 0 )
                        continue;
                     
                    if (institutionData.BuyAmount[j] > institutionData.SellAmount[j])
                        continueBuyDayCount++;
                    else
                        continueBuyDayCount = 0;

                    if (continueBuyDayCount < target)
                        currentData.IsFilter[idx] = true;
                }

            }
        }
    }
}
