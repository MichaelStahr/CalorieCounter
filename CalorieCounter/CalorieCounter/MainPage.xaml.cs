using Android;
using Android.OS;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            NavigationPage.SetBackButtonTitle(this, "Home");
            StackLayout header = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Spacing = 3,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Spacing = 0,
                        Children =
                        {
                            new Label {Text = "Miami University",
                            FontSize = 25,
                            FontFamily = Device.RuntimePlatform == Device.Android ? "Acme-Regular.ttf#Acme-Regular" : null,
                            HorizontalOptions = LayoutOptions.EndAndExpand},

                            new Label {Text = "Oxford",
                            FontSize = 20,
                            FontFamily = Device.RuntimePlatform == Device.Android ? "Acme-Regular.ttf#Acme-Regular" : null,
                            HorizontalOptions = LayoutOptions.EndAndExpand},
                        }
                    },
                    new Image {Source = "miami.jpg", HeightRequest = 50, HorizontalOptions = LayoutOptions.End},
                }
            };
            NavigationPage.SetTitleView(this, header);
            Color labelColor = Color.FromHex("503047");
            //Preferences.Clear();
            
            DateTime datetime = Calendar.SelectedDate.Value;
            string date = datetime.ToShortDateString();
            //Notes.Text = Preferences.Get(date, "No notes yet!");

        }

        private void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs e)
        {
            string date = e.DateTime.Date.ToShortDateString();
            DateLabel.Text = e.DateTime.Date.ToShortDateString();

            //if (Preferences.ContainsKey(date))
            //{
            //    Notes.Text = Preferences.Get(date, "No notes yet");
            //} else
            //{
            //    Preferences.Set(date, "");
            //}
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            
            Button button = (Button)sender;
            
            DateTime currentDate = Calendar.SelectedDate.Value;
            DateTime newDate;
            if (button.Equals(GoBack))
            {
                newDate = currentDate.AddDays(-1);
            }
            else
            {
                newDate = currentDate.AddDays(1);
            }
            Calendar.SelectedDate = newDate;
            DateLabel.Text = newDate.Date.ToShortDateString();
        }


        async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExtraDetailsPage(), true);
        }

        private void Notes_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime datetime = Calendar.SelectedDate.Value;
            string date = datetime.ToShortDateString();

            //Preferences.Set(date, Notes.Text);

        }
        private void ClickToShowPopup_Clicked(object sender, EventArgs e)
        {
            
            popup.Show();
        }
    }
}
