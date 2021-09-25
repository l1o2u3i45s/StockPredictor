using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StockPredictor.ViewModel
{
    public class RegularQuotaViewModel : ViewModelBase
    {
        private DateTime startDate;

        public DateTime StartDate
        {
            get => startDate;
            set { Set(() => StartDate, ref startDate, value); }
        }

        private int monthlyInvestValue;

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

        public RegularQuotaViewModel()
        {

            CaculateCommand = new RelayCommand(CaculateAction);
        }

        private void CaculateAction()
        {

        }
    }
}
