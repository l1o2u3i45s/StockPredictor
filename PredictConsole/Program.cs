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
            List<string> stockCodeList = new List<string>();
            using (var reader = new StreamReader(@"StockInfofile\\stockIDList.txt"))
            {
                while (!reader.EndOfStream)
                {
                    stockCodeList.Add(reader.ReadLine());
                }
            }

            Task.Run(() => {
                DataParser.GetStockPriceData(new DateTime(2018, 1, 1), stockCodeList);
            });
           

           

            Console.WriteLine("Done");
            Console.ReadLine();
        }
         
    }
}
