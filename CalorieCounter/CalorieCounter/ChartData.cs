using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// A data point on the bar chart on the Main page
    /// </summary>
    public class ChartData
    {
        public string Date { get; set; } 

        public double Calories { get; set; }
        public ChartData(string date, double calories)
        {
            this.Date = date;
            this.Calories = calories;
        }

        

        

    }
}
