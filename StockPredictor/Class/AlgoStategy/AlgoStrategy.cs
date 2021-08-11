using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using StockPredictCore.Filter;
using StockPredictor.Class.FilterInfo;

namespace StockPredictor.Class
{
    public class AlgoStrategy : ObservableObject
    {
        private string name = "演算法策略";
        public string Name
        {
            get => name;
            set { Set(() => Name, ref name, value); }
        }

        private ObservableCollection<FilterInfo.FilterInfo> filterInfoList = new ObservableCollection<FilterInfo.FilterInfo>();

        public ObservableCollection<FilterInfo.FilterInfo> FilterInfoList
        {
            get => filterInfoList;
            set { Set(() => FilterInfoList, ref filterInfoList, value); }
        }
         

        public List<FilterInfo.FilterInfo> AverageLineFilterList //均線
        {
            get => FilterInfoList.Where(_ => _.TypeName.Contains("Ma")).ToList(); 
        }

        public List<FilterInfo.FilterInfo> TargerFilterList //指標
        {
            get => FilterInfoList.Where(_ => _.TypeName.Contains("RSI")).ToList();
        }

        public List<FilterInfo.FilterInfo> ValueFilterList //價格
        {
            get => FilterInfoList.Where(_ => _.TypeName.Contains("股價")).ToList();
        }

        public List<FilterInfo.FilterInfo> VolumnFilterList //成交量
        {
            get => FilterInfoList.Where(_ => _.TypeName.Contains("成交量")).ToList();
        }

        public List<FilterInfo.FilterInfo> OtherFilterList //特殊算法
        {
            get => FilterInfoList.Where(_ => _.TypeName.Contains("特殊算法")).ToList();
        }
        

        public AlgoStrategy()
        {
           
        }
        public AlgoStrategy(bool init)
        {
            if (init)
            {
                foreach (FilterType type in Enum.GetValues(typeof(FilterType)))
                {
                    FilterInfoList.Add(FilterInfoFactory.CreatFilterInfoByFilterType(type));
                }
            }
         
        }
       
    }
}
