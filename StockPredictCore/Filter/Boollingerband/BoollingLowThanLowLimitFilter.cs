﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter.Boollingerband
{ 
    public class BoollingLowThanLowLimitFilter : FilterBase
    {
        public BoollingLowThanLowLimitFilter()
        {
            
        }

        public BoollingLowThanLowLimitFilter(double initParam) : base(initParam)
        {

        }
        public BoollingLowThanLowLimitFilter(IEnumerable<StockData> _stockDataList, double param) : base(_stockDataList, param)
        {
        }

        public override void Execute()
        { 
            for (int i = 0; i < stockDataList.Count; i++)
            {
                var currentData = stockDataList[i];

                for (int j = 1; j < currentData.Date.Length; j++)
                {
                    if (currentData.IsFilter[j])
                        continue;

                    bool isCorrespond = currentData.ClosePrice[j] <= currentData.BoollingLowLimit[j];

                    

                    if (isCorrespond == false)
                        currentData.IsFilter[j] = true;
                }
            }
        }
    }
}
