using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{

    public class ChartData
    {
        //public ObservableCollection<ChartDataPoint> Calories { get; set; }
        public string Date { get; set; } 

        public double Calories { get; set; }
        public ChartData(string date, double calories)
        {
            this.Date = date;
            this.Calories = calories;
        }

        

        

    }
}
