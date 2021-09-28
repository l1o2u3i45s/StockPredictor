using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using InfraStructure;

namespace StockPredictor.ViewModel
{
    public class StockDetailViewModel : ViewModelBase
    {
        private string stockID;

        public string StockID
        {
            get => stockID;
            set { Set(() => StockID, ref stockID, value); }
        }

        private string stockName;

        public string StockName
        {
            get => stockName;
            set { Set(() => StockName, ref stockName, value); }
        }
         
        public StockDetailViewModel(StockData stockData)
        {
            StockID = stockData.ID;
            StockName = stockData.Name;
        }
    }
}
