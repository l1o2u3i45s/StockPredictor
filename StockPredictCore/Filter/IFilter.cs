using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public abstract class IFilter
    {
        public IFilter(List<StockData> _stockDataList)
        {
            stockDataList = _stockDataList;
        }

        public abstract void Execute();
         
        protected List<StockData> stockDataList;
    }
}
