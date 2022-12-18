using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore.Service
{
    public interface ICSVParser
    {
        Dictionary<string, string> GetStockInfo(string path);
    }
    public class CSVParser : ICSVParser
    {

        public Dictionary<string, string> GetStockInfo(string path)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            using (var reader = new StreamReader(path))
            {
                bool isTitle = true;
                while (!reader.EndOfStream)
                {
                    if (isTitle)
                    {
                        reader.ReadLine();
                        isTitle = false;
                    }

                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string code = values[1].ToString().Replace('"', ' ').Trim();
                    string companyName = values[3].Replace('"', ' ').Trim();
                    result.Add(code, companyName);
                }
            }

            return result;
        }
    }
}
