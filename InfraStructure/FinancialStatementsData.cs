using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{
    public struct FinancialStatementsData
    {
        public FinancialStatementsData(string stockID, DateTime date, double eps)
        {
            StockID = stockID;
            Date = date;
            Eps = eps;
        }
         
        public string StockID { get; set; }

        public DateTime Date { get; set; }

        public double Eps { get; set; }
    }

    public class FinancialStatementJson
    {
        public FinancialStatementJson() { }
        
        public string msg { get; set; }

        public string status { get; set; }

        public List<FinancialStatementItemJson> data { get; set; }
    }

    public class FinancialStatementItemJson
    {
        public FinancialStatementItemJson() { }
        public DateTime date { get; set; }
        public string stock_id { get; set; }
        public string type { get; set; }
        public double value { get; set; }
        public string origin_name { get; set; } 
    }
}
