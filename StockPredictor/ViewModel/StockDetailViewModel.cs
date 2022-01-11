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

        //Ma5與Ma60交換發生時間點
        private List<FeatureInfo> ma5ma60MappingFeatureList = new List<FeatureInfo>();

        public List<FeatureInfo> Ma5ma60MappingFeatureList
        {
            get => ma5ma60MappingFeatureList;
            set { Set(() => Ma5ma60MappingFeatureList, ref ma5ma60MappingFeatureList, value); }
        }

        public StockDetailViewModel(StockData stockData)
        {
            StockID = stockData.ID;
            StockName = stockData.Name;



        }

        private List<FeatureInfo> GetMa5ma60MappingFeatureListData(StockData source)
        {
            List<FeatureInfo> result = new List<FeatureInfo>();



            return result;
        }
    }

    public class FeatureInfo
    {
        public DateTime Date { get; set; }

        public double ClosedPrice { get; set; }

        public string Status { get; set; }
    }
}
