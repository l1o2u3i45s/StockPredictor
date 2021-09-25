using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InfraStructure;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using StockPredictCore.Service;

namespace StockPredictor.ViewModel
{
    public class RegularQuotaViewModel : ViewModelBase
    {
        private ConcurrentBag<StockData> _stockDataList;

        private DateTime startDate = new DateTime(2015,1,1);

        public DateTime StartDate
        {
            get => startDate;
            set { Set(() => StartDate, ref startDate, value); }
        }

        private int monthlyInvestValue = 10000;

        public int MonthlyInvestValue
        {
            get => monthlyInvestValue;
            set { Set(() => MonthlyInvestValue, ref monthlyInvestValue, value); }
        }

        private string stockID;

        public string StockID
        {
            get => stockID;
            set { Set(() => StockID, ref stockID, value); }
        }

        private SeriesCollection seriesList = new SeriesCollection();

        public SeriesCollection SeriesList
        {
            get => seriesList;
            set { Set(() => SeriesList, ref seriesList, value); }
        }
     

        public RelayCommand CaculateCommand { get; set; }

        public RegularQuotaViewModel(ConcurrentBag<StockData> stockdataList)
        { 
            CaculateCommand = new RelayCommand(CaculateAction);
            _stockDataList = stockdataList;
        }

        private void CaculateAction()
        {
            var data = _stockDataList.SingleOrDefault(_ => _.ID == stockID);
            if (data == null)
            {
                MessageBox.Show("股票代碼錯誤!");
               return;
            }

            var resultList = RegularQuotaService.Calulate(data,startDate,monthlyInvestValue);
            SeriesList.Clear();
            LineSeries currentStockPriceLineSeries = new LineSeries(){Values = new ChartValues<ObservableValue>() };
            LineSeries averageHistoryStockPriceLineSeries = new LineSeries() { Values = new ChartValues<ObservableValue>(),  };
            SeriesList.Add(currentStockPriceLineSeries);
            SeriesList.Add(averageHistoryStockPriceLineSeries);

            foreach (var result in resultList)
            { 
                currentStockPriceLineSeries.Values.Add(new ObservableValue(result.CurrentPrice));
                averageHistoryStockPriceLineSeries.Values.Add(new ObservableValue(result.InventoryAveragePrice));
            }
        }
    }
}
