using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StockPredictor.Class.SumResult
{
    public class SumResult : ObservableObject
    {
        private double totalPrice;
        public double TotalPrice
        {
            get => totalPrice;
            set { Set(() => TotalPrice, ref totalPrice, value); }
        }

        private double diffPrice;
        public double DiffPrice
        {
            get => diffPrice;
            set { Set(() => DiffPrice, ref diffPrice, value); }
        }

        private double growRatio;
        public double GrowRatio
        {
            get => growRatio;
            set { Set(() => GrowRatio, ref growRatio, value); }
        }

        private int winAmount;
        public int WinAmount
        {
            get => winAmount;
            set { Set(() => WinAmount, ref winAmount, value); }
        }

        private int loseAmount;
        public int LoseAmount
        {
            get => loseAmount;
            set { Set(() => LoseAmount, ref loseAmount, value); }
        }
    }
}
