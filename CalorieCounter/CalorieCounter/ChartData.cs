using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{

    public class ChartData
    {
        public ObservableCollection<ChartDataPoint> Calories { get; set; }

        public ChartData()
        {
            Calories = new ObservableCollection<ChartDataPoint>();
            DateTime today = DateTime.Today;
            
            Calories.Add(new ChartDataPoint(today, 100));
            Calories.Add(new ChartDataPoint(today.AddDays(1), 200));
            Calories.Add(new ChartDataPoint(today.AddDays(2), 300));
            Calories.Add(new ChartDataPoint(today.AddDays(3), 145));
            Calories.Add(new ChartDataPoint(today.AddDays(4), 78));
        }

    }
}
