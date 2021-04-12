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
            result.Date = new DateTime[lines.Length];
            result.OpenPrice = new double[lines.Length];
            result.ClosePrice = new double[lines.Length];
            result.HightestPrice = new double[lines.Length];
            result.LowestPrice = new double[lines.Length];
            result.Volumn = new double[lines.Length];
            result.KValue = new double[lines.Length];
            result.DValue = new double[lines.Length];
            result.MA20 = new double[lines.Length];
            result.MA5 = new double[lines.Length];
            result.RSV = new double[lines.Length];
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
