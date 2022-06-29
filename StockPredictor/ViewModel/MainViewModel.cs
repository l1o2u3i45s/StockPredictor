using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using StockPredictCore;
using StockPredictCore.Filter;
using StockPredictCore.Service;
using StockPredictor.ApplicationService;
using StockPredictor.Class;
using StockPredictor.Class.AlgoStategy;
using StockPredictor.Class.FilterInfo;
using StockPredictor.Class.SumResult;
using StockPredictor.UserControl;

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
        private ConcurrentBag<StockData> stockDataList;
        private ConcurrentBag<FinancialStatementsData> financialStatementsDataList;
        private ConcurrentBag<PERatioTableData> peRatioTableDataDataList;
        private ConcurrentBag<InvestInstitutionBuySellData> investInstitutionBuySellDataDataList;
        private Dictionary<string, string> stockInfoDictionary = new Dictionary<string, string>();

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set { Set(() => IsBusy, ref _isBusy, value); }
        }

        private string _busyContent;

        public string BusyContent
        {
            get => _busyContent;
            set { Set(() => BusyContent, ref _busyContent, value); }
        }

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

        private double totalDiffValue;

        public double TotalDiffValue
        {
            get => totalDiffValue;
            set { Set(() => TotalDiffValue, ref totalDiffValue, value); }
        }

        private eSelectTabType selectedTabItemType;

        public eSelectTabType SelectedTabItemType
        {
            get => selectedTabItemType;
            set
            {
                Set(() => SelectedTabItemType, ref selectedTabItemType, value);
            }
        }

        private List<StockInfo> stockInfoCollection = new List<StockInfo>();

        public List<StockInfo> StockInfoCollection
        {
            get => stockInfoCollection;
            set { Set(() => StockInfoCollection, ref stockInfoCollection, value); }
        }

        private StockInfo selectedStockInfo;

        public StockInfo SelectedStockInfo
        {
            get => selectedStockInfo;
            set { Set(() => SelectedStockInfo, ref selectedStockInfo, value); }
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

        private DataCenterViewModel dataCenterViewModel;

        public DataCenterViewModel DataCenterViewModel
        {
            get => dataCenterViewModel;
            set { Set(() => DataCenterViewModel, ref dataCenterViewModel, value); }
        }

        private RegularQuotaViewModel regularQuotaViewModel;

        public RegularQuotaViewModel RegularQuotaViewModel
        {
            get => regularQuotaViewModel;
            set { Set(() => RegularQuotaViewModel, ref regularQuotaViewModel, value); }
        }

        private RegularQuotaProfitCaculateViewModel regularQuotaProfitCaculateViewModel;

        public RegularQuotaProfitCaculateViewModel RegularQuotaProfitCaculateViewModel
        {
            get => regularQuotaProfitCaculateViewModel;
            set { Set(() => RegularQuotaProfitCaculateViewModel, ref regularQuotaProfitCaculateViewModel, value); }
        }
        

        private ObservableCollection<StockDetailViewModel> stockDetailViewModelList = new ObservableCollection<StockDetailViewModel>();

        public ObservableCollection<StockDetailViewModel> StockDetailViewModelList
        {
            get => stockDetailViewModelList;
            set { Set(() => StockDetailViewModelList, ref stockDetailViewModelList, value); }
        }

        public RelayCommand AddStrategyCommand { get; set; }
        public RelayCommand RemoveStrategyCommand { get; set; }
        public RelayCommand AnalysisCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand<TabControl> CreateStockDetailCommand { get; set; }


        public MainViewModel()
        {
            DataCenterViewModel = new DataCenterViewModel();
            DataCenterViewModel.SetUpdateDataDoneCallback(UpdateStockData); 
            InitStockInfo();
            UpdateStockData();
            InitAlgoStrategy();
            RegularQuotaViewModel = new RegularQuotaViewModel(stockDataList);
            RegularQuotaProfitCaculateViewModel = new RegularQuotaProfitCaculateViewModel();
            AnalysisCommand = new RelayCommand(AnalysisAction);
            AddStrategyCommand = new RelayCommand(AddStrategyAction);
            RemoveStrategyCommand = new RelayCommand(RemoveStrategyAction);
            CloseWindowCommand = new RelayCommand(CloseWindowAction);
            CreateStockDetailCommand = new RelayCommand<TabControl>(CreateStockDetailAction);
        }

        private void CreateStockDetailAction(TabControl tabControl)
        {
            if (SelectedStockInfo == null)
                return;

            if (StockDetailViewModelList.Any(_ => _.StockID == SelectedStockInfo.ID))
            {
                foreach (TabItem item in tabControl.Items)
                {
                    if (item.Header.ToString() == $"{SelectedStockInfo.ID} {SelectedStockInfo.Name}")
                    {
                        tabControl.SelectedIndex = tabControl.Items.IndexOf(item);
                        break;
                    }
                }
                return;
            }

            var data = stockDataList.Single(_ => _.ID == SelectedStockInfo.ID);

            StockDetailViewModel newTabVM = new StockDetailViewModel(data);
            ucStockDetail ucStockDetail = new ucStockDetail(){DataContext = newTabVM};
            TabItem newTabItem = new TabItem
            {
                Header = $"{SelectedStockInfo.ID} {SelectedStockInfo.Name}",
                Content = ucStockDetail,
                Tag = eSelectTabType.StockDetail
            };
             
            StockDetailViewModelList.Add(newTabVM);
            tabControl.Items.Add(newTabItem);

            tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        private void UpdateStockData()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (sender, args) =>
            {
                if (isDesign == false) 
                    PreProcessData(Directory.GetFiles(DataParser.StockRawDataPath)); 
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                IsBusy = false;  
            };

            IsBusy = true;
            BusyContent = "Update Stock Data";


            worker.RunWorkerAsync();
        }

        private void InitAlgoStrategy()
        {
            AlgoStrategyCollection = new ObservableCollection<AlgoStrategy>(AlgoStrategyService.Load());

            if (AlgoStrategyCollection.Count == 0)
            {
                AlgoStrategyCollection.Add(new AlgoStrategy(true));
                AlgoStrategyCollection.Add(new AlgoStrategy(true));
            }

            SeletedAlgoStratrgy = algoStrategyCollection.FirstOrDefault();
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
            if (SeletedAlgoStratrgy == null)
                return;
             
            AlgoStrategyCollection.Remove(SeletedAlgoStratrgy); 
            SeletedAlgoStratrgy = AlgoStrategyCollection.LastOrDefault(); 
        }

        private void AddStrategyAction()
        { 
            AlgoStrategyCollection.Add(new AlgoStrategy(true));
        }

        private void AnalysisAction()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (sender, args) =>
            {
                ResetData(stockDataList);
                FilterService service = new FilterService();
                foreach (var selectedFilter in SeletedAlgoStratrgy.FilterInfoList.Where(_ => _.IsSelected))
                {
                    service.AddFilter(FilterFactory.CreatFilterByFilterType(selectedFilter.Type, selectedFilter.Param, stockDataList));
                }
                service.Execute();


                StockInfoService stockInfoService = new StockInfoService(); 
                var stockInfoList = stockInfoService.FilterData(stockDataList, stockInfoDictionary, startTime, endTime);
                 
                TotalSumResult = SumResultFactory.Create(stockInfoList); 
                StockInfoCollection = new List<StockInfo>(stockInfoList.OrderByDescending(_ => _.Date).ToList());
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                IsBusy = false;
            };

            IsBusy = true;
            BusyContent = "Analysis Data....";

            worker.RunWorkerAsync();
        }

        private void PreProcessData(string[] stockFiles)
        {
            GetFinancialStatementsData(Directory.GetFiles(DataParser.FinancialStatementPath));
            GetPERatioTableData(Directory.GetFiles(DataParser.PERatioTablePath));
            GetInvestInstitutionBuySellDataData(Directory.GetFiles(DataParser.TaiwanStockInstitutionalInvestorsBuySellPath));

            stockDataList = new ConcurrentBag<StockData>();
            //foreach (var _ in stockFiles)
            //{
            //    StockData data = DataParser.GetStockData(_);
            //    PreProcessor preProcessor = new PreProcessor();
            //    preProcessor.Execute(data);
            //    stockDataList.Add(data);
            //}
           
            Parallel.ForEach(stockFiles, _ =>
            {
                StockData data = DataParser.GetStockData(_);
                if (stockInfoDictionary.ContainsKey(data.ID))
                    data.Name = stockInfoDictionary[data.ID];
                  
                data.UpdateInstitutionBuySellData(investInstitutionBuySellDataDataList.Where(x => x.StockID == data.ID).ToList());
                 
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);
                stockDataList.Add(data);
            });
        }

        private void GetFinancialStatementsData(string[] stockFiles)
        {
            financialStatementsDataList = new ConcurrentBag<FinancialStatementsData>();

            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetFinancialStatementsData(_))
                {
                    financialStatementsDataList.Add(data);
                }
            });
        }

        private void GetPERatioTableData(string[] stockFiles)
        {
            peRatioTableDataDataList = new ConcurrentBag<PERatioTableData>();

            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetPERatioTableData(_))
                {
                    peRatioTableDataDataList.Add(data);
                }
            });
        }

        private void GetInvestInstitutionBuySellDataData(string[] stockFiles)
        {
            investInstitutionBuySellDataDataList = new ConcurrentBag<InvestInstitutionBuySellData>();
            Parallel.ForEach(stockFiles, _ =>
            {
                foreach (var data in DataParser.GetInvestInstitutionBuySellData(_))
                {
                    investInstitutionBuySellDataDataList.Add(data);
                }
            });
        }

        private void ResetData(IEnumerable<StockData> stockDataList)
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
            if (Directory.Exists(DataParser.StockRawDataPath) == false)
                Directory.CreateDirectory(DataParser.StockRawDataPath);

            if (Directory.Exists(DataParser.PERatioTablePath) == false)
                Directory.CreateDirectory(DataParser.PERatioTablePath);

            if (Directory.Exists(DataParser.FinancialStatementPath) == false)
                Directory.CreateDirectory(DataParser.FinancialStatementPath);

            CSVParser csvParser = new CSVParser();
            stockInfoDictionary = csvParser.GetStockInfo(@"StockInfoList\\stockInfo.csv"); 
        }
         
    }

    public enum eSelectTabType
    {
        StockFilter,
        DataCenter,
        StockDetail
    }

}