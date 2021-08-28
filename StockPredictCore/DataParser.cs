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

        public readonly static string StockRawDataPath = "StockRawData";

        //取得股價資訊
        public static void GetStockPriceData(DateTime startDate, List<string> stockCodeList)
        { 
            if (Directory.Exists(StockRawDataPath) == false)
                Directory.CreateDirectory(StockRawDataPath);

            DateTime startDateTime = new DateTime(startDate.Year, startDate.Month, 1);
            DownloadFile(DatasetType.TaiwanStockPrice, stockCodeList, StockRawDataPath, startDateTime, DateTime.Today);
        }

        public static StockData ConvertData(string filepath)
        {
            string alltext = System.IO.File.ReadAllText(filepath);
            StockQueryJson dataList = JsonConvert.DeserializeObject<StockQueryJson>(alltext);

            StockData result = new StockData(dataList.data.Count, Path.GetFileNameWithoutExtension(filepath));

            for (int i = 1; i < dataList.data.Count; i++)
            {
                StockDataJson currrentData = dataList.data[i];
                result.Date[i] = currrentData.date;
                result.OpenPrice[i] = currrentData.open;
                result.HightestPrice[i] = currrentData.max;
                result.LowestPrice[i] = currrentData.min;
                result.ClosePrice[i] = currrentData.close;
                result.Volumn[i] = currrentData.Trading_Volume;
                result.KValue[i] = 0;
                result.DValue[i] = 0;

                result.MA60[i] = 0;
                result.MA20[i] = 0;
                result.MA5[i] = 0;
            }

            return result;
        }


        private static void DownloadFile(DatasetType dataset,List<string> stockCodeList,string downloadFolder,DateTime startDate, DateTime endDate)
        {
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

        class StockQueryJson
        {
            public StockQueryJson() { }
            public string msg { get; set; }
            public string status { get; set; }
            public List<StockDataJson> data { get; set; }
        }

        class StockDataJson
        {
            public StockDataJson() { }
            public DateTime date { get; set; }
            public string stock_id { get; set; }
            public double Trading_Volume { get; set; }
            public double Trading_money { get; set; }
            public double open { get; set; }
            public double max { get; set; }
            public double min { get; set; }
            public double close { get; set; }
            public double spread { get; set; }
            public double Trading_turnover { get; set; }
        }

        enum DatasetType
        {
            [Description("TaiwanStockPrice")]
            TaiwanStockPrice
        }
    }
}
