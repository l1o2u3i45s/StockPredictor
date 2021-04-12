using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.PredictStrategy
{
    public class HightRiskPredictStrategy : IPredictStrategy //算反轉趨勢用的
    {
        public override int[] FindBuyTime(StockData data)
        {
            int ratio = 1;
            List<int> result = new List<int>();

            for(int i = 0;i < data.Date.Length; i++)
            { 
                if( 是否上漲(i,data) && 是否下跌(i -1,data) && 是否下跌(i - 2, data)  && 是否下跌(i - 3, data))
                { 
                    if (下影線超過燭身的N倍(i,data,ratio))
                    {
                        if(上影線小於燭身(i,data) ) // && 是否MA5減收盤價大於一個燭身(i,data))
                        {
                            if(是否低於20日均線(i,data) && 是否低於5日均線(i,data))
                                result.Add(i);
                        } 
                    } 
                } 
            } 
            return result.ToArray();
        }
        public override int[] FindSellTime(StockData data)
        {
            throw new NotImplementedException();
        }

        private bool 是否上漲(int idx,StockData data)
        {
            if (idx < 0) return false;

            return data.ClosePrice[idx] > data.OpenPrice[idx];
        }

        private bool 是否下跌(int idx, StockData data)
        {
            if (idx < 0) return false;

            return data.ClosePrice[idx] < data.OpenPrice[idx];
        }

        private bool 下影線超過燭身的N倍(int idx, StockData data,double ratio)
        {
            var candleLegth = Math.Round(data.ClosePrice[idx] - data.OpenPrice[idx], 2);
            var highLine = Math.Round(data.HightestPrice[idx] - data.ClosePrice[idx], 2);
            var downLine = Math.Round(data.OpenPrice[idx] - data.LowestPrice[idx], 2);

            return Math.Round(candleLegth * ratio, 2) <= Math.Round(downLine, 2);
        }

        private bool 上影線小於燭身(int idx, StockData data)
        {
            var candleLegth = Math.Round(data.ClosePrice[idx] - data.OpenPrice[idx], 2);
            var highLine = Math.Round(data.HightestPrice[idx] - data.ClosePrice[idx], 2);

            return highLine <= candleLegth;
        }

        private bool 是否低於20日均線(int idx,StockData data)
        {
            return data.MA20[idx] >= data.ClosePrice[idx];
        }
        private bool 是否低於5日均線(int idx, StockData data)
        {
            return data.MA5[idx] >= data.ClosePrice[idx];
        }
        private bool 是否MA5減收盤價大於一個燭身(int idx, StockData data)
        {
            var candleLegth = Math.Round(data.ClosePrice[idx] - data.OpenPrice[idx], 2);
            return data.MA5[idx] - data.ClosePrice[idx] >= candleLegth;
        }
    }
}
