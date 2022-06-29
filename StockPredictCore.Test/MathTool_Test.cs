using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockPredictCore.Test
{
    [TestClass]
    public class MathTool_Test
    {
         

        [TestMethod]
        public void CalculateStdDevTest()
        {

            List<double> data = new List<double>() { 1, 2, 3, 4, 5 };
            var result = MathTool.CalculateStdDev(data);
            var answer = 1.4142;
            Assert.AreEqual(answer, Math.Round(result,4));
        }
    }
}
