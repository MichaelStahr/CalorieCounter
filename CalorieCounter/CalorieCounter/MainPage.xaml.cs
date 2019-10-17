using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalorieCounter
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            Button button = (Button)sender;
            if (button.Equals(addFood))
            {
                foodLabel.Text += foodEntry.Text + "\n";
            } else
            {
                exerciseLabel.Text += exerciseLabel.Text + "\n";
            }
        }

        private void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs e)
        {            
            DateLabel.Text = e.DateTime.Date.ToShortDateString();
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            DateTime currentDate = Calendar.SelectedDate.Value;
            DateTime newDate;

            if (button.Equals(GoBack))
            {
                newDate = currentDate.AddDays(-1);
            } else
            {
                newDate = currentDate.AddDays(1);
            }
            Calendar.SelectedDate = newDate;
            DateLabel.Text = newDate.Date.ToShortDateString();
        }
    }
}
