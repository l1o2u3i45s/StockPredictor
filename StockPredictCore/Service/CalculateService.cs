using InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore.Service
{
    public interface ICalculateService
    {
        double[] Cal_MA(double[] data, int avgDays);
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
    }
}
