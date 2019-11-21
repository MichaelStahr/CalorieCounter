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

        public string GetFoodEatenByUserDay(string date)
        {
            // /api.asmx/GetFoodEatenByUserDay?uniqueId=string&date=string&token=string
            string requestUri = apiEndpoint;
            requestUri += "GetFoodEatenByUserDay";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&eatDate={date}";
            requestUri += $"&token={token}";

            return requestUri;
        }

        protected override void OnAppearing()
        {
            GetFoodForDay();
        }

        async void GetFoodForDay()
        {
            List<UserItemsByDay> foodItems;
            foodItems = await _restService.GetFoodEatenByUserDayAsync(GetFoodEatenByUserDay(dateString));
            foodLog.ItemsSource = foodItems;
            int i = 0;

            foodLog.ItemTemplate = new DataTemplate(() =>
            {
                Label name = new Label();
                name.Text = foodItems[i].Foodname.ToString();
                name.FontSize = 20;

                Label calsText = new Label();
                calsText.Text = "Calories: " + foodItems[i].Calories.ToString() + "g";

                Label carbsText = new Label();
                carbsText.Text = "Carbs: " + foodItems[i].Carbs.ToString() + "g";

                Label sugarsText = new Label();
                sugarsText.Text = "Sugars: " + foodItems[i].Sugars.ToString() + "g";

                Label proteinsText = new Label();
                proteinsText.Text = "Protein: " + foodItems[i].Protein.ToString() + "g";

                i++;
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Padding = 25,
                        Children =
                        {
                            new StackLayout
                            {
                                Children =
                                {
                                    name,
                                }
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Spacing = 30,
                                Children =
                                {
                                    calsText, carbsText, sugarsText, proteinsText,
                                }
                            },
                            
                        }
                    }
                };
            });
        }
    }
}