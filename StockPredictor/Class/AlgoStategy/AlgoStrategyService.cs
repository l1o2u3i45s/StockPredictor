using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace StockPredictor.Class.AlgoStategy
{
    public static class AlgoStrategyService
    {
        public static string filePath = "AlgoStrategy";

        public static void Save(this AlgoStrategy algoStrategy)
        { 
            string json = JsonConvert.SerializeObject(algoStrategy);
            string filepath = $"AlgoStrategy\\{algoStrategy.Name}_{DateTime.Now.ToString("yyyyMMddmmssfffffff")}.txt";
            File.WriteAllText(filepath, json); 
        }

        public static List<AlgoStrategy> Load( )
        { 
            List<AlgoStrategy> result = new List<AlgoStrategy>();

            if(Directory.Exists(filePath) == false)
                return result;

            string[] files = Directory.GetFiles(filePath);

            foreach (var file in files)
            {

                var json =System.IO.File.ReadAllText(file);

                var data = JsonConvert.DeserializeObject<AlgoStrategy>(json);
                result.Add(data);
            }
           
            return result;
        }
    }
}
