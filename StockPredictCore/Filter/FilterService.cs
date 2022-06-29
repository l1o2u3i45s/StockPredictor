﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraStructure;

namespace StockPredictCore.Filter
{
   public class FilterService
   {
       private List<IFilter> filters = new List<IFilter>();

       public void AddFilter(IFilter filter)
       {
           filters.Add(filter);
       }

       public void Execute()
       {
           foreach (var filter in filters)
           {
               filter.Execute();
           }
       }

       

   }
}
