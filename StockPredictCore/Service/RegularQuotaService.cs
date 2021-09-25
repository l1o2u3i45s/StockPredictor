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

        public static List<RegularQuotaStockInfo> Calulate(StockData data, DateTime startDate, int monthlyInvestValue)
        {
            List<RegularQuotaStockInfo> result = new List<RegularQuotaStockInfo>();

            for (int i = 1; i < data.Date.Length; i++)
            {
                if (data.Date[i] >= startDate && data.Date[i].Month == data.Date[i-1].AddMonths(1).Month)
                {
                   
                    RegularQuotaStockInfo info = new RegularQuotaStockInfo();
                    result.Add(info);
                    info.Date = data.Date[i];
                    info.CurrentPrice = data.ClosePrice[i];  
                    info.InventoryAveragePrice = result.Average(_ => _.CurrentPrice);
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
