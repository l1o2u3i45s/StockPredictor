using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StockPredictor.ViewModel
{
    public class RegularQuotaProfitCaculateViewModel : ViewModelBase
    {
        private int targetMoney=2000;

        public int TargetMoney
        {
            get => targetMoney;
            set { Set(() => TargetMoney, ref targetMoney, value); }
        }

        private int targetYear=25;

        public int TargetYear
        {
            get => targetYear;
            set { Set(() => TargetYear, ref targetYear, value); }
        }

        private double yearlyRatio=6;

        public double YearlyRatio
        {
            get => yearlyRatio;
            set { Set(() => YearlyRatio, ref yearlyRatio, value); }
        }

        private string result;

        public string Result
        {
            get => result;
            set { Set(() => Result, ref result, value); }
        }
        public RelayCommand CaculationCommand { get; set; }

        public RegularQuotaProfitCaculateViewModel()
        {
            CaculationCommand = new RelayCommand(CaculationAction);
        }

        private void CaculationAction()
        {
            double ratioSum = 0;
            for (int i = 0; i < targetYear; i++)
            {
                ratioSum += Math.Pow( (1 + (yearlyRatio/100) ), i);
            }

            double yearlySaving = (TargetMoney * 10000) / ratioSum;


            Result = $"預計每年投入 {(int)yearlySaving}元 \r\n"; 
            Result += $"每月需投入 {(int)(yearlySaving /12)} \r\n";
            Result += $"總投入金額 {(int) (yearlySaving * TargetYear)} \r\n";
            Result += $"報酬率 { (int)(100 + ratioSum)}% \r\n";
        }
    }
}
