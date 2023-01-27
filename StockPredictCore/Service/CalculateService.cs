using System;
using InfraStructure;

namespace StockPredictCore.Service
{

    public struct BoollingData
    {
        public BoollingData(double[] lowerLimit, double[] upperLimit)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        public double[] LowerLimit { get; }
        public double[] UpperLimit { get; }
    }
    public interface ICalculateService
    {
        double[] Cal_MA(double[] data, int avgDays);

        BoollingData Cal_Boolling(double[] closedPrice, int avgDays = 20, int std = 2);

        double[] Cal_RSI(double[] closedPrice, int calculateDays);
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
                return new BoollingData(Array.Empty<double>(), Array.Empty<double>());

            double[] lowerLimit = new double[closedPrice.Length];
            double[] upperLimit = new double[closedPrice.Length];
            var maData = Cal_MA(closedPrice, avgDays);

            for (int i = avgDays - 1; i < closedPrice.Length; i++)
            {
                double stdValue = Math.Pow(Math.Pow(closedPrice[i] - maData[i], 2) / avgDays, 0.5);

                lowerLimit[i] = closedPrice[i] - stdThreshold * stdValue;
                upperLimit[i] = closedPrice[i] + stdThreshold * stdValue;
            }

            return new BoollingData(lowerLimit, upperLimit);
        }

        public double[] Cal_RSI(double[] closedPrice, int calculateDays)
        {
            int dataCount = closedPrice.Length;
            double[] result = new double[dataCount];

            int first = 1, last = 1;

            double increaseSum = 0;
            double decreaseSum = 0;

            while (last < dataCount)
            {
                var value = closedPrice[last] - closedPrice[last - 1];
                if (value > 0)
                    increaseSum += value;
                else
                    decreaseSum += Math.Abs(value);

                if (last - first >= dataCount - 2)
                {
                    double increaseAvg = increaseSum / calculateDays;
                    double decreaseAvg = decreaseSum / calculateDays;

                    double rsi = (increaseAvg / (increaseAvg + decreaseAvg)) * 100;
                    result[last] = rsi;

                    if (value > 0)
                        increaseSum -= value;
                    else
                        decreaseSum -= value;

                    first++;
                }
                last++;
            }

            return result;
        }


    }
}
