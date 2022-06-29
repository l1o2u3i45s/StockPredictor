using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictor.Class.SumResult
{
    public static class SumResultFactory
    {
        public static SumResult Create(IEnumerable<StockInfo> stockInfoList)
        {
            SumResult result = new SumResult();
            result.TotalPrice = Math.Round(stockInfoList.Sum(_ => _.ClosePrice), 2);
            result.DiffPrice = Math.Round(stockInfoList.Sum(_ => _.CloseDiffValue), 2);
            result.GrowRatio = Math.Round(((result.TotalPrice + result.DiffPrice) / result.TotalPrice - 1) * 100, 2);
            result.WinAmount = stockInfoList.Count(_ => _.CloseDiffValue > 0);
            result.LoseAmount = stockInfoList.Count(_ => _.CloseDiffValue <= 0);

            return result;
        }
    }
}
