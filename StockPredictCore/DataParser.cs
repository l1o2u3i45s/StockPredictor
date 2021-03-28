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
        public List<StockData> GrabData(string filepath)
        {
            List<StockData> result = new List<StockData>();

            string[] lines = System.IO.File.ReadAllLines(filepath);

            for (int i = 1; i < lines.Length; i++)
            {
                result.Add((StockData)lines[i]);
            }
             
            return result;
        }
    }
}
