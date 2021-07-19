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

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public AlgoStrategy()
        {
            foreach (FilterType type in Enum.GetValues(typeof(FilterType)))
            {
                FilterInfoList.Add(FilterInfoFactory.CreatFilterInfoByFilterType(type));
            }
        }
    }
}
