using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraDetailsPage : ContentPage
    {
        RestService _restService;

        const string token = "dasgfdszfe";
        // date is initialized to calendar date of today on start
        public string dateString;
        const string uniqueId = "birdaj";
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";

        public ExtraDetailsPage(string date)
        {
            InitializeComponent();
            _restService = new RestService();
            dateString = date;
            
        }

        public string DisplayDailyValuesByUserDay(string date)
        {
            // /api.asmx/GetFoodEatenByUserDay?uniqueId=string&date=string&token=string
            string requestUri = apiEndpoint;
            requestUri += "GetFoodEatenByUserDay";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&date={date}";
            requestUri += $"&token={token}";

            return requestUri;
        }

        protected override void OnAppearing()
        {
            GetFoodForDay();
        }

        async void GetFoodForDay()
        {
            List<FoodItem> foodItems;
            foodItems = await _restService.GetFoodDataAsync(DisplayDailyValuesByUserDay(dateString));
            foodLog.ItemsSource = foodItems;
        }
    }
}