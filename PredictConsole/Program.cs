using InfraStructure;
using StockPredictCore;
using StockPredictCore.PredictStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictConsole
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string fileFolder = @"E:\\StockData";
            var stockFiles = Directory.GetFiles(fileFolder);

            foreach(var stockpath in stockFiles)
            { 
                StockData data = DataParser.GrabData(stockpath);
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);
                HightRiskPredictStrategy strategy = new HightRiskPredictStrategy();
                var result = strategy.FindBuyTime(data);
                for (int i = 0; i < result.Length; i++)
                {
                    int idx = result[i];
                    if(data.Date[idx] == DateTime.Today.AddDays(-1))
                    {
                        Console.WriteLine(stockpath);
                        Console.WriteLine("時間: " + data.Date[idx]);
                    }
                   
                }
            }

            Console.WriteLine("Done");

        }
    }
}
