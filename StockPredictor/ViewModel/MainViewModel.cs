using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using StockPredictCore;
using StockPredictCore.Filter;
using StockPredictor.Class;
using StockPredictor.Class.AlgoStategy;
using StockPredictor.Class.FilterInfo;
using StockPredictor.Class.SumResult;

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
        private ConcurrentBag<StockData> stockDataList = new ConcurrentBag<StockData>();
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
         
        private List<StockInfo> stockInfoCollection = new List<StockInfo>();

        public List<StockInfo> StockInfoCollection
        {
            get => stockInfoCollection;
            set { Set(() => StockInfoCollection, ref stockInfoCollection, value); }
        }

        private ObservableCollection<AlgoStrategy> algoStrategyCollection = new ObservableCollection<AlgoStrategy>();

        public ObservableCollection<AlgoStrategy> AlgoStrategyCollection
        {
            get => algoStrategyCollection;
            set { Set(() => AlgoStrategyCollection, ref algoStrategyCollection, value); }
        }

        private AlgoStrategy seletedAlgoStratrgy;

        public AlgoStrategy SeletedAlgoStratrgy
        {
            get => seletedAlgoStratrgy;
            set { Set(() => SeletedAlgoStratrgy, ref seletedAlgoStratrgy, value); }
        }
         

        public RelayCommand AddStrategyCommand { get; set; }
        public RelayCommand RemoveStrategyCommand { get; set; }
        public RelayCommand AnalysisCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public MainViewModel()
        {
            InitStockInfo(); 
            AnalysisCommand = new RelayCommand(AnalysisAction);
            AddStrategyCommand = new RelayCommand(AddStrategyAction);
            RemoveStrategyCommand = new RelayCommand(RemoveStrategyAction);
            CloseWindowCommand = new RelayCommand(CloseWindowAction);


            if (isDesign == false)
            {
                PreProcessData(Directory.GetFiles(DataParser.rawDataFolderPath));
            }


            AlgoStrategyCollection = new ObservableCollection<AlgoStrategy>(AlgoStrategyService.Load()); 

            if (AlgoStrategyCollection.Count == 0)
            {
                AlgoStrategyCollection.Add(new AlgoStrategy(true));
                AlgoStrategyCollection.Add(new AlgoStrategy(true));
            }
           
            SeletedAlgoStratrgy = algoStrategyCollection[0];
        }

        private void CloseWindowAction()
        {
            if (Directory.Exists(AlgoStrategyService.filePath) == false)
                Directory.CreateDirectory(AlgoStrategyService.filePath);

            foreach (var file in Directory.GetFiles(AlgoStrategyService.filePath))
            {
                File.Delete(file);
            }
            foreach (var algo in AlgoStrategyCollection)
            {
                algo.Save();
            }
        }
       

        private void RemoveStrategyAction()
        {
            if( SeletedAlgoStratrgy == null)
                return;

            int idx = AlgoStrategyCollection.IndexOf(SeletedAlgoStratrgy);

            AlgoStrategyCollection.Remove(SeletedAlgoStratrgy);

            if(idx>0)
              SeletedAlgoStratrgy = AlgoStrategyCollection[idx-1];

        }

        private void AddStrategyAction()
        {

            AlgoStrategyCollection.Add(new AlgoStrategy(true));
        }

        private void AnalysisAction()
        {
            ResetData(stockDataList);
            FilterService service = new FilterService();
            foreach (var selectedFilter in SeletedAlgoStratrgy.FilterInfoList.Where(_ => _.IsSelected))
            { 
                service.AddFilter(FilterFactory.CreatFilterByFilterType(selectedFilter.Type,selectedFilter.Param, stockDataList));
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

                        if(stockInfoDictionary.ContainsKey(stockData.ID) == false)
                            continue;
                          
                        var info = new StockInfo(stockData, i, stockData.ID, stockInfoDictionary[stockData.ID]);
                        info.CurrentClosePrice = lastestClosePrice;
                         
                        if(stockInfoList.Count(_ => _.Name == info.Name) == 0)
                             stockInfoList.Add(info);
                    }
                }
            });
             
            TotalSumResult = SumResultService.Create(stockInfoList);

            StockInfoCollection = new List<StockInfo>(stockInfoList.OrderByDescending(_ => _.Date).ToList());
        }

        private void PreProcessData(string[] stockFiles)
        {

            //foreach (var _ in stockFiles)
            //{
            //    StockData data = DataParser.ConvertData(_);
            //    PreProcessor preProcessor = new PreProcessor();
            //    preProcessor.Execute(data);
            //    stockDataList.Add(data);
            //}
            Parallel.ForEach(stockFiles, _ =>
            {
                StockData data = DataParser.ConvertData(_);
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);
                stockDataList.Add(data);
            });
        }

       

        private void ResetData(IEnumerable<StockData>  stockDataList)
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
            using (var reader = new StreamReader(@"StockInfoList\\stockInfo.csv"))
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

            DataParser.GetFinancialReport("FinanicalReport");
        }

        
    }

   
}