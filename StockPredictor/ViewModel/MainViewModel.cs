using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using InfraStructure;
using StockPredictCore;

namespace StockPredictor.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        const string fileFolder = @"E:\\StockData";
        private List<StockData> stockDataList = new List<StockData>();
        public MainViewModel()
        { 
            PreProcessData(Directory.GetFiles(fileFolder));
             
        }

        private void PreProcessData(string[] stockFiles)
        {
            Parallel.ForEach(stockFiles, _ =>
            {
                StockData data = DataParser.GrabData(_);
                PreProcessor preProcessor = new PreProcessor();
                preProcessor.Execute(data);
                stockDataList.Add(data);
            });
        }
    }
}