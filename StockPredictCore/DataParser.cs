using System;
using System.Collections.Generic;
using System.Data;
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

        public static void CrawData(DateTime startDate,List<string> stockCodeList)
        {
            string stockRawDataPath = "StockRawData";
            if(Directory.Exists(stockRawDataPath) == false)
                Directory.CreateDirectory(stockRawDataPath);

             
            foreach(var stockCode in stockCodeList)
            {
                string idpath = Path.Combine(stockRawDataPath, $"{stockCode}.txt");
                DateTime timeDateTime = startDate;
                StreamWriter sw = new StreamWriter(idpath);
                while (timeDateTime <= DateTime.Today)
                {
                    Thread.Sleep(200);
                    string url = $"https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={timeDateTime.ToString("yyyyMMdd")}&stockNo={stockCode}"; //20210601

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = WebRequestMethods.Http.Get;
                    request.ContentType = "application/json";
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var stream = response.GetResponseStream())
                            using (var reader = new StreamReader(stream))
                            {
                                dynamic dynamicData = JsonConvert.DeserializeObject(reader.ReadToEnd());

                                if (dynamicData.stat.ToString() == "很抱歉，沒有符合條件的資料!")
                                    continue;

                                string dataList = dynamicData.data.ToString();
                                string[] splitArray = dataList.Split(new string[] { "]," }, StringSplitOptions.RemoveEmptyEntries);

                                foreach (string data in splitArray)
                                {
                                    string temp = data.Replace("[", "");
                                    string[] infoList = temp.Split(',');

                                    string resultLine = "";
                                    for(int i =0;i < infoList.Length; i++)
                                    {
                                        string infoData = infoList[i].Replace(",", "");
                                        infoData = infoData.Trim();
                                        resultLine += infoData;

                                        if (i < infoList.Length - 1)
                                            resultLine += ",";
                                    }
                                    sw.WriteLine(resultLine);
                                } 
                            }
                        }
                    }
                }
                sw.Close(); 
            }
               
        }

        public static StockData ConvertData(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            StockData result = new StockData(lines.Length, Path.GetFileNameWithoutExtension(filepath)); 

            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');
                if (data.Count(_ => string.IsNullOrEmpty(_)) > 0)
                    continue;

                result.Date[i] = Convert.ToDateTime(data[0]);
                result.OpenPrice[i] = Convert.ToDouble(data[1]);
                result.HightestPrice[i] = Convert.ToDouble(data[2]);
                result.LowestPrice[i] = Convert.ToDouble(data[3]);
                result.ClosePrice[i] = Convert.ToDouble(data[4]);
                result.Volumn[i] = Convert.ToDouble(data[5]);
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
