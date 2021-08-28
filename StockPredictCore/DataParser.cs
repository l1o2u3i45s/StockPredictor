using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;
using InfraStructure;
using Newtonsoft.Json;

namespace StockPredictCore
{
    public class DataParser
    {
        public static readonly string apiToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJkYXRlIjoiMjAyMS0wOC0yOCAxMDoxNDo1MSIsInVzZXJfaWQiOiJsMW8ydTNpNDVzIiwiaXAiOiIxMTEuMjUwLjMyLjE2MyJ9.yme23fRjTCE-kpHACFI4dTuPx5-ywgmAiOjaQZ6thvM";

        public static readonly string apiUrl = "https://api.finmindtrade.com/api/v4/data?";

        public readonly static string StockRawDataPath = "StockRawData";//每日股價
        public readonly static string FinancialStatementPath = "FinancialStatement"; //綜合損益表

        //取得股價資訊
        public static void CrawlStockPriceData(DateTime startDate, List<string> stockCodeList)
        {   
            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, 1);
            DownloadFile(DatasetType.TaiwanStockPrice, stockCodeList, StockRawDataPath, startDateTime, DateTime.Today);
        }

        //取得綜合損益表
        public static void CrawlFinancialStatementsData(DateTime startDate, List<string> stockCodeList)
        { 
            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, 1);
            DownloadFile(DatasetType.TaiwanStockFinancialStatements, stockCodeList, FinancialStatementPath, startDateTime, DateTime.Today);
        }

        public static StockData GetStockData(string filepath)
        {
            string alltext = System.IO.File.ReadAllText(filepath);
            StockQueryJson dataList = JsonConvert.DeserializeObject<StockQueryJson>(alltext);

            StockData result = new StockData(dataList.data.Count, Path.GetFileNameWithoutExtension(filepath));

            for (int i = 1; i < dataList.data.Count; i++)
            { 
                result.UpdateData(dataList.data[i],i); 
            }

            return result;
        }
        public static List<FinancialStatementsData> GetFinancialStatementsData(string filepath)
        {
            string alltext = System.IO.File.ReadAllText(filepath);
            FinancialStatementJson dataList = JsonConvert.DeserializeObject<FinancialStatementJson>(alltext);
            List<FinancialStatementsData> result = new List<FinancialStatementsData>();

            foreach (var item in dataList.data.Where(_ => _.type == "EPS"))
            {
                result.Add(new FinancialStatementsData(item.stock_id, item.date, item.value));
            }
             
            return result;
        }
        

        private static void DownloadFile(DatasetType dataset,List<string> stockCodeList,string downloadFolder,DateTime startDate, DateTime endDate)
        {
            if (Directory.Exists(downloadFolder) == false)
                Directory.CreateDirectory(downloadFolder);

            List<Task> taskList = new List<Task>();
            foreach (var stockCode in stockCodeList)
            { 
                string idpath = Path.Combine(downloadFolder, $"{stockCode}.txt");
                
                string queryString = GetQueryString(dataset.GetDescriptionText(), stockCode, startDate, endDate);
                 
                taskList.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(queryString, idpath);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }));

            }

            Task.WaitAll(taskList.ToArray());
        }

        private static string GetQueryString(string dataSet, string stockID, DateTime startDate, DateTime endDate)
        {
            return $"{apiUrl}dataset={dataSet}&data_id={stockID}&start_date={startDate.ToString("yyyy-MM-dd")}&end_date={endDate.ToString("yyyy-MM-dd")}&token={apiToken}";
        }
         
        enum DatasetType
        {
            [Description("TaiwanStockPrice")]
            TaiwanStockPrice, //每日股價
            [Description("TaiwanStockFinancialStatements")]
            TaiwanStockFinancialStatements //綜合損益表
        }
    }
}
