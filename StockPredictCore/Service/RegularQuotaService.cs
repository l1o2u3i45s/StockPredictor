using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;
using StockPredictCore.Filter;

namespace StockPredictCore.Service
{
    public static class RegularQuotaService
    {

        public static List<RegularQuotaStockInfo> Calulate(StockData data, DateTime startDate, int monthlyInvestValue)
        {
            List<RegularQuotaStockInfo> result = new List<RegularQuotaStockInfo>();

            for (int i = 1; i < data.Date.Length; i++)
            {
                if (data.Date[i] >= startDate && data.IsFilter[i] == false)
                {
                   
                    RegularQuotaStockInfo info = new RegularQuotaStockInfo();
                    result.Add(info);
                    info.Date = data.Date[i];
                    info.CurrentPrice = data.ClosePrice[i];  
                    info.InventoryAveragePrice = Math.Round(result.Average(_ => _.CurrentPrice),2);
                    info.AccumulationMoney = result.Count() * monthlyInvestValue; 
                    info.GrowRatio = info.CurrentPrice / info.InventoryAveragePrice;
                    info.TotalStockValue = info.GrowRatio * info.AccumulationMoney;
                }

            }

            return result;
        }
         
        public class RegularQuotaStockInfo
        {
            public DateTime Date { get; set; }

            public double InventoryAveragePrice { get; set; } //目前平均股價

            public double CurrentPrice { get; set; } //現在股價

            public double AccumulationMoney { get; set; }  //累積金額

            public double TotalStockValue { get; set; }  //股票現值

            public double GrowRatio { get; set; } //收益率
        }
    }
}
