using System;
using System.Collections.Concurrent;
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

        
        private SumResult totalSumResult = new SumResult();

        public SumResult TotalSumResult
        {
            get => totalSumResult;
            set { Set(() => TotalSumResult, ref totalSumResult, value); }
        }

        private double totalDiffValue ;

        public double TotalDiffValue
        {
            get => totalDiffValue;
            set { Set(() => TotalDiffValue, ref totalDiffValue, value); }
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

            ConcurrentBag<StockInfo> stockInfoList = new ConcurrentBag<StockInfo>();

            Parallel.ForEach(stockDataList, stockData =>
            {
                for (int i = 0; i < stockData.Date.Length; i++)
                {

                    if (stockData.IsFilter[i] == false && stockData.Date[i] >= StartTime &&
                        stockData.Date[i] <= EndTime)
                    {
                        double lastestClosePrice = Math.Round(stockData.ClosePrice[stockData.Date.Length - 1], 2);

                        var info = new StockInfo(stockData, i, stockData.ID, stockInfoDictionary[stockData.ID]);
                        info.CurrentClosePrice = lastestClosePrice;

                        stockInfoList.Add(info);
                    }
                }
            });


            TotalSumResult.TotalPrice = Math.Round(stockInfoList.Sum(_ => _.ClosePrice), 2);
            TotalSumResult.DiffPrice = Math.Round(stockInfoList.Sum(_ => _.CloseDiffValue), 2);
            TotalSumResult.GrowRatio = Math.Round((  (TotalSumResult.TotalPrice + TotalSumResult.DiffPrice) / TotalSumResult.TotalPrice   - 1) * 100,2);
            TotalSumResult.WinAmount = stockInfoList.Count(_ => _.CloseDiffValue > 0);
            TotalSumResult.LoseAmount = stockInfoList.Count(_ => _.CloseDiffValue <= 0);
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

        public class SumResult:ObservableObject
        {
            private double totalPrice;
            public double TotalPrice
            {
                get => totalPrice;
                set { Set(() => TotalPrice, ref totalPrice, value); }
            }
          
            private double diffPrice;
            public double DiffPrice
            {
                get => diffPrice;
                set { Set(() => DiffPrice, ref diffPrice, value); }
            }

            private double growRatio;
            public double GrowRatio
            {
                get => growRatio;
                set { Set(() => GrowRatio, ref growRatio, value); }
            }

            private int winAmount;
            public int WinAmount
            {
                get => winAmount;
                set { Set(() => WinAmount, ref winAmount, value); }
            }

            private int loseAmount;
            public int LoseAmount
            {
                get => loseAmount;
                set { Set(() => LoseAmount, ref loseAmount, value); }
            }
        }
    }

   
}