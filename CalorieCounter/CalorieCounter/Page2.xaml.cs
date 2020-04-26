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
        AddPopUpViewModel selectedItems;
        AddItemsPopUp selectedItemsCount;
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
            selectedItems = new AddPopUpViewModel();
            count = 0;
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

        protected override void OnAppearing()
        {
            //searchFrame.IsVisible = false;
            //SaveSelectedItems.IsEnabled = true;
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

            string search = "https://www.hdg.miamioh.edu/Code/MyCard/MyFSSNutritionalAPI.php?ThisLocation=ASC";


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
            requestUri += $"&token={userTokenId}";

            return requestUri;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            LookUpFoodByNameAndLocation();
            
            
            if(SearchingFoods.Text != "" && locations.SelectedIndex > 0)
            {
                //searchFrame.IsVisible = true;
                SaveSelectedItems.IsEnabled = true;
                
            } else if (SearchingFoods.Text.Equals("") && locations.SelectedIndex > 0)
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
        async void LookUpFoodByNameAndLocation()
        {
            List<MiamiItem> miamiFoodItem = new List<MiamiItem>();

            if (!string.IsNullOrWhiteSpace(SearchingFoods.Text))
            {
                if (locations.SelectedIndex > 0)
                {
                    string location = locations.SelectedItem.ToString();
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
           
        }

        async void LookUpFoodByLocation()
        {
            List<MiamiItem> miamiFoodItem = new List<MiamiItem>();

            if (locations.SelectedIndex > 0)
            {
                miamiFoodItem = await _restService.GetFoodDataAsync(SearchFoodByLocation(locations.SelectedItem.ToString()));
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
            } else
            {
                foodItemslv.ItemsSource = null;
                //SaveSelectedItems.IsVisible = false;
                //searchFrame.IsVisible = false;
                SaveSelectedItems.IsEnabled = false;
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

        private void QuickAdd_Clicked(object sender, EventArgs e)
        {
            MiamiItem mItem = (MiamiItem)foodItemslv.SelectedItem;
            if (mItem != null)
            {
                InsertFoodForUser(mItem);
                DisplayAlert("", "Item added!", "Close");
            }
        }

        private async void ShowFoodsToBeAdded_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddItemsPopUp(popUpView, counterLabel));
        }

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