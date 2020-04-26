using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemsPopUp : PopupPage
    {

        public static string BaseAddress = "http://caloriecounter.mikestahr.com";
        //Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;

        private ObservableCollection<AddItemPopUpModel> itemData;
        private DateTime currentDate = DateTime.Today;
        private readonly string unique_id;
        private Label count;
        
        
        public AddItemsPopUp(AddPopUpViewModel viewModel, Label count)
        {
            InitializeComponent();
            //this.itemList = viewModel.ItemData;
            this.itemData = viewModel.ItemData;
            addedItemsLv.ItemsSource = itemData;
            unique_id = Preferences.Get("user", "");
            _restService = new RestService();
            this.count = count;
        }

        private async void Close_Button_Clicked(object sender, EventArgs e)
        {
            int totalCount = 0;
            foreach(AddItemPopUpModel i in itemData)
            {
                totalCount += i.Count;
            }
            count.Text = totalCount.ToString();
            await PopupNavigation.Instance.PopAsync(true);
        }

        protected override void OnAppearing()
        {
            if (itemData == null || itemData.Count == 0)
            {
                DeleteButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
            } else
            {
                DeleteButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            itemData.Clear();
            count.Text = 0.ToString();
            DeleteButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (itemData != null && itemData.Count > 0)
            {
                foreach (AddItemPopUpModel item in itemData)
                {
                    for (int i = 1; i <= item.Count; i++)
                    {
                        if (item.Count > 0)
                        {
                            InsertFoodForUser(item.Item);
                        }
                    }
                }
                itemData.Clear();
                //DisplayAlert("", "Items have been added!", "Close");
                SaveButton.IsEnabled = false;
                await PopupNavigation.Instance.PopAsync(true);
            }
            count.Text = 0.ToString();
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

        private void MinusButton_Clicked(object sender, EventArgs e)
        {
            int minus = Int32.Parse(count.Text);
            Button minusButton = (Button)sender;
            StackLayout layout = (StackLayout)minusButton.Parent;
            Grid g = (Grid)layout.Parent;
            Label countLabel = (Label)g.Children[1];
            int num = Int32.Parse(countLabel.Text);
            if (num > 0)
            {
                num--;
                minus--;
            }
            if (num == 0)
            {
                minusButton.IsEnabled = false;
            } else
            {
                minusButton.IsEnabled = true;
            }
            countLabel.Text = num.ToString();
            count.Text = minus.ToString();
            
        }

        private void PlusButton_Clicked(object sender, EventArgs e)
        {
            int add = Int32.Parse(count.Text);
            Button plusButton = (Button)sender;
            StackLayout layout = (StackLayout)plusButton.Parent;
            Button minusButton = (Button)layout.Children[0];
            Grid g = (Grid)layout.Parent;
            Label countLabel = (Label)g.Children[1];
            int num = Int32.Parse(countLabel.Text);
            num++;
            add++;
            countLabel.Text = num.ToString();
            minusButton.IsEnabled = true;
            count.Text = add.ToString();
        }
    }
}