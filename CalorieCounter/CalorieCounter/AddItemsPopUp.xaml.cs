using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemsPopUp : PopupPage
    {
        
        public static string BaseAddress =
        Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        private ObservableCollection<AddItemPopUpModel> itemData;
        private DateTime currentDate = DateTime.Today;
        private const string unique_id = "birdaj";
        
        
        public AddItemsPopUp(AddPopUpViewModel viewModel)
        {
            InitializeComponent();
            //this.itemList = viewModel.ItemData;
            this.itemData = viewModel.ItemData;
            addedItemsLv.ItemsSource = itemData;
            _restService = new RestService();
        }

        private async void Close_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            //MiamiItem mItem = (MiamiItem)addedItemsLv.SelectedItem;
            AddItemPopUpModel mItem = (AddItemPopUpModel)addedItemsLv.SelectedItem;

            if (mItem != null)
            {
                itemData.Remove(mItem);
            } else
            {
                DisplayAlert("Attention", "Select an item to delete it", "Close");
            }
        }

        private void AddAllButton_Clicked(object sender, EventArgs e)
        {
            if (itemData != null && itemData.Count > 0)
            {
                foreach (AddItemPopUpModel item in itemData)
                {
                    for (int i = 1; i <= item.Count; i++)
                    {
                        InsertFoodForUser(item.Item);
                    }
                }
                itemData.Clear();
                DisplayAlert("Attention", "Items have been added!", "Close");
            }
            else
            {
                DisplayAlert("Attention", "Select items in order to add them", "Close");
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

        private string ChangeDateToString(DateTime date)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString();
            string day = date.Day.ToString();
            string strDate = year + "-" + month + "-" + day;
            return strDate;
        }

       
    }
}