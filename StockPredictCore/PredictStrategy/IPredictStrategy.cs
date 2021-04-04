using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore
{
    public abstract class IPredictStrategy
    { 
        public abstract int[] FindBuyTime(StockData data); 
        public abstract int[] FindSellTime(StockData data);
    }
}
