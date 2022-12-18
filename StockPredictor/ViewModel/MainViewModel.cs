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
        private Dictionary<string, string> stockInfoDictionary = new Dictionary<string, string>();
        private ConcurrentBag<FinancialStatementsData> financialStatementsDataList;
        private ConcurrentBag<PERatioTableData> peRatioTableDataDataList;
        private ConcurrentBag<InvestInstitutionBuySellData> investInstitutionBuySellDataDataList;
        
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

        private void PreProcessData(string[] stockFiles)
        {
            GetFinancialStatementsData(Directory.GetFiles(DataParser.FinancialStatementPath));
            GetPERatioTableData(Directory.GetFiles(DataParser.PERatioTablePath));
            //GetInvestInstitutionBuySellDataData(Directory.GetFiles(DataParser.TaiwanStockInstitutionalInvestorsBuySellPath));

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
                  
                //data.UpdateInstitutionBuySellData(investInstitutionBuySellDataDataList.Where(x => x.StockID == data.ID).ToList());
                 
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