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
        private bool isDesign = true;
        const string fileFolder = @"E:\\StockData";
        private List<StockData> stockDataList = new List<StockData>();

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

        public RelayCommand AnalysisCommand { get; set; }

        public MainViewModel()
        {

            InitFilterList();
            AnalysisCommand = new RelayCommand(AnalysisAction);
            if (isDesign == false)
            {
                PreProcessData(Directory.GetFiles(fileFolder));
            }
                
        }

        private void AnalysisAction()
        {
            FilterService service = new FilterService();
            foreach (var selectedFilter in FilterInfoCollection.Where(_ => _.IsSelected))
            { 
                service.AddFilter(FilterFactory.CreatFilterByFilterType(selectedFilter.Type, stockDataList));
            }
            service.Execute();
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
    }

   
}