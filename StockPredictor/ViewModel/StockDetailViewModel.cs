using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using InfraStructure;
using StockPredictCore.Filter.AverageLine;

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
        private List<FeatureInfo> ma5ma60MappingFeatureList;

        public List<FeatureInfo> Ma5ma60MappingFeatureList
        {
            get => ma5ma60MappingFeatureList;
            set { Set(() => Ma5ma60MappingFeatureList, ref ma5ma60MappingFeatureList, value); }
        }

        public StockDetailViewModel(StockData stockData)
        {
            StockID = stockData.ID;
            StockName = stockData.Name;
             
            Ma5ma60MappingFeatureList = GetMa5ma60MappingFeatureListData(stockData);
        }

        private List<FeatureInfo> GetMa5ma60MappingFeatureListData(StockData source)
        {
            for (int i = 0; i < source.IsFilter.Length; i++)
            {
                source.IsFilter[i] = false;
            }
            List<FeatureInfo> result = new List<FeatureInfo>();
            List<StockData> data = new List<StockData>(){ source };
            Ma5OverMa60Filter ma5OverMa60Filter = new Ma5OverMa60Filter(data, 0);
            Ma60OverMa5Filter ma60OverMa5Filter = new Ma60OverMa5Filter(data, 0);

            ma5OverMa60Filter.Execute();
            
            source = data.First();
            for (int i = 0; i < source.Date.Length; i++)
            {
                if(source.IsFilter[i])
                    continue;

                FeatureInfo feature = new FeatureInfo()
                {
                    Date = source.Date[i],
                    ClosedPrice = source.ClosePrice[i],
                    Status = "Ma5穿過Ma60"
                };
                result.Add(feature);
            }

            for (int i = 0; i < source.IsFilter.Length; i++)
            {
                source.IsFilter[i] = false;
            }
            ma60OverMa5Filter.Execute();

            for (int i = 0; i < source.Date.Length; i++)
            {
                if (source.IsFilter[i])
                    continue;

                FeatureInfo feature = new FeatureInfo()
                {
                    Date = source.Date[i],
                    ClosedPrice = source.ClosePrice[i],
                    Status = "Ma60穿過Ma5"
                };
                result.Add(feature);
            }

            result = result.OrderByDescending(_ => _.Date).ToList();

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
