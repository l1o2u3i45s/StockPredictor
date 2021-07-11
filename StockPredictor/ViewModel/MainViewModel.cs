using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using StockPredictCore;
using StockPredictCore.Filter;
using StockPredictor.Class;

namespace StockPredictor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private bool isDesign = false;
        const string fileFolder = @"E:\\StockData";
        private List<StockData> stockDataList = new List<StockData>();
        private Dictionary<string, string> stockInfoDictionary = new Dictionary<string, string>();
        private DateTime startTime = DateTime.Today.AddDays(-7);

        public DateTime StartTime
        {
            get => startTime;
            set { Set(() => StartTime, ref startTime, value); }
        }

        private DateTime endTime = DateTime.Today;

        public DateTime EndTime
        {
            get => endTime;
            set { Set(() => EndTime, ref endTime, value); }
        }

        private ObservableCollection<FilterInfo> filterInfoCollection = new ObservableCollection<FilterInfo>();

        public ObservableCollection<FilterInfo> FilterInfoCollection
        {
            get => filterInfoCollection;
            set { Set(() => FilterInfoCollection, ref filterInfoCollection, value); }
        }

        private ObservableCollection<StockInfo> stockInfoCollection = new ObservableCollection<StockInfo>();

        public ObservableCollection<StockInfo> StockInfoCollection
        {
            get => stockInfoCollection;
            set { Set(() => StockInfoCollection, ref stockInfoCollection, value); }
        }

        public RelayCommand AnalysisCommand { get; set; }

        public MainViewModel()
        {
            InitStockInfo();
            InitFilterList();
            AnalysisCommand = new RelayCommand(AnalysisAction);
            if (isDesign == false)
            {
                PreProcessData(Directory.GetFiles(fileFolder));
            }
                
        }

        private void AnalysisAction()
        {
            ResetData(stockDataList);
            FilterService service = new FilterService();
            foreach (var selectedFilter in FilterInfoCollection.Where(_ => _.IsSelected))
            { 
                service.AddFilter(FilterFactory.CreatFilterByFilterType(selectedFilter.Type, stockDataList));
            }
            service.Execute();

            List<StockInfo> stockInfoList = new List<StockInfo>();

            //Parallel.ForEach(stockDataList, stockData =>
            //{
            //    for (int i = 0; i < stockData.Date.Length; i++)
            //    {
            //        if (stockData.IsFilter[i] == false && stockData.Date[i] >= StartTime && stockData.Date[i] <= EndTime)
            //            stockInfoList.Add(new StockInfo(stockData, i, stockData.ID));
            //    }
            //});
            foreach (var stockData in stockDataList)
            {
                for (int i = 0; i < stockData.Date.Length; i++)
                {
                    if (stockData.IsFilter[i] == false && stockData.Date[i] >= StartTime && stockData.Date[i] <= EndTime)
                        stockInfoList.Add(new StockInfo(stockData, i, stockData.ID,stockInfoDictionary[stockData.ID]));
                }
            }

            StockInfoCollection = new ObservableCollection<StockInfo>(stockInfoList.OrderByDescending(_ => _.Date).ToList());
        }

        private void PreProcessData(string[] stockFiles)
        {
            Parallel.ForEach(stockFiles, _ =>
            {
                StockData data = DataParser.GrabData(_);
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);
                stockDataList.Add(data);
            });
        }

        private void InitFilterList()
        {
            foreach (FilterType type in Enum.GetValues(typeof(FilterType)))
            {
                FilterInfo fileInfo = new FilterInfo();
                fileInfo.Type = type;
                FilterInfoCollection.Add(fileInfo);
            }
            
        }

        private void ResetData(List<StockData>  stockDataList)
        {
            Parallel.ForEach(stockDataList, stockData =>
            {
                for (int i = 0; i < stockData.Date.Length; i++)
                {
                    stockData.IsFilter[i] = false;
                }
            });
        }

        private void InitStockInfo()
        {
            using (var reader = new StreamReader(@"stockInfo.csv"))
            {
                bool isTitle = true;
                while (!reader.EndOfStream)
                { 
                    if (isTitle)
                    {
                        reader.ReadLine();
                        isTitle = false;
                    }

                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    string code = values[1].ToString().Replace('"',' ').Trim(); 
                    string companyName = values[3].Replace('"',' ').Trim();
                    stockInfoDictionary.Add( code, companyName);
                }
            }
        }
    }

   
}