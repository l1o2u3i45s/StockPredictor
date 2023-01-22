using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq; 
using NetTrader.Indicator;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace StockPredictCore
{
    public interface IPreProcessService
    {
        ConcurrentBag<StockData> GetStockData(string[] stockFiles, Dictionary<string, string> stockInfoDictionary);

        ConcurrentBag<InvestInstitutionBuySellData> GetInvestInstitutionBuySellDataData(string[] stockFiles);
        ConcurrentBag<PERatioTableData> GetPERatioTableData(string[] stockFiles);
        ConcurrentBag<FinancialStatementsData> GetFinancialStatementsData(string[] stockFiles);
        void Execute(StockData data);
    }

    public class PreProcessService : IPreProcessService
    {
        public ConcurrentBag<InvestInstitutionBuySellData> GetInvestInstitutionBuySellDataData(string[] stockFiles)
        {
            var investInstitutionBuySellDataDataList = new ConcurrentBag<InvestInstitutionBuySellData>();
            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetInvestInstitutionBuySellData(_))
                {
                    investInstitutionBuySellDataDataList.Add(data);
                }
            });
            return investInstitutionBuySellDataDataList;
        }

        public ConcurrentBag<StockData> GetStockData(string[] stockFiles, Dictionary<string, string> stockInfoDictionary)
        {
            var stockDataList = new ConcurrentBag<StockData>();
           
            Parallel.ForEach(stockFiles, _ =>
            {
                StockData data = DataParser.GetStockData(_, stockInfoDictionary);
                if (stockInfoDictionary.ContainsKey(data.ID))
                    data.Name = stockInfoDictionary[data.ID];

                Execute(data);
                stockDataList.Add(data);
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
        public void Execute(StockData data)
        {
            Stopwatch stopwatcher = new Stopwatch();
            stopwatcher.Start();
            CaculateRSVandKD(data);
            CaculateMA(data);
            CaculateRSI(data);
            CaculateBoolling(data);
            Console.WriteLine(stopwatcher.ElapsedMilliseconds + " ms");
        }

        void CaculateBoolling(StockData data)
        {
            int dataCount = data.Date.Count();
            int day = 20;
            for (int i = day * 2 -1; i < dataCount; i++)
            {

                double avg = 0;
                double masum = 0;
                for (int j = i- day + 1; j <= i; j++)
                {
                    masum += data.MA20[j];
                } 
                avg = masum / day;

                double sigmasum = 0;
                for (int j = i - day + 1; j < i; j++)
                {
                    sigmasum += (data.ClosePrice[j] - avg) * (data.ClosePrice[j] - avg);
                }

                double std = Math.Sqrt(sigmasum / day);

                data.BoollingHighLimit[i] = avg + std * 2;
                data.BoollingLowLimit[i] = avg - std * 2;
                   
            }
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
            int dataCount = data.Date.Count();
            Queue<double> queue5 = new Queue<double>();
            Queue<double> queue20 = new Queue<double>();
            Queue<double> queue60 = new Queue<double>();
            Queue<double> queue120 = new Queue<double>();
            for (int i = 0; i < dataCount; i++)
            {
                if (queue5.Count < 5)
                    queue5.Enqueue(data.ClosePrice[i]);

                if (queue20.Count < 20)
                    queue20.Enqueue(data.ClosePrice[i]);
                 
                if (queue60.Count < 60)
                    queue60.Enqueue(data.ClosePrice[i]);

                if (queue120.Count < 120)
                    queue120.Enqueue(data.ClosePrice[i]);

                if (queue5.Count == 5)
                { 
                    data.MA5[i] = queue5.Average();
                    queue5.Dequeue();
                }

                if (queue20.Count == 20)
                { 
                    data.MA20[i] = queue20.Average();
                    queue20.Dequeue();
                }
                
                if(queue60.Count == 60)
                { 
                    data.MA60[i] = queue60.Average();
                    queue60.Dequeue();
                }

                if (queue120.Count == 120)
                {
                    data.MA120[i] = queue120.Average();
                    queue120.Dequeue();
                }

            }
        }
        
    }
}
