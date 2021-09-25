using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Service
{
    public static class RegularQuotaService
    {

        public static RegularQuotaResult Calulate(StockData data,DateTime startDate,int monthlyInvestValue)
        {
            RegularQuotaResult result = new RegularQuotaResult();
              
            return result;
        }

        public class RegularQuotaResult
        {
            public RegularQuotaResult()
            {
                RegularQuotaStockInfoList = new List<RegularQuotaStockInfo>();
            }

            public int InvestTotalMoney { get; set; }
            public int FinalTotalMoney { get; set; }
            public double GrowRatio { get; set; }

            public List<RegularQuotaStockInfo> RegularQuotaStockInfoList { get; set; }  
        }

        public class RegularQuotaStockInfo
        {
            public DateTime Date { get; set; }

            public double InventoryAveragePrice { get; set; }

            public double CurrentPrice { get; set; }

            public double CurrentInventory { get; set; }
        }
    }
}
