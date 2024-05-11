using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using NetTrader.Indicator;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using StockPredictCore.Service;

namespace StockPredictCore
{
    public interface IPreProcessService
    {
        ConcurrentBag<StockData> GetStockData(string[] stockFiles, Dictionary<string, string> stockInfoDictionary);

        List<InvestInstitutionBuySellData> GetInvestInstitutionBuySellDataData(List<string> filePaths);
        ConcurrentBag<PERatioTableData> GetPERatioTableData(string[] stockFiles);
        ConcurrentBag<FinancialStatementsData> GetFinancialStatementsData(string[] stockFiles);
        Task Execute(StockData data);
    }

    public class PreProcessService : IPreProcessService
    {
        private readonly ICalculateService _calculateService;
        public PreProcessService(ICalculateService calculateService)
        {

            _calculateService = calculateService;
        }
        public List<InvestInstitutionBuySellData> GetInvestInstitutionBuySellDataData(List<string> filePaths)
        {
            var investInstitutionBuySellDataDataList = new List<InvestInstitutionBuySellData>();

            foreach (var file in filePaths)
            {
                foreach (var data in DataParser.GetInvestInstitutionBuySellData(file))
                {
                    investInstitutionBuySellDataDataList.Add(data);
                }
            }

            return investInstitutionBuySellDataDataList;
        }

        public ConcurrentBag<StockData> GetStockData(string[] stockFiles, Dictionary<string, string> stockInfoDictionary)
        {
            var stockDataList = new ConcurrentBag<StockData>();

            Parallel.ForEach(stockFiles, async filepath =>
            {
                StockData data = DataParser.GetStockData(filepath, stockInfoDictionary);
                stockDataList.Add(data);
                await Execute(data);
            });

            return stockDataList;
        }


        public ConcurrentBag<PERatioTableData> GetPERatioTableData(string[] stockFiles)
        {
            var peRatioTableDataDataList = new ConcurrentBag<PERatioTableData>();

            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetPERatioTableData(_))
                {
                    peRatioTableDataDataList.Add(data);
                }
            });

            return peRatioTableDataDataList;
        }

        public ConcurrentBag<FinancialStatementsData> GetFinancialStatementsData(string[] stockFiles)
        {
            var financialStatementsDataList = new ConcurrentBag<FinancialStatementsData>();

            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetFinancialStatementsData(_))
                {
                    financialStatementsDataList.Add(data);
                }
            });


            return financialStatementsDataList;
        }
        public async Task Execute(StockData data)
        {
            CaculateMA(data);
            var taskList = new List<Task>
            {
                Task.Factory.StartNew(() => CaculateRSVandKD(data)),
                Task.Factory.StartNew(() => CaculateRSI(data)),
                Task.Factory.StartNew(() => CalculateBoolling(data))
            };
            await Task.WhenAll(taskList);

        }

        private void CalculateBoolling(StockData data)
        {

            var boollingData = _calculateService.Cal_Boolling(data.ClosePrice);
            data.BoollingHighLimit = boollingData.UpperLimit;
            data.BoollingLowLimit = boollingData.LowerLimit;

        }

        private void CaculateRSI(StockData data)
        {
            int dataCount = data.Date.Count();
            List<Ohlc> ohlcList = new List<Ohlc>();
            for (int i = 0; i < dataCount; i++)
            {
                Ohlc a = new Ohlc();
                a.Date = data.Date[i];
                a.Open = data.OpenPrice[i];
                a.Close = data.ClosePrice[i];
                a.High = data.HightestPrice[i];
                a.Low = data.LowestPrice[i];
                ohlcList.Add(a);
            }
            if (dataCount < 10)
                return;

            RSI rsi5 = new RSI(5);
            rsi5.Load(ohlcList);
            RSISerie serie5 = rsi5.Calculate();


            RSI rsi10 = new RSI(10);
            rsi10.Load(ohlcList);
            RSISerie serie10 = rsi10.Calculate();

            for (int i = 0; i < dataCount; i++)
            {
                if (serie5.RSI[i] != null)
                    data.RSI5[i] = (double)serie5.RSI[i];

                if (serie10.RSI[i] != null)
                    data.RSI10[i] = (double)serie10.RSI[i];

            }
        }


        void CaculateRSVandKD(StockData data)
        {
            int dataCount = data.Date.Count();

            //RSV = (今日收盤價 - 最近九天的最低價) / (最近九天的最高價 - 最近九天最低價)
            //K = 2/3 X (昨日K值) + 1/3 X (今日RSV)
            //D = 2/3 X (昨日D值) + 1/3 X (今日K值)
            for (int i = 8; i < dataCount; i++)
            {
                double lowerPriceIn9 = 100000;
                double higherPriceIn9 = 0;
                for (int j = i - 8; j <= i; j++)
                {
                    if (lowerPriceIn9 > data.LowestPrice[j])
                        lowerPriceIn9 = data.LowestPrice[j];

                    if (higherPriceIn9 < data.HightestPrice[j])
                        higherPriceIn9 = data.HightestPrice[j];
                }

                data.RSV[i] = (data.ClosePrice[i] - lowerPriceIn9) / (higherPriceIn9 - lowerPriceIn9);
                data.KValue[i] = data.KValue[i - 1] * 2 / 3 + data.RSV[i] / 3;
                data.DValue[i] = data.DValue[i - 1] * 2 / 3 + data.KValue[i] / 3;
            }
        }

        void CaculateMA(StockData data)
        {
            data.MA5 = _calculateService.Cal_MA(data.ClosePrice, 5);
            data.MA20 = _calculateService.Cal_MA(data.ClosePrice, 20);
            data.MA60 = _calculateService.Cal_MA(data.ClosePrice, 60);
            data.MA120 = _calculateService.Cal_MA(data.ClosePrice, 120);
            data.MA120 = _calculateService.Cal_MA(data.ClosePrice, 360);
        }

    }
}
