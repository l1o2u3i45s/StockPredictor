using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockPredictCore.Filter.Value;

namespace StockPredictCore.Service
{

    public struct BoollingData
    {
        public BoollingData(double[] lowerLimit,double[] upperLimit)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public double[] LowerLimit { get;}
        public double[] UpperLimit { get;}
    }
    public interface ICalculateService
    {
        double[] Cal_MA(double[] data, int avgDays);

        BoollingData Cal_Boolling(double[] closedPrice,int avgDays = 20,int std = 2);
    }
    public class CalculateService : ICalculateService
    {
        public double[] Cal_MA(double[] data, int avgDays)
        {
            if (data == null)
                return Array.Empty<double>();

            double[] result = new double[data.Length];


            int first = 0, last = 0;

            double sumresult = 0;
            while (last < data.Length)
            {
                sumresult += data[last];

                
                if (last - first >= avgDays - 1)
                {
                    result[last] = sumresult / avgDays;
                    sumresult -= data[first];
                    first++;
                }
                last++;
            }
            return result;
        }

        public BoollingData Cal_Boolling(double[] closedPrice, int avgDays = 20, int stdThreshold = 2)
        {
            if (closedPrice is null || closedPrice.Length < avgDays)
                return new BoollingData( Array.Empty<double>(),Array.Empty<double>());

            double[] lowerLimit = new double[closedPrice.Length];
            double[] upperLimit = new double[closedPrice.Length];
            var maData = Cal_MA(closedPrice, avgDays);

            for (int i = avgDays - 1; i < closedPrice.Length; i++)
            {
                double stdValue = Math.Pow( Math.Pow(closedPrice[i] - maData[i],2) / avgDays ,0.5);

                lowerLimit[i] = closedPrice[i] - stdThreshold * stdValue;
                upperLimit[i] = closedPrice[i] + stdThreshold * stdValue;
            }

            return new BoollingData(lowerLimit, upperLimit);
        }
    }
}
