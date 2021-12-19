using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{

    public struct InvestInstitutionBuySellData
    {
        public DateTime Date { get; set; }
        public string StockID { get; set; }
        public string Name { get; set; }  
        public double Buy { get; set; }  
        public double Sell { get; set; }  
        
    }
   public struct PERatioTableData
    {
        public DateTime Date { get; set; }
        public string StockID { get; set; }
        public double PERatio { get; set; } //本益比
        public double PBRatio { get; set; } //每股淨值比
        public double DividendYield { get; set; } //現金殖利率
    }

   public class PERatioTableJson
   {
       public string msg { get; set; }

       public string status { get; set; }

       public List<PERatioTableItemJson> data { get; set; }
    }
     
    public class PERatioTableItemJson
    {
       public PERatioTableItemJson() { }
       public DateTime date { get; set; }
       public string stock_id { get; set; }
       public double dividend_yield { get; set; }
       public double PER { get; set; }
       public double PBR { get; set; } 
   }

    public class InvestInstitutionBuySellTableJson
    {
        public string msg { get; set; }

        public string status { get; set; }

        public List<InvestInstitutionBuySellTableItemJson> data { get; set; }
    }

    public class InvestInstitutionBuySellTableItemJson
    {
        public InvestInstitutionBuySellTableItemJson() { }
        public DateTime date { get; set; }
        public string stock_id { get; set; }
        public double buy { get; set; }
        public double sell { get; set; }
        public string name { get; set; }
    }
}
