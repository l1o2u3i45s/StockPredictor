using System;
using System.Collections.Generic;
using System.IO;
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
            string[] lines = System.IO.File.ReadAllLines(filepath);
            StockData result = new StockData(lines.Length, Path.GetFileNameWithoutExtension(filepath)); 

            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                if (data.Count(_ => string.IsNullOrEmpty(_)) > 0)
                    continue;

                result.Date[i] = Convert.ToDateTime(data[0]);
                result.OpenPrice[i] = Convert.ToDouble(data[1]);
                result.HightestPrice[i] = Convert.ToDouble(data[2]);
                result.LowestPrice[i] = Convert.ToDouble(data[3]);
                result.ClosePrice[i] = Convert.ToDouble(data[4]);
                result.Volumn[i] = Convert.ToDouble(data[5]);
                result.KValue[i] = 0;
                result.DValue[i] = 0;
                result.MA20[i] = 0;
                result.MA5[i] = 0;
            }

            return result;
        }
    }
}
