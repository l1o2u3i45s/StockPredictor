using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using StockPredictCore.Filter;
using StockPredictor.ApplicationService;
using StockPredictor.Class;
using StockPredictor.Class.AlgoStategy;
using StockPredictor.Class.SumResult;
using StockPredictor.UserControl;

namespace StockPredictor.ViewModel
{
    public class StockFilterViewModel: ViewModelBase
    {
        private readonly ConcurrentBag<StockData> _stockDataList;
        private readonly Dictionary<string, string> _stockInfoDictionary = new Dictionary<string, string>();

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

        private ObservableCollection<StockDetailViewModel> stockDetailViewModelList = new ObservableCollection<StockDetailViewModel>();

        public ObservableCollection<StockDetailViewModel> StockDetailViewModelList
        {
            get => stockDetailViewModelList;
            set { Set(() => StockDetailViewModelList, ref stockDetailViewModelList, value); }
        }

        private SumResult totalSumResult = new SumResult();

        public SumResult TotalSumResult
        {
            get => totalSumResult;
            set { Set(() => TotalSumResult, ref totalSumResult, value); }
        }


        public RelayCommand AddStrategyCommand { get; set; }
        public RelayCommand RemoveStrategyCommand { get; set; }
        public RelayCommand AnalysisCommand { get; set; }
        public RelayCommand<TabControl> CreateStockDetailCommand { get; set; }

        public StockFilterViewModel(ConcurrentBag<StockData> stockDataList, Dictionary<string, string> stockInfoDictionary)
        {
            _stockDataList = stockDataList;
            _stockInfoDictionary = stockInfoDictionary;
            InitAlgoStrategy();
            AnalysisCommand = new RelayCommand(AnalysisAction);
            AddStrategyCommand = new RelayCommand(AddStrategyAction);
            RemoveStrategyCommand = new RelayCommand(RemoveStrategyAction);
            CreateStockDetailCommand = new RelayCommand<TabControl>(CreateStockDetailAction);
        }

        public void CloseWindowAction()
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

            var data = _stockDataList.Single(_ => _.ID == SelectedStockInfo.ID);

            StockDetailViewModel newTabVM = new StockDetailViewModel(data);
            ucStockDetail ucStockDetail = new ucStockDetail() { DataContext = newTabVM };
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
                ResetData(_stockDataList);
                IFilterService service = new FilterService();
                foreach (var selectedFilter in SeletedAlgoStratrgy.FilterInfoList.Where(_ => _.IsSelected))
                {
                    service.AddFilter(FilterFactory.CreatFilterByFilterType(selectedFilter.Type, selectedFilter.Param, _stockDataList));
                }
                service.Execute();


                IStockInfoService stockInfoService = new StockInfoService();
                var stockInfoList = stockInfoService.FilterData(_stockDataList, _stockInfoDictionary, startTime, endTime);

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

        private void ResetData(IEnumerable<StockData> stockDataList)
        {
            foreach (var stockData in stockDataList)
            {
                for (int i = 0; i < stockData.IsFilter.Length; i++)
                {
                    stockData.IsFilter[i] = false;
                }
            }
        }
    }
}
