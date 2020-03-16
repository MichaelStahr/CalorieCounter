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


            Calories.Add(new ChartDataPoint(today.AddDays(-2), 200));
            Calories.Add(new ChartDataPoint(today.AddDays(-1), 300));
            Calories.Add(new ChartDataPoint(today, 900));
            Calories.Add(new ChartDataPoint(today.AddDays(1), 145));
            Calories.Add(new ChartDataPoint(today.AddDays(2), 78));
        }

        public void AddDataPoint(DateTime date, double amount)
        {
            Calories.Add(new ChartDataPoint(date, amount));
        }

        public ObservableCollection<ChartDataPoint> getCalories()
        {
            return Calories;
        }

        

    }
}
