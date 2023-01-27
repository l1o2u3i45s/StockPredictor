using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using InfraStructure;
using StockPredictCore.Service;
using System.IO;

namespace StockPredictCore.Test.ServiceTest
{
    /// <summary>
    /// Summary description for CalculateServiceTest
    /// </summary>
    [TestClass]
    public class CalculateServiceTest
    {
        private readonly CalculateService calculateService = new CalculateService();
        public CalculateServiceTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod]
        public void Test_MA_0to5()
        {
            double[] data = new double[5] { 1, 2, 3, 4, 5 };
            int avgDays = 5;
            var result = calculateService.Cal_MA(data, avgDays);

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
            Assert.AreEqual(0, result[2]);
            Assert.AreEqual(0, result[3]);
            Assert.AreEqual(3, result[4]);


        }

        [TestMethod]
        public void Test_MA_0to100()
        {

            int avgDays = 10;
            double[] data = new double[100];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }

            var result = calculateService.Cal_MA(data, avgDays);

            Assert.AreEqual(5.5, result[10]);

        }

        [TestMethod]
        public void Test_Boolling_Simple()
        {
            int avgDays = 20;
            double[] data = new double[20];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = i;
            }

            var result = calculateService.Cal_Boolling(data);

            Assert.AreEqual(14.75, Math.Round(result.LowerLimit[19], 2));
            Assert.AreEqual(23.25, Math.Round(result.UpperLimit[19], 2));
        }

        [TestMethod]
        public void Test_RSI_Simple()
        {
            double[] data = new double[10]
            {
                23.7,
                27.9,
                26.5,
                29.6,
                31.1,
                29.4,
                25.5,
                28.9,
                20.5,
                23.2
            };

            calculateService.Cal_RSI(data,10);
        }
    }
}
