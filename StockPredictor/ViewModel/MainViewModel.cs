using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Compilation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using StockPredictCore;
using StockPredictCore.Service;
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
        private List<InvestInstitutionBuySellData> investInstitutionBuySellDataDataList;

        private readonly IPreProcessService _preProcessService = new PreProcessService(new CalculateService());
        private ICSVParser _csvParser = new CSVParser();
        private Stopwatch _stopwatch = new Stopwatch();

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




            CloseWindowCommand = new RelayCommand(CloseWindowAction);
        }

        private void UpdateStockData()
        {
            Task.Factory.StartNew(async () =>
            {
                _stopwatch.Restart();
                if (isDesign == false)
                    await PreProcessData(Directory.GetFiles(DataParser.StockRawDataPath), Directory.GetFiles(DataParser.TaiwanStockInstitutionalInvestorsBuySellPath));

                RegularQuotaViewModel = new RegularQuotaViewModel(stockDataList);
                StockFilterViewModel = new StockFilterViewModel(stockDataList, stockInfoDictionary);
                RegularQuotaProfitCaculateViewModel = new RegularQuotaProfitCaculateViewModel();
                IsBusy = false;
                ConsoleWriteStopWatch(_stopwatch, "UpdateStockData");
            });

            IsBusy = true;
            BusyContent = "Update Stock Data";
        }

        private void CloseWindowAction()
        {
            StockFilterViewModel.CloseWindowAction();
        }

        private async Task PreProcessData(string[] stockFiles,string[] buysellFiles)
        {
            var FinancialStatementfileList = Directory.GetFiles(DataParser.FinancialStatementPath);
            var peRatioTableFileList = Directory.GetFiles(DataParser.PERatioTablePath);

            var getFinanceTask = Task.Factory.StartNew(() => _preProcessService.GetFinancialStatementsData(FinancialStatementfileList));
            var getPERTask = Task.Factory.StartNew(() => _preProcessService.GetPERatioTableData(peRatioTableFileList));
            var getStockTask = Task.Factory.StartNew(() => _preProcessService.GetStockData(stockFiles, stockInfoDictionary));
            var getBuySellTask = Task.Factory.StartNew(() => _preProcessService.GetInvestInstitutionBuySellDataData(buysellFiles));
            await Task.WhenAll(getFinanceTask, getPERTask, getStockTask, getBuySellTask);
            financialStatementsDataList = getFinanceTask.Result;
            peRatioTableDataDataList = getPERTask.Result;
            stockDataList = getStockTask.Result;
            investInstitutionBuySellDataDataList = getBuySellTask.Result;

            foreach (var stockData in stockDataList)
            {
                var buysellData = investInstitutionBuySellDataDataList.Where(_ => _.StockID == stockData.ID).ToList();
                stockData.UpdateInstitutionBuySellData(buysellData);
            }

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

        private void ConsoleWriteStopWatch(Stopwatch stopwatch, string functionName)
        {
            Console.WriteLine($"{functionName}: {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    public enum eSelectTabType
    {
        StockFilter,
        DataCenter,
        StockDetail
    }

}