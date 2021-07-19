using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StockPredictor.Class
{
    public class AlgoStrategy
    {
        public string Name { get; set; }

        public List<FilterInfo.FilterInfo> FilterInfoP { get; set; }

        public AlgoStrategy()
        {

        }
    }
}
