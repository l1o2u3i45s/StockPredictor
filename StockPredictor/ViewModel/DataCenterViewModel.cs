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
        public RelayCommand GetFinancialStatementCommand { get; set; }
        public RelayCommand GetPERDataCommand { get; set; }
         

        private UpdateDataDoneCallback updateDataDoneCallback;

        public void SetUpdateDataDoneCallback(UpdateDataDoneCallback cb)
        {
            updateDataDoneCallback = cb;
        }

        public DataCenterViewModel()
        {
            UpdateHistorialStockDataCommand = new RelayCommand(UpdateHistorialStockDataAction);
            GetFinancialStatementCommand = new RelayCommand(GetFinancialStatementAction);
            GetPERDataCommand = new RelayCommand(GetPERDataAction);
        }

        private void GetPERDataAction()
        {
            DataParser.CrawlStockPERData(new DateTime(2000, 1, 1), GetStockIdList());

            MessageBox.Show("更新P/E ratio表完成");
        }
       
        private void GetFinancialStatementAction()
        {
            DataParser.CrawlFinancialStatementsData(new DateTime(2000, 1, 1), GetStockIdList());
             
            MessageBox.Show("更新綜合損益表完成");
        }

        private void UpdateHistorialStockDataAction()
        { 
            DataParser.CrawlStockPriceData(new DateTime(2000,1,1), GetStockIdList());

            if(updateDataDoneCallback != null)
                updateDataDoneCallback.Invoke();
             
            MessageBox.Show("更新股價完成");
             
        }

        private List<string> GetStockIdList()
        {
            List<string> stockCodeList = new List<string>();
            using (var reader = new StreamReader(@"StockInfofile\\0050.txt"))
            {
                while (!reader.EndOfStream)
                {
                    stockCodeList.Add(reader.ReadLine());
                }
            }

            return stockCodeList;
        }
    }
}
