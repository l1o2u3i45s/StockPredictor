using InfraStructure;
using StockPredictCore;
using StockPredictCore.PredictStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            StockData data = DataParser.GrabData(@"E:\\StockData\\3653.TW");
            PreProcessor preProcessor = new PreProcessor();
            preProcessor.Execute(data);
            HightRiskPredictStrategy strategy = new HightRiskPredictStrategy();
            var result = strategy.FindBuyTime(data);
            for(int i = 0;i < result.Length; i++)
            {
                int idx = result[i];
                Console.WriteLine("時間: " + data.Date[idx]);
            }
            Console.ReadLine();
        }
    }
}
