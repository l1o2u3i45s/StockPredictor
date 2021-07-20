using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace StockPredictor.Class.AlgoStategy
{
    public static class AlgoStrategyService
    {
        private static string filePath = "AlgoStrategy";

        public static void Save(this AlgoStrategy algoStrategy)
        {
            if (Directory.Exists(filePath) == false)
                Directory.CreateDirectory(filePath);

            string json = JsonConvert.SerializeObject(algoStrategy);

            File.WriteAllText($"AlgoStrategy\\{algoStrategy.Name}_{DateTime.Now.ToString("yyyyMMddmmss")}.txt", json);
        }

        public static List<AlgoStrategy> Load( )
        {
            List<AlgoStrategy> result = new List<AlgoStrategy>();

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
