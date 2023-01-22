using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private const string _stockInfoPath = @"StockInfoList\\stockInfo.csv";
        private bool isDesign = false;
        private ConcurrentBag<StockData> stockDataList;
        private Dictionary<string, string> stockInfoDictionary = new Dictionary<string, string>();
        private ConcurrentBag<FinancialStatementsData> financialStatementsDataList;
        private ConcurrentBag<PERatioTableData> peRatioTableDataDataList;
        private ConcurrentBag<InvestInstitutionBuySellData> investInstitutionBuySellDataDataList;

        private readonly IPreProcessService _preProcessService = new PreProcessService();
        private ICSVParser _csvParser = new CSVParser();

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


        private StockFilterViewModel _stockFilterViewModel;

        public StockFilterViewModel StockFilterViewModel
        {
            get => _stockFilterViewModel;
            set { Set(() => StockFilterViewModel, ref _stockFilterViewModel, value); }
        }

        public RelayCommand CloseWindowCommand { get; set; }

        public MainViewModel()
        {
            DataCenterViewModel = new DataCenterViewModel();
            DataCenterViewModel.SetUpdateDataDoneCallback(UpdateStockData);
            InitStockInfo();
            UpdateStockData();

            RegularQuotaProfitCaculateViewModel = new RegularQuotaProfitCaculateViewModel();

            CloseWindowCommand = new RelayCommand(CloseWindowAction);
        }

        private void UpdateStockData()
        {
            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (sender, args) =>
            {
                if (isDesign == false)
                    PreProcessData(Directory.GetFiles(DataParser.StockRawDataPath));

                RegularQuotaViewModel = new RegularQuotaViewModel(stockDataList);
                StockFilterViewModel = new StockFilterViewModel(stockDataList, stockInfoDictionary);
            };

            worker.RunWorkerCompleted += (sender, args) =>
            {
                IsBusy = false;
            };

            IsBusy = true;
            BusyContent = "Update Stock Data";

            worker.RunWorkerAsync();
        }

        private void CloseWindowAction()
        {
            StockFilterViewModel.CloseWindowAction();
        }

        private async void PreProcessData(string[] stockFiles)
        {
            var FinancialStatementfileList = Directory.GetFiles(DataParser.FinancialStatementPath);
            var peRatioTableFileList = Directory.GetFiles(DataParser.PERatioTablePath);

            var getFinanceTask = Task.Factory.StartNew(() => _preProcessService.GetFinancialStatementsData(FinancialStatementfileList));
            var getPERTask = Task.Factory.StartNew(() => _preProcessService.GetPERatioTableData(peRatioTableFileList));
            var getStockTask = Task.Factory.StartNew(() => _preProcessService.GetStockData(stockFiles, stockInfoDictionary));

            await Task.WhenAll(getFinanceTask, getPERTask, getStockTask);
            financialStatementsDataList = getFinanceTask.Result;
            peRatioTableDataDataList = getPERTask.Result;
            stockDataList = getStockTask.Result;
        }

        private void InitStockInfo()
        {
            if (Directory.Exists(DataParser.StockRawDataPath) == false)
                Directory.CreateDirectory(DataParser.StockRawDataPath);

            if (Directory.Exists(DataParser.PERatioTablePath) == false)
                Directory.CreateDirectory(DataParser.PERatioTablePath);

            if (Directory.Exists(DataParser.FinancialStatementPath) == false)
                Directory.CreateDirectory(DataParser.FinancialStatementPath);

            stockInfoDictionary = _csvParser.GetStockInfo(_stockInfoPath);
        }
    }

    public enum eSelectTabType
    {
        StockFilter,
        DataCenter,
        StockDetail
    }

}