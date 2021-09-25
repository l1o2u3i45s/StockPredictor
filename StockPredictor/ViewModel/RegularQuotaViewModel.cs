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
                 
        }
    }
}
