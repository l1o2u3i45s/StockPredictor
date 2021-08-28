using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StockPredictCore;

namespace StockPredictor.ViewModel
{
    public class DataCenterViewModel:ViewModelBase
    {

        public delegate void UpdateDataDoneCallback();
        public RelayCommand UpdateHistorialStockDataCommand { get; set; }

        private UpdateDataDoneCallback updateDataDoneCallback;

        public void SetUpdateDataDoneCallback(UpdateDataDoneCallback cb)
        {
            updateDataDoneCallback = cb;
        }

        public DataCenterViewModel()
        {
            UpdateHistorialStockDataCommand = new RelayCommand(UpdateHistorialStockDataAction);

        }

        private void UpdateHistorialStockDataAction()
        {
            List<string> stockCodeList = new List<string>();
            using (var reader = new StreamReader(@"StockInfofile\\0050.txt"))
            {
                while (!reader.EndOfStream)
                {
                    stockCodeList.Add(reader.ReadLine());
                }
            }
             
            DataParser.GetStockPriceData(new DateTime(2000,1,1), stockCodeList);

            if(updateDataDoneCallback != null)
                updateDataDoneCallback.Invoke();
             
            MessageBox.Show("更新股價完成");
             
        }
    }
}
