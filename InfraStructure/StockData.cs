using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{
    public class StockData
    {
        public StockData(string sData)
        {

        }
        public DateTime Date { get; set; }
        public double OpenPrice { get; set; }
        public double ClosePrice { get; set; }
        public double HightestPrice { get; set; }
        public double LowestPrice { get; set; }
        public double Volumn { get; set; }

        public static explicit operator StockData(string sData) => new StockData(sData);
    }
}
