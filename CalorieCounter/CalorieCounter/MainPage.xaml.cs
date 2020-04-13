using Newtonsoft.Json.Linq;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

        private string userTokenId;
        private string dateString;
        private string uniqueId;
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;
        ChartViewModel model;
        private DateTime currentSelectedDate;
        

        public MainPage(IdToken idToken)
        {
            InitializeComponent();
            _restService = new RestService();
            model = new ChartViewModel();
           
            uniqueId = idToken.Email.Substring(0, idToken.Email.IndexOf('@'));
            GetUserIdToken();

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

            currentSelectedDate = Calendar.SelectedDate.Value;
            Preferences.Set("currentSelectedDate", currentSelectedDate);
            string year = currentSelectedDate.Year.ToString();
            string month = currentSelectedDate.Month.ToString();
            string day = currentSelectedDate.Day.ToString();
            dateString = year + "-" + month + "-" + day;

            HighlightCurrentSelectedDayOnChart();
        }

        private async void GetUserIdToken()
        {
            try
            {
                userTokenId = await SecureStorage.GetAsync("id_token");
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
        private void HighlightCurrentSelectedDayOnChart()
        {
            DateTime date = Preferences.Get("currentSelectedDate", DateTime.Today);
            DayOfWeek day = date.DayOfWeek;
            string dayStyle = day.ToString() + "Colors";
            Resources["chartStyle"] = Resources[dayStyle];
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            if (_restService != null && this.CurrentPage is ContentPage)
            {
                FoodLookup(dateString);
                GetFoodForDay();
                currentSelectedDate = Preferences.Get("currentSelectedDate", DateTime.Today);
                UpdateCalorieGraph(currentSelectedDate);
            }
        }

        protected override void OnAppearing()
        {
            FoodLookup(dateString);
            currentSelectedDate = Preferences.Get("currentSelectedDate", DateTime.Today);
            UpdateCalorieGraph(currentSelectedDate);
            //GetFoodForDay();
        }

        public string DisplayDailyValuesByUserDay(string date)
        {
            // /api.asmx/DisplayDailyValuesByUserDay
            //date = "2020-3-05";
            string requestUri = apiEndpoint;
            requestUri += "DisplayDailyValuesByUserDay";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&date={date}";
            requestUri += $"&token={userTokenId}";

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

            }
            
        }

        private async void UpdateCalorieGraph(DateTime selectedDate)
        {
            
            model.Data1.Clear();

            DayOfWeek dayOfWeek = selectedDate.DayOfWeek;
            int numberDayOfWeek = (int)dayOfWeek;

            // selectedDate is a Sunday if i = 0
            int daysBack = 0;
            int daysForward = 7;
            switch (numberDayOfWeek)
            {
                // Monday
                case 1:
                    daysBack = -1;
                    daysForward = 6;
                    break;
                case 2:
                    daysBack = -2;
                    daysForward = 5;
                    break;
                case 3:
                    daysBack = -3;
                    daysForward = 4;
                    break;
                case 4:
                    daysBack = -4;
                    daysForward = 3;
                    break;
                case 5:
                    daysBack = -5;
                    daysForward = 2;
                    break;
                case 6:
                    daysBack = -6;
                    daysForward = 1;
                    break;

            }

            for (int k = daysBack; k < daysForward; k++)
            {
                DateTime otherDateTime = selectedDate.AddDays(k);
                string otherDate = ChangeDateToString(otherDateTime);
                int comparison = DateTime.Compare(otherDateTime, DateTime.Today.AddDays(1));
                double otherTotalCals = 0;
                if (comparison < 0)
                {
                    otherTotalCals = await GetDailyCaloriesForDate(otherDate);
                }
                model.Data1.Add(new ChartData(otherDate, otherTotalCals));
            }
            //for (int g = 6; g > 0; g--)
            //{
            //    DateTime prevDateTime = selectedDate.AddDays(-g);
            //    string prevDate = ChangeDateToString(prevDateTime);
            //    double prevTotalCals = await GetDailyCaloriesForDate(prevDate);
            //    model.Data1.Add(new ChartData(prevDate, prevTotalCals));
            //}

            //model.Data1.Add(new ChartData(formattedSelectedDate, numTotalCal));
            calorieChartSeries.ItemsSource = model.Data1;
        }

       

        private string ChangeDateToString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string strDate = year + "-" + month + "-" + day;
            return strDate;
        }

        public async Task<Double> GetDailyCaloriesForDate(string date)
        {
            List<DailyValues> dailyValues = null;
            dailyValues = await _restService.DisplayDailyValuesByUserDayAsync(DisplayDailyValuesByUserDay(date));
            double totalCals = 0;
            if (dailyValues != null && dailyValues.Count != 0)
            {
                totalCals = Double.Parse(dailyValues[0].TotalCalories.ToString());
            }
            return totalCals;
        }

        private void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs e)
        {
            DateTime date = e.DateTime.Date;
            DateLabel.Text = e.DateTime.Date.ToShortDateString();
            FoodLookup(ChangeDateToString(date));
            GetFoodForDay();

            DateTime previousSelected = Preferences.Get("currentSelectedDate", DateTime.Today);
            if (CheckIfChartNeedsUpdated(previousSelected, date))
            {
                UpdateCalorieGraph(date);
            }
            Preferences.Set("currentSelectedDate", date);
            HighlightCurrentSelectedDayOnChart();
        }

        private bool CheckIfChartNeedsUpdated(DateTime previousSelected, DateTime currentSeleceted)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            int weekForPreviousDate = cal.GetWeekOfYear(previousSelected, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            int weekForCurrentDate = cal.GetWeekOfYear(currentSeleceted, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            if (!weekForCurrentDate.Equals(weekForPreviousDate))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void GoBackOrForwardADay_Clicked(object sender, EventArgs e)
        {
            
            Button button = (Button)sender;

            currentSelectedDate = Preferences.Get("currentSelectedDate", DateTime.Today);
            DateTime previousSelected = currentSelectedDate;

            if (button.Equals(GoBack))
            {
                currentSelectedDate = currentSelectedDate.AddDays(-1);
            }
            else
            {
                currentSelectedDate = currentSelectedDate.AddDays(1);
            }
            Calendar.SelectedDate = currentSelectedDate;
            DateLabel.Text = currentSelectedDate.Date.ToShortDateString();

            dateString = ChangeDateToString(currentSelectedDate);
            FoodLookup(dateString);
            GetFoodForDay();

            if (CheckIfChartNeedsUpdated(previousSelected, currentSelectedDate))
            {
                UpdateCalorieGraph(currentSelectedDate);
            }
            Preferences.Set("currentSelectedDate", currentSelectedDate);
            HighlightCurrentSelectedDayOnChart();

        }

        private void GoBackOrForwardAWeek_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            currentSelectedDate = Preferences.Get("currentSelectedDate", DateTime.Today);

            if (button.Equals(GoBackaWeek))
            {
                currentSelectedDate = currentSelectedDate.AddDays(-7);
            }
            else
            {
                currentSelectedDate = currentSelectedDate.AddDays(7);
            }

            Calendar.SelectedDate = currentSelectedDate;
            DateLabel.Text = currentSelectedDate.Date.ToShortDateString();

            dateString = ChangeDateToString(currentSelectedDate);
            FoodLookup(dateString);
            GetFoodForDay();

            UpdateCalorieGraph(currentSelectedDate);
            Preferences.Set("currentSelectedDate", currentSelectedDate);
            HighlightCurrentSelectedDayOnChart();

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

        public string DisplayFoodItemsByUserDay(string date)
        {
            // /api.asmx/GetFoodEatenByUserDay?uniqueId=string&date=string&token=string
            string requestUri = apiEndpoint;
            requestUri += "DisplayFoodItemsByUserDay";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&date={date}";
            requestUri += $"&token={userTokenId}";

            return requestUri;
        }
        async void GetFoodForDay()
        {
            List<SimpleFood> foods;
            foods = await _restService.GetSimpleFoodItemForUserAsync(DisplayFoodItemsByUserDay(dateString));
            simpleFoodlv.ItemsSource = foods;
        }

        private void JumpToToday_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            currentSelectedDate = Preferences.Get("currentSelectedDate", DateTime.Today);

            currentSelectedDate = DateTime.Today;

            Calendar.SelectedDate = currentSelectedDate;
            DateLabel.Text = currentSelectedDate.Date.ToShortDateString();

            dateString = ChangeDateToString(currentSelectedDate);
            FoodLookup(dateString);
            GetFoodForDay();

            UpdateCalorieGraph(currentSelectedDate);
            Preferences.Set("currentSelectedDate", currentSelectedDate);
            HighlightCurrentSelectedDayOnChart();

        }
    }
}
