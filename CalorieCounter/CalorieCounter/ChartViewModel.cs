using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    public class ChartViewModel
    {
        public ObservableCollection<ChartData> Data1 { get; set; }

        public ChartViewModel()
        {
            Data1 = new ObservableCollection<ChartData>();
        }
    }

    
}
