using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore
{
    public class DataParser
    {
        public static StockData GrabData(string filepath)
        {
            StockData result = new StockData();

            string[] lines = System.IO.File.ReadAllLines(filepath);

            for (int i = 1; i < lines.Length; i++)
            {
                
            }
             
            return result;
        }
    }
}
