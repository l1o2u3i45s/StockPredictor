using InfraStructure;
using StockPredictCore; 
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
                   
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
         
    }
}
