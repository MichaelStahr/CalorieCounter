using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : TabbedPage
    {
        
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx";
        RestService _restService;

        public static string miamiApiEndpoint = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php";

        public const string unique_id = "birdaj";
        public const string eatsDate = "2019-11-20";
        public const string userToken = "dasgfdszfe";

        public Page2()
        {
            InitializeComponent();
            _restService = new RestService();
            //SearchingFoods.Text = "Searching Foods";
        }

        protected override void OnAppearing()
        {
            searchFrame.IsVisible = false;
        }

        public string GetFoodBySearch(string token)
        {
            // /api.asmx/GetFoodBySearch?food=string&token=string
            string food = Uri.EscapeUriString(SearchingFoods.Text);
            string requestUri = apiEndpoint;
            requestUri += "/GetFoodBySearch";
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


            //string food = Uri.EscapeUriString(SearchingFoods.Text);
            //string requestUri = miamiApiEndpoint;
            //requestUri += $"?ThisLocation={location}";
            //requestUri += $"&ThisItem={food}";


            return "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php?ThisLocation=Martin";
        }

        public string InsertMiamiFoodItems(string date, int location)
        {

            string requestUri = apiEndpoint;
            requestUri += "InsertXml";
            requestUri += $"?date={date}";
            requestUri += $"&location={location}";

            return requestUri;
        }

        public string UpdateDailyLogByUser()
        {
            // /api.asmx / UpdateDailyValuesByUserDay ? uniqueId = string & date = string & token = string

            string requestUri = apiEndpoint;
            requestUri += "UpdateDailyLogByUserDay";
            requestUri += $"?uniqueId={unique_id}";
            requestUri += $"&date={eatsDate}";
            requestUri += $"&token={userToken}";

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
            
            if(SearchingFoods.Text != "")
            {
                searchFrame.IsVisible = true;
                addFoodToLog.IsVisible = true;
                historyFrame.IsVisible = false;
                favoritesFrame.IsVisible = false;
            } else
            {
                searchFrame.IsVisible = false;
                addFoodToLog.IsVisible = false;
                historyFrame.IsVisible = true;
                favoritesFrame.IsVisible = true;
            }
        }

        private void Locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            // access our db
            //FoodLookup();
            // access Miami API and pull from db
            MiamiFoodLookup();
        }

        // need to figure out best time and place to call this 
        async void MiamiFoodLookup()
        {
            string foods = null;
            XElement items = null;
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
               
                //foods = await _restService.GetMiamiFoodDataAsync(GetMiamiFoodByNameAndLocation(locations.SelectedItem.ToString()));
                // returns an xml file
                foods = await _restService.GetMiamiFoodDataAsync(GetMiamiFoodByNameAndLocation(locations.SelectedItem.ToString()));
                // send xml file to stored procedure
                //await _restService.InsertMiamiFoodDataAsync(apiEndpoint + "InsertXml", foods);
                await _restService.InsertMiamiFoodDataAsync(apiEndpoint + "/InsertXml", foods);

                // parse foods and store in our db

            }
            else
            {
                //foodItemslv.ItemsSource = foodItem;
            }
        }

        //get
        async void FoodLookup()
        {
            List<FoodItem> foodItem = null;
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                int num = locations.SelectedIndex;
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

        //post
        async void UpdateDailyLog()
        {
            UserLogData data = new UserLogData
            {
                UniqueId = unique_id,
                Date = eatsDate,
                Token = userToken,
            };
            await _restService.UpdateDailyLogForUser(UpdateDailyLogByUser(), data);
            
        }

        //post
        async void InsertFood(int food_Id, int location_Id)
        {
            
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                
                FoodEaten foodAdded = new FoodEaten
                {
                    UniqueID = unique_id,
                    FoodID = food_Id,
                    EatsDate = eatsDate,
                    LocationID = location_Id,
                    Multiplier = 1,
                    Token = userToken,
                };
                await _restService.InsertFoodIntoLogForUser(apiEndpoint + "InsertUserEatsFood", foodAdded);
                
            }
            
        }

        private void AddFoodToLog_Clicked(object sender, EventArgs e)
        {
            FoodItem item = (FoodItem)foodItemslv.SelectedItem;
            int food_Id = item.Food_Id;
            int location_Id = item.FL_Id;
            // insert food to log
            InsertFood(food_Id, location_Id);
            UpdateDailyLog();
        }
    }
}