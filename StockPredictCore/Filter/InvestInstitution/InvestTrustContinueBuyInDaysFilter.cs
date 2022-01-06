using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.InvestInstitution
{
    //投信連續幾天買進
   
    public class InvestTrustContinueBuyInDaysFilter : IFilter
    {
        public InvestTrustContinueBuyInDaysFilter()
        {
            parameter = 5;
        }

        public InvestTrustContinueBuyInDaysFilter(double initParam) : base(initParam)
        {

        }

        public InvestTrustContinueBuyInDaysFilter(IEnumerable<StockData> _stockDataList, double _param) : base(_stockDataList, _param)
        {
        }

        public override void Execute()
        {
            int target = (int)parameter;

            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];
                var institutionData = currentData.InstitutionBuyAndSell.Single(_ => _.InstitutionType == eInstitution.投信);
                
                int continueBuyDayCount = 0;
                
                if(currentData.Date.Length != institutionData.Date.Length)
                    continue; 

                for (int j = 0; j < institutionData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                       continue;
                     
                    if (institutionData.BuyAmount[j] > 0)
                        continueBuyDayCount++;
                    else 
                        continueBuyDayCount = 0;

                    if (continueBuyDayCount < target)
                        currentData.IsFilter[j] = true;
                }

            }
        }
    }
}
