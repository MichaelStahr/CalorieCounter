using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Rg.Plugins.Popup.Contracts;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public static string BaseAddress = "http://caloriecounter.mikestahr.com";
        //Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        public static string miamiApiEndpoint = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php";

        private DateTime currentDate = DateTime.Today;

        private readonly string unique_id;
        private string eatsDate;
        private string userTokenId;
        AddPopUpViewModel popUpView;
        //AddPopUpViewModel selectedItems;
        //AddItemsPopUp selectedItemsCount;
        int count;
        public Page2()
        {
            InitializeComponent();
            _restService = new RestService();
            eatsDate = ChangeDateToString(currentDate);
            unique_id = Preferences.Get("user", "");
            GetUserIdToken();
            // access Miami API and put in our DB - currently uncomment and run manually
            //MiamiFoodLookup();
            popUpView = new AddPopUpViewModel();
            //selectedItems = new AddPopUpViewModel();
            count = 0;
            GetActiveLocations();
        }

        /// <summary>
        /// Get the user's IdToken
        /// </summary>
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

        /// <summary>
        /// Change a date to a string version of itself
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string ChangeDateToString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string strDate = year + "-" + month + "-" + day;
            return strDate;
        }

        protected void OnCurrentPageChanged()
        {
            if (popUpView.ItemData.Count == 0)
            {
                count = 0;
            }
        }

        /// <summary>
        /// Gets active or open dining locations
        /// </summary>
        public async void GetActiveLocations()
        {
            // /api.asmx/GetActiveLocations?
            List<Location> activeLocations = new List<Location>();
            activeLocations = await _restService.GetActiveLocations(apiEndpoint + "GetActiveLocations?");
            
            for (int i = 0; i < locationsPicker.Items.Count; i++)
            {
                bool isOpen = false;
                string locationName = locationsPicker.Items[i];
                foreach (Location l in activeLocations)
                {
                    if (l.ServiceUnit.Equals(locationName))
                    {
                        isOpen = true;
                        break;
                    }
                }
                if (!isOpen)
                {
                    locationsPicker.Items[i] = locationName + " (not open)";
                }
            }
            
        }

        /// <summary>
        /// Returns the request Uri for searching a food by it's name and location
        /// </summary>
        /// <param name="location"></param>
        /// <returns>
        /// The request Uri
        /// </returns>
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

        /// <summary>
        /// Returns the request Uri for searching foods by location only
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public string SearchFoodByLocation(string location)
        {
            // /api.asmx/SearchFoodByLocation?location=string
            string requestUri = apiEndpoint;
            requestUri += "SearchFoodByLocation";
            requestUri += $"?location={location}";

            return requestUri;
        }

        /// <summary>
        /// Returns the request Uri for retrieving all items at a location on Miami's campus
        /// Change the last param (the location) to get another location's items
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public string GetMiamiFoodByLocation(string location)
        {
            // Locations: ASC, King, Martin...
            string search = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php?ThisLocation=ASC";

            return search;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            LookUpFoodByNameAndLocation();
            
            
            if(SearchingFoods.Text != "")
            {
                //searchFrame.IsVisible = true;
                SaveSelectedItems.IsEnabled = true;
                
            } else if (SearchingFoods.Text.Equals(""))
            {
                //searchFrame.IsVisible = true;
                SaveSelectedItems.IsEnabled = true;
                LookUpFoodByLocation();
                
            } else 
            {
                //searchFrame.IsVisible = false;
                SaveSelectedItems.IsEnabled = false;
            }
        }

        private void Locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                LookUpFoodByNameAndLocation();
            } else
            {
                LookUpFoodByLocation();
            }
        }

        /// <summary>
        /// Stores all Miami items at a location in our database
        /// Currently have to uncomment in the contructor and run manually for each location you want food to be added
        /// </summary>
        async void MiamiFoodLookup()
        {
            string foods = null;
            // returns a string representation of an xml file
            foods = await _restService.GetMiamiFoodDataAsync(GetMiamiFoodByLocation(""));
            // send xml file to stored procedure to store in DB
            await _restService.InsertMiamiFoodDataAsync(apiEndpoint + "InsertXml", foods);

        }

        /// <summary>
        /// Displays food by it's name and location
        /// </summary>
        async void LookUpFoodByNameAndLocation()
        {
            List<MiamiItem> miamiFoodItem = new List<MiamiItem>();

            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                
                    string location = locationsPicker.SelectedItem.ToString();
                    miamiFoodItem = await _restService.GetFoodDataAsync(SearchFoodByNameAndLocation(location));
                    foreach (MiamiItem m in miamiFoodItem)
                    {
                        double d = Double.Parse(m.CaloriesK);
                        d = Math.Round(d, 2);
                        m.CaloriesK = d.ToString();
                    }
                    foodItemslv.ItemsSource = miamiFoodItem;
                    if (miamiFoodItem.Count > 0)
                    {
                        SaveSelectedItems.IsEnabled = true;
                    } else
                    {
                        SaveSelectedItems.IsEnabled = false;
                    }
                
            }
           
        }

        /// <summary>
        /// Displays food by location only
        /// </summary>
        async void LookUpFoodByLocation()
        {
            List<MiamiItem> miamiFoodItem = new List<MiamiItem>();

            
                miamiFoodItem = await _restService.GetFoodDataAsync(SearchFoodByLocation(locationsPicker.SelectedItem.ToString()));
                //searchFrame.IsVisible = true;
                foreach (MiamiItem m in miamiFoodItem)
                {
                    double d = Double.Parse(m.CaloriesK);
                    d = Math.Round(d, 2);
                    m.CaloriesK = d.ToString();
                }
                foodItemslv.ItemsSource = miamiFoodItem;
                if (miamiFoodItem.Count > 0)
                {
                    SaveSelectedItems.IsEnabled = true;
                } else
                {
                    SaveSelectedItems.IsEnabled = false;
                }
            
        }

        private async void ShowFoodsToBeAdded_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddItemsPopUp(popUpView, counterLabel));
        }

        /// <summary>
        /// When an item in the food list is tapped, keep track of the count and add it to the AddItemsPopup list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoodItemslv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MiamiItem mItem = (MiamiItem)foodItemslv.SelectedItem;
            bool check = false;
            if(popUpView.ItemData.Count == 0)
            {
                count = 0;
            }
            else
            {
                count = Int32.Parse(counterLabel.Text);
            }
            count++;
            counterLabel.Text = count.ToString();
            
            foreach (AddItemPopUpModel m in popUpView.ItemData)
            {
                if (m.Item.Equals(mItem))
                {
                    m.Count++;
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                popUpView.ItemData.Add(new AddItemPopUpModel(mItem, 1));
            }
        }
    }
}