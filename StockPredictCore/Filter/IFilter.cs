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
        public IFilter(IEnumerable<StockData> _stockDataList, double[] _param = null)
        {
            stockDataList = _stockDataList.ToList();
            parameter = _param;
        }

        public abstract void Execute();
         
        protected List<StockData> stockDataList;
        protected double[] parameter;
    }
}
