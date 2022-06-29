using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockPredictCore.Filter.AverageLine;

namespace StockPredictCore.Test.FilterTest
{
    [TestClass]
    public class AverageLineFilter_Test
    {

        [TestMethod]
        public void Ma5DecreaseFilter_Test()
        {



            IEnumerable<StockData> _stockDataList = new List<StockData>();
            double _param = 0.5;

            Ma5DecreaseFilter filter = new Ma5DecreaseFilter(_stockDataList,_param);

        }

        private StockData CreateFakeStockData(int size)
        {
            StockData result = new StockData(size, "fake");




            return result;
        }

    }
}
