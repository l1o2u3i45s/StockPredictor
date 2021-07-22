using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
        public static readonly string apiToken = "60f5a54edc6276.88263769";
        public static readonly string rawDataFolderPath = "StockRawData";
        public static void CrawData(DateTime startDate, List<string> stockCodeList)
        {
             
            if (Directory.Exists(rawDataFolderPath) == false)
                Directory.CreateDirectory(rawDataFolderPath);

            foreach (var stockCode in stockCodeList)
            {
                string idpath = Path.Combine(rawDataFolderPath, $"{stockCode}.txt");
                DateTime timeDateTime = new DateTime(startDate.Year, startDate.Month, 1);
                string sTime = timeDateTime.ToString("yyyy-MM-dd");
                string eTime = DateTime.Today.ToString("yyyy-MM-dd");
                string url = $@"https://eodhistoricaldata.com/api/eod/{stockCode}.TW?from={sTime}&to={eTime}&period=d&api_token={apiToken}";

                Task.Run(() =>
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(url, idpath);
                    }

                }); 
            }

           
             
        }

        public static StockData ConvertData(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            StockData result = new StockData(lines.Length-1, Path.GetFileNameWithoutExtension(filepath));
            
                for (int i = 1; i < lines.Length-1; i++)
                {
                    string[] data = lines[i].Split(',');
                    if (data.Count(_ => string.IsNullOrEmpty(_)) > 0)
                        continue;

                    DateTime time;
                    if(DateTime.TryParse(data[0], out time) == false)
                        continue;

                    result.Date[i] = time;
                    result.OpenPrice[i] = Convert.ToDouble(data[1]);
                    result.HightestPrice[i] = Convert.ToDouble(data[2]);
                    result.LowestPrice[i] = Convert.ToDouble(data[3]);
                    result.ClosePrice[i] = Convert.ToDouble(data[4]);
                    result.Volumn[i] = Convert.ToDouble(data[6]);
                    result.KValue[i] = 0;
                    result.DValue[i] = 0;

                    result.MA60[i] = 0;
                    result.MA20[i] = 0;
                    result.MA5[i] = 0;
                }
           
            return result;
        }

        public static Dictionary<string, double> GetFinancialReport(string folder)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();


            var fileList = Directory.GetFiles(folder);

            foreach (var filepath in fileList)
            {
                FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read);

                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                //...
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                DataSet dataSet = excelReader.AsDataSet();
                //...
                //4. DataSet - Create column names from first row
                // excelReader.IsFirstRowAsColumnNames = true;
                //DataSet result = excelReader.AsDataSet();

                //5. Data Reader methods
                while (excelReader.Read())
                {
                    //excelReader.GetInt32(0);
                }

                //6. Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();
            }



            return result;
        }

        class stockJsonInfo
        {
            public stockJsonInfo() { }
            public DateTime TradeDate { get; set; }
            public double Volumn { get; set; }
            public double VolumnMoney { get; set; }
            public double OpenPrice { get; set; }
            public double HightestPrice { get; set; }
            public double LowestPrice { get; set; }
            public double ClosedPrice { get; set; }
            public double PriceDiff { get; set; }
            public double TradeCount { get; set; }
            // "日期","成交股數","成交金額","開盤價","最高價","最低價","收盤價","漲跌價差","成交筆數"
        }

      
    }
}
