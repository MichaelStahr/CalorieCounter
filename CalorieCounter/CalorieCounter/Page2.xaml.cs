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
    public partial class Page2 : ContentPage
    {
        
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        public static string miamiApiEndpoint = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php";

        DateTime currentDate = DateTime.Today;

        public const string unique_id = "birdaj";
        //public const string eatsDate = "2019-03-10";
        public string eatsDate;
        public const string userToken = "dasgfdszfe";

        
        public Page2()
        {
            InitializeComponent();
            _restService = new RestService();
            eatsDate = ChangeDateToString(currentDate);
            // access Miami API and put in our DB - currently run manually
            // MiamiFoodLookup();
        }

        private string ChangeDateToString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string strDate = year + "-" + month + "-" + day;
            return strDate;
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
            requestUri += "GetFoodBySearch";
            requestUri += $"?food=%{food}%";
            requestUri += $"&token={token}";

            return requestUri;
        }

        public string SearchFoodByNameAndLocation(string location)
        {

            // /api.asmx/SearchFoodByNameAndLocation?food=string&location=string

            string food = Uri.EscapeUriString(SearchingFoods.Text);
            string requestUri = apiEndpoint;
            requestUri += "SearchFoodByNameAndLocation";
            requestUri += $"?food={food}";
            requestUri += $"&location={location}";

            return requestUri;
        }

        public string SearchFoodByLocation(string location)
        {
            // /api.asmx/SearchFoodByLocation?location=string
            string requestUri = apiEndpoint;
            requestUri += "SearchFoodByLocation";
            requestUri += $"?location={location}";

            return requestUri;
        }

        public string GetMiamiFoodByNameAndLocation(string location)
        {

            string search = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php?ThisLocation=Bell";


            //string food = Uri.EscapeUriString(SearchingFoods.Text);
            //string requestUri = miamiApiEndpoint;
            //requestUri += $"?ThisLocation={location}";
            //requestUri += $"&ThisItem={food}";

            // hard coded for now
            return search;
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

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            LookUpFoodByName();
            
            
            if(SearchingFoods.Text != "")
            {
                searchFrame.IsVisible = true;
                addFoodToLog.IsVisible = true;
                //historyFrame.IsVisible = false;
                //favoritesFrame.IsVisible = false;
            } else
            {
                searchFrame.IsVisible = false;
                addFoodToLog.IsVisible = false;
                //historyFrame.IsVisible = true;
                //favoritesFrame.IsVisible = true;
            }
        }

        private void Locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                LookUpFoodByName();
            } else
            {
                LookUpFoodByLocation();
            }
        }

        // need to figure out best time and place to call this 
        async void MiamiFoodLookup()
        {
            string foods = null;
            // returns a string representation of an xml file
            foods = await _restService.GetMiamiFoodDataAsync(GetMiamiFoodByNameAndLocation(locations.SelectedItem.ToString()));
            // send xml file to stored procedure to store in DB
            await _restService.InsertMiamiFoodDataAsync(apiEndpoint + "InsertXml", foods);

        }

        //get
        async void LookUpFoodByName()
        {
            List<MiamiItem> miamiFoodItem = null;

            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                if (locations.SelectedIndex > 0)
                {
                    string location = locations.SelectedItem.ToString();
                    miamiFoodItem = await _restService.GetFoodDataAsync(SearchFoodByNameAndLocation(location));
                    searchFrame.IsVisible = true;
                    foodItemslv.ItemsSource = miamiFoodItem;
                }

               
            }
           
        }

        async void LookUpFoodByLocation()
        {
            List<MiamiItem> miamiFoodItem = null;

            if (locations.SelectedIndex > 0)
            {
                miamiFoodItem = await _restService.GetFoodDataAsync(SearchFoodByLocation(locations.SelectedItem.ToString()));
                searchFrame.IsVisible = true;
                foodItemslv.ItemsSource = miamiFoodItem;
                addFoodToLog.IsVisible = true;
            } else
            {
                foodItemslv.ItemsSource = null;
                addFoodToLog.IsVisible = false;
                searchFrame.IsVisible = false;
            }

        }

        async void InsertFoodForUser(MiamiItem item)
        {
            // uniqueId=string&offeredId=string&date=string
            if (item != null)
            {
                string date = ChangeDateToString(currentDate);
                string data = "uniqueId=" + unique_id + "&offeredId=" + item.Offered_id + "&date=" + date;
                
                await _restService.InsertFoodIntoUserEats(apiEndpoint + "UserEatFood", data);
            }
          
        }

        private void AddFoodToLog_Clicked(object sender, EventArgs e)
        {
            MiamiItem mItem = (MiamiItem)foodItemslv.SelectedItem;
            
            InsertFoodForUser(mItem);
        }
    }
}