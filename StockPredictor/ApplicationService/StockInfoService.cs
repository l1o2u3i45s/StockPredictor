using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictor.ApplicationService
{
    public interface IStockInfoService
    {
        ConcurrentBag<StockInfo> FilterData(ConcurrentBag<StockData> stockDataList,
            Dictionary<string, string> stockInfoDictionary, DateTime startTime, DateTime endTime);
    }
    public class StockInfoService: IStockInfoService
    {
        public ConcurrentBag<StockInfo> FilterData(ConcurrentBag<StockData> stockDataList, Dictionary<string, string> stockInfoDictionary, DateTime startTime, DateTime endTime)
        {
            ConcurrentBag<StockInfo> stockInfoList = new ConcurrentBag<StockInfo>();

            Parallel.ForEach(stockDataList, stockData =>
            {
                for (int i = 0; i < stockData.Date.Length; i++)
                {

                    if (stockData.IsFilter[i] == false && stockData.Date[i] >= startTime &&
                        stockData.Date[i] <= endTime)
                    {
                        double lastestClosePrice = Math.Round(stockData.ClosePrice[stockData.Date.Length - 1], 2);

                        if (stockInfoDictionary.ContainsKey(stockData.ID) == false)
                            continue;

                        var info = new StockInfo(stockData, i, stockData.ID, stockInfoDictionary[stockData.ID]);
                        info.CurrentClosePrice = lastestClosePrice;

                        if (stockInfoList.Count(_ => _.Name == info.Name) == 0)
                            stockInfoList.Add(info);
                    }
                }
            });

            return stockInfoList;
        }
    }
}
