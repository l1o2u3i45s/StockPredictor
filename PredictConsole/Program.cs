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
             
            foreach (var stockpath in stockFiles)
            { 
                StockData data = DataParser.GrabData(stockpath);
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);

                 
                IPredictStrategy strategy = new BollingerBandsStrategy();
                var result = strategy.FindBuyTime(data);
                strategy = new HightRiskPredictStrategy();
                var result2 = strategy.FindBuyTime(data);

                List<int> mergeData = new List<int>();
                foreach(var a in result2)
                {
                    foreach(var b in result)
                    {
                        if(a > b-4 && a > b + 4)
                        {
                            if(mergeData.Contains(a) == false)
                                mergeData.Add(a);
                        }
                    }
                }
                
                for (int i = 0; i < mergeData.Count; i++)
                {
                    int idx = mergeData[i];
                    if (data.Date[idx] >= DateTime.Today.AddDays(-1))
                    {
                        Console.WriteLine(stockpath);
                        Console.WriteLine("時間: " + data.Date[idx]);
                    }
                   
                }
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
         
    }
}
