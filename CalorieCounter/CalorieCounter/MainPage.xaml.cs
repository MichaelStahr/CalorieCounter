using Newtonsoft.Json.Linq;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace CalorieCounter
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {

        const string token = "dasgfdszfe";
        // date is initialized to calendar date of today on start
        public string dateString;
        const string uniqueId = "birdaj";
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;
        ChartViewModel model;


        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();
            model = new ChartViewModel();
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
            
            DateTime date = Calendar.SelectedDate.Value;
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            dateString = year + "-" + month + "-" + day;
            //Notes.Text = Preferences.Get(date, "No notes yet!");

        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (_restService != null && this.CurrentPage is ContentPage)
            {
                FoodLookup(dateString);
            }
        }

        protected override void OnAppearing()
        {
            FoodLookup(dateString);
        } 

        public string DisplayDailyValuesByUserDay(string date)
        {
            // /api.asmx/DisplayDailyValuesByUserDay
            //date = "2020-3-05";
            string requestUri = apiEndpoint;
            requestUri += "DisplayDailyValuesByUserDay";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&date={date}";
            requestUri += $"&token={token}";

            return requestUri;
        }
        
        async void FoodLookup(string date)
        {
            List<DailyValues> dailyValues = null;

            
            dailyValues = await _restService.DisplayDailyValuesByUserDayAsync(DisplayDailyValuesByUserDay(date));
            if (dailyValues != null && dailyValues.Count != 0)
            {
                totalCal.Text = dailyValues[0].TotalCalories.ToString() + "k";
                fat.Text = dailyValues[0].TotalFat.ToString() + "g";
                cholesterol.Text = dailyValues[0].TotalCholesterol.ToString() + "mg";
                sodium.Text = dailyValues[0].TotalSodium.ToString() + "mg";
                carbs.Text = dailyValues[0].TotalCarbs.ToString() + "g";
                calcium.Text = dailyValues[0].TotalCalcium.ToString() + "mg";
                sugar.Text = dailyValues[0].TotalSugars.ToString() + "g";
                protein.Text = dailyValues[0].TotalProtein.ToString() + "g";

                model.Data1.Clear();
                
            }
            
        }

        private void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs e)
        {
            //string date = e.DateTime.Date.ToShortDateString();
            DateTime date = e.DateTime.Date;
            DateLabel.Text = e.DateTime.Date.ToShortDateString();
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            dateString = year + "-" + month + "-" + day;
            FoodLookup(dateString);
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

            await Navigation.PushAsync(new ExtraDetailsPage(dateString), true);
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
