using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// A collection of ChartData points
    /// </summary>
    public class ChartViewModel
    {
        public ObservableCollection<ChartData> Data1 { get; set; }

        public ChartViewModel()
        {
            Data1 = new ObservableCollection<ChartData>();
        }
    }

    
}
