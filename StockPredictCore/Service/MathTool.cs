using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockPredictCore
{
   public static class MathTool
    {
        public static double CalculateStdDev(List<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {  
                double avg = values.Average(); 
                double sum = 0;
                for(int i = 0;i < values.Count(); i++)
                {
                    sum += (values[i] - avg) * (values[i] - avg);
                }
                
               
                ret = Math.Sqrt(sum / values.Count());
            }
            return ret;
        }
    }
}
