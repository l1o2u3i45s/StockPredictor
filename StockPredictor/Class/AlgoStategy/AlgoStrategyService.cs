using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StockPredictor.Class.AlgoStategy
{
    public class AlgoStrategyService
    {
        public void Save(List<AlgoStrategy> algoStrategy)
        { 
            var json = new JavaScriptSerializer().Serialize(algoStrategy);
        }

        public List<AlgoStrategy> Load(string file)
        {
            var data = new JavaScriptSerializer().DeserializeObject(file);

            return data as List<AlgoStrategy>;
        }
    }
}
