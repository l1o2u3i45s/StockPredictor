using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq; 
using NetTrader.Indicator;

namespace StockPredictCore
{
    public class PreProcessor
    {
        public void Execute(StockData data)
        {
            CaculateRSVandKD(data);
            CaculateMA(data);
           CaculateRSI(data);
        }

        void CaculateRSI(StockData data)
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
