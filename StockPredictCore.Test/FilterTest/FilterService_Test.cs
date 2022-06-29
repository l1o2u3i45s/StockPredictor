using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using StockPredictCore.Filter;

namespace StockPredictCore.Test
{
    [TestClass]
    public class FilterService_Test
    {
        
        private FilterService _filterService  = new FilterService();
         
        [TestMethod]
        public void TestExecuteReallyExecute()
        {
            int count = 10000;
            List<IFilter> filters = new List<IFilter>();

            for (int i = 0; i < count; i++)
            {
                var fakeFilter = Substitute.For<IFilter>(); 
                filters.Add(fakeFilter);

                _filterService.AddFilter(fakeFilter);
            }

            _filterService.Execute();

            //make sure every filter is executed.
            foreach (var item in filters)
            {
                item.Received(1).Execute(); 
            } 
        }
    }
}
