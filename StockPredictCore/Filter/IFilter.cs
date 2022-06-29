using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
    public abstract class FilterBase
    { 
        public FilterBase(){}

        public FilterBase(double _initparam)
        {
            parameter = _initparam;
        }

        public FilterBase(IEnumerable<StockData> _stockDataList, double _param )
        {
            stockDataList = _stockDataList.ToList();
            parameter = _param;
        }

        public abstract void Execute();

        public double GetParam()
        {
            return parameter;
        }

        protected List<StockData> stockDataList;
        protected double parameter;
          
    }

    
}
