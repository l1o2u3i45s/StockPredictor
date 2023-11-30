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
        private List<string> stockIDfileList;

        public List<string> StockIDfileList
        {
            get => stockIDfileList;
            set { Set(() => StockIDfileList, ref stockIDfileList, value); }
        }

        private string selectedStockIDFile;

        public string SelectedStockIDFile
        {
            get => selectedStockIDFile;
            set { Set(() => SelectedStockIDFile, ref selectedStockIDFile, value); }
        }

        public delegate void UpdateDataDoneCallback();

        public RelayCommand GetInterationStockDataCommand { get; set; }

        public RelayCommand UpdateHistorialStockDataCommand { get; set; }
        public RelayCommand GetFinancialStatementCommand { get; set; }
        public RelayCommand GetPERDataCommand { get; set; } 
        public RelayCommand GetInstitutionalInvestCommand { get; set; }

        private UpdateDataDoneCallback updateDataDoneCallback;

        public void SetUpdateDataDoneCallback(UpdateDataDoneCallback cb)
        {
            updateDataDoneCallback = cb;
        }

        public DataCenterViewModel()
        {
            GetInterationStockDataCommand = new RelayCommand(GetInterationStockDataAction);
            UpdateHistorialStockDataCommand = new RelayCommand(UpdateHistorialStockDataAction);
            GetFinancialStatementCommand = new RelayCommand(GetFinancialStatementAction);
            GetPERDataCommand = new RelayCommand(GetPERDataAction);
            GetInstitutionalInvestCommand = new RelayCommand(GetInstitutionalInvestAction);
            
            StockIDfileList = new List<string>();
            StockIDfileList.AddRange(Directory.GetFiles("StockInfofile"));
            SelectedStockIDFile = StockIDfileList[0];
        }

        private async void GetInterationStockDataAction()
        {
            await DataParser.GetIntegrationStockData( GetStockIdList());

            
            MessageBox.Show("匯出完成");
        }

        private void GetInstitutionalInvestAction()
        {
            DataParser.CrawStockInstitutionalInvest(new DateTime(2000, 1, 1), GetStockIdList());

            MessageBox.Show("更新法人買賣資料完成");
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

        private async void UpdateHistorialStockDataAction()
        { 
            await DataParser.CrawlStockPriceData(new DateTime(2000,1,1), GetStockIdList());

            if(updateDataDoneCallback != null)
                updateDataDoneCallback.Invoke();
             
            MessageBox.Show("更新股價完成");
             
        }

        private List<string> GetStockIdList()
        {
            List<string> stockCodeList = new List<string>();
            using (var reader = new StreamReader(selectedStockIDFile))
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
