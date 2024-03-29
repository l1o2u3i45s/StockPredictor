﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using InfraStructure;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using StockPredictCore.Filter;
using StockPredictCore.Filter.RSI;
using StockPredictCore.Service;
using StockPredictor.Class;
using StockPredictor.Class.FilterInfo;

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

        private string stockID = "0050";

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

        private string displayResult_StockNameInfo;

        public string DisplayResult_StockNameInfo
        {
            get => displayResult_StockNameInfo;
            set { Set(() => DisplayResult_StockNameInfo, ref displayResult_StockNameInfo, value); }
        }

        private int displayResult_TotalInvestMoney;

        public int DisplayResult_TotalInvestMoney
        {
            get => displayResult_TotalInvestMoney;
            set { Set(() => DisplayResult_TotalInvestMoney, ref displayResult_TotalInvestMoney, value); }
        }

        private int displayResult_CurrentEarningMoney;

        public int DisplayResult_CurrentEarningMoney
        {
            get => displayResult_CurrentEarningMoney;
            set { Set(() => DisplayResult_CurrentEarningMoney, ref displayResult_CurrentEarningMoney, value); }
        }

        private double displayResult_AveragePrice;

        public double DisplayResult_AveragePrice
        {
            get => displayResult_AveragePrice;
            set { Set(() => DisplayResult_AveragePrice, ref displayResult_AveragePrice, value); }
        }

        private double displayResult_LastestPrice;

        public double DisplayResult_LastestPrice
        {
            get => displayResult_LastestPrice;
            set { Set(() => DisplayResult_LastestPrice, ref displayResult_LastestPrice, value); }
        }

        private double displayResult_GrowRatio;

        public double DisplayResult_GrowRatio
        {
            get => displayResult_GrowRatio;
            set { Set(() => DisplayResult_GrowRatio, ref displayResult_GrowRatio, value); }
        }

        private double displayResult_YearlyGrowRatio;

        public double DisplayResult_YearlyGrowRatio
        {
            get => displayResult_YearlyGrowRatio;
            set { Set(() => DisplayResult_YearlyGrowRatio, ref displayResult_YearlyGrowRatio, value); }
        }

        private List<FilterInfo> filterTypeList;

        public List<FilterInfo> FilterTypeList
        {
            get => filterTypeList;
            set { Set(() => FilterTypeList, ref filterTypeList, value); }
        }

        private FilterInfo selectedFilterInfo;

        public FilterInfo SelectedFilterInfo
        {
            get => selectedFilterInfo;
            set { Set(() => SelectedFilterInfo, ref selectedFilterInfo, value); }
        }
        

        public RelayCommand CaculateCommand { get; set; } 

        public RegularQuotaViewModel(ConcurrentBag<StockData> stockdataList)
        { 
            CaculateCommand = new RelayCommand(CaculateAction);
            _stockDataList = stockdataList;

            filterTypeList = new List<FilterInfo>();
            foreach (FilterType type in Enum.GetValues(typeof(FilterType)))
            {
                filterTypeList.Add(FilterInfoFactory.CreatFilterInfoByFilterType(type)); 
            }

            SelectedFilterInfo = filterTypeList.FirstOrDefault();
        }
          
        private void CaculateAction()
        {
            if(_stockDataList == null)
                return;

            Parallel.ForEach(_stockDataList, stockData =>
            {
                for (int i = 0; i < stockData.Date.Length; i++)
                {
                    stockData.IsFilter[i] = false;
                }
            });

            var data = _stockDataList.SingleOrDefault(_ => _.ID == stockID);
            if (data == null)
            {
                MessageBox.Show("股票代碼錯誤!");
               return;
            }

            List<StockData> stockDataList = new List<StockData>();
            stockDataList.Add(data);

            FilterBase filter = FilterFactory.CreatFilterByFilterType(SelectedFilterInfo.Type, SelectedFilterInfo.Param, stockDataList);
            filter.Execute();

            var resultList = RegularQuotaService.Calulate(stockDataList.Single(), startDate,monthlyInvestValue);

            if (resultList.Count == 0)
            {
                MessageBox.Show("無資料符合條件!");
                return;
            }

            SeriesList.Clear(); 
            LineSeries currentStockPriceLineSeries = new LineSeries(){
                Values = new ChartValues<ObservableValue>() ,
                PointGeometrySize = 0,
                StrokeThickness = 3,
                Title = "現在股價"
            };
            LineSeries averageHistoryStockPriceLineSeries = new LineSeries() { 
                Values = new ChartValues<ObservableValue>(),
                PointGeometrySize = 0,
                StrokeThickness = 3,
                Title = "定期定額均價"
            }; 
            SeriesList.Add(currentStockPriceLineSeries);
            SeriesList.Add(averageHistoryStockPriceLineSeries);
             
            foreach (var result in resultList)
            { 
                currentStockPriceLineSeries.Values.Add(new ObservableValue(result.CurrentPrice));
                averageHistoryStockPriceLineSeries.Values.Add(new ObservableValue(result.InventoryAveragePrice));
            }
            double yearhDiff = ((double)(DateTime.Now.Year - StartDate.Year) * 12 + DateTime.Now.Month - StartDate.Month) / 12;
            DisplayResult_StockNameInfo = $"{stockDataList.Single().ID} {stockDataList.Single().Name}";
            DisplayResult_TotalInvestMoney = (int)resultList.Last().AccumulationMoney;
            DisplayResult_LastestPrice = stockDataList.Single().ClosePrice[stockDataList.Single().ClosePrice.Length - 1];
            DisplayResult_AveragePrice = resultList.Last().InventoryAveragePrice;
            DisplayResult_GrowRatio = Math.Round( (DisplayResult_LastestPrice / DisplayResult_AveragePrice) - 1, 2) * 100;
            DisplayResult_CurrentEarningMoney = (int)(DisplayResult_TotalInvestMoney * (100 + DisplayResult_GrowRatio )/100); 
            DisplayResult_YearlyGrowRatio = Math.Round(DisplayResult_GrowRatio / yearhDiff ,2);
        }
    }
}
