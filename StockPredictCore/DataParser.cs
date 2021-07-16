using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using InfraStructure;

namespace StockPredictCore
{
    public class DataParser
    {

        public static void CrawData()
        {

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
    }
}
