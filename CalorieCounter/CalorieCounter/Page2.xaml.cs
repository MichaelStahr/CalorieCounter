using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : TabbedPage
    {
        public static string BaseAddress =
            Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        public static string miamiApiEndpoint = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php";


        public Page2()
        {
            InitializeComponent();
            _restService = new RestService();
            //SearchingFoods.Text = "Searching Foods";
        }
        
        
        public string GetFoodBySearch(string token)
        {
            // /api.asmx/GetFoodBySearch?food=string&token=string
            string food = Uri.EscapeUriString(SearchingFoods.Text);
            string requestUri = apiEndpoint;
            requestUri += "GetFoodBySearch";
            requestUri += $"?food=%{food}%";
            requestUri += $"&token={token}";

            return requestUri;
        }

        public string GetFoodBySearchAndLocation(int location, string token)
        {

            // /api.asmx / GetFoodLocationBySearch ? food = string & location_id = string & token = string

            // for now until location id's get corrected in the DB
            if (location > 2)
            {
                location++;
            }

            string food = Uri.EscapeUriString(SearchingFoods.Text);
            string requestUri = apiEndpoint;
            requestUri += "GetFoodLocationBySearch";
            requestUri += $"?food=%{food}%";
            requestUri += $"&location_id={location}";
            requestUri += $"&token={token}";

            return requestUri;
        }

        public string GetMiamiFoodByNameAndLocation(string location)
        {

            //https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php?ThisLocation=Martin

           
            string food = Uri.EscapeUriString(SearchingFoods.Text);
            string requestUri = miamiApiEndpoint;
            requestUri += $"?ThisLocation={location}";
            requestUri += $"&ThisItem={food}";

            return requestUri;
        }



        //async void QueryClick_Clicked(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(testEntry.Text))
        //    {
        //        FoodItem foodItem = await _restService.GetFoodCaloriesAsync(CreateFoodQuery());
        //        BindingContext = foodItem;
        //    }
        //}


        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            FoodLookup();
        }

        private void Locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            // access our db
            FoodLookup();
            // access Miami API and pull from db
            //MiamiFoodLookup();
        }

        async void MiamiFoodLookup()
        {
            string foods = null;
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
               
                foods = await _restService.GetMiamiFoodDataAsync(GetMiamiFoodByNameAndLocation(locations.SelectedItem.ToString()));
                
                // parse foods and store in our db

            }
            else
            {
                //foodItemslv.ItemsSource = foodItem;
            }
        }

        async void FoodLookup()
        {
            List<FoodItem> foodItem = null;
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                if (locations.SelectedIndex > 0)
                {
                    foodItem = await _restService.GetFoodDataAsync(GetFoodBySearchAndLocation(locations.SelectedIndex, "token"));
                }
                else
                {
                    foodItem = await _restService.GetFoodDataAsync(GetFoodBySearch("token"));
                }
                foodItemslv.ItemsSource = foodItem;

            }
            else
            {
                foodItemslv.ItemsSource = foodItem;
            }
        }

        private void AddFoodToLog_Clicked(object sender, EventArgs e)
        {
            FoodItem item = (FoodItem)foodItemslv.SelectedItem;
            int food_Id = item.Food_Id;
            int location_Id = item.FL_Id;
        }

    }
}