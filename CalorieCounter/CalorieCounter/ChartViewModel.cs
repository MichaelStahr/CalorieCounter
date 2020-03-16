using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    public class ChartViewModel
    {
        public ObservableCollection<ChartData> Data1 { get; set; }
        public List<ChartData> Data { get; set; }

        public ChartViewModel()
        {
            Data1 = new ObservableCollection<ChartData>
            {
                new ChartData("2020-3-16", 100),
                new ChartData("2020-3-17", 200),
                new ChartData("2020-3-18", 50)

            };
        }
    }

    
}
