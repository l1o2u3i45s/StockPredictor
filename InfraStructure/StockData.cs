using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{
    public class StockData
    {
      
        public DateTime[] Date { get; set; }
        public double[] OpenPrice { get; set; }
        public double[] ClosePrice { get; set; }
        public double[] HightestPrice { get; set; }
        public double[] LowestPrice { get; set; }
        public double[] Volumn { get; set; }

        public double[] RSV { get; set; }
        public double[] KValue { get; set; }
        public double[] DValue { get; set; }

        public double[] MA20 { get; set; }
        public double[] MA5 { get; set; }
    }
}
