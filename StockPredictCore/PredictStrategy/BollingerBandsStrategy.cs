using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.PredictStrategy
{
    public class BollingerBandsStrategy : IPredictStrategy //布林通道預測 抓下線反轉
    {
        public override int[] FindBuyTime(StockData data)
        {
            List<int> result = new List<int>();
            bool[] isUnderBottom = new bool[data.Date.Length];
            for (int i = 19; i < data.Date.Length; i++)
            {
                List<double> closePrice20 = new List<double>();
                for(int k = i -19; k <= i; k++)
                {
                    closePrice20.Add(data.ClosePrice[k]);
                }
                var std = MathTool.CalculateStdDev(closePrice20);
                var upperLine = data.MA20[i] + std * 2;
                var bottomLine = data.MA20[i] - std * 2;

                isUnderBottom[i] = data.ClosePrice[i] > bottomLine;

                if(isUnderBottom[i - 1] == true && isUnderBottom[i] == false)
                {
                    result.Add(i);
                }

            } 
            return result.ToArray();
        }

        

        public override int[] FindSellTime(StockData data)
        {
            throw new NotImplementedException();
        }

       
    }
}
