using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore
{
    public interface IPredictStrategy
    {
       int[] FindBuyTime { get; set; }
       int[] FindSellTime { get; set; }
    }
}
