using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {

        //public static string BaseAddress = "http://caloriecounter.mikestahr.com";
        //Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string BaseAddress = "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
        RestService _restService;
        public Page1()
        {
            InitializeComponent();
            genPicker();
            genFeetPicker();
            genInchesPicker();
            _restService = new RestService();
            string picUrl = Preferences.Get("picture", "");
            profilePic.Source = ImageSource.FromUri(new Uri(picUrl));
            firstName.Text = Preferences.Get("firstName", "");
            lastName.Text = Preferences.Get("lastName", "");
            name.Text = Preferences.Get("user", "");
            email.Text = Preferences.Get("email", "");
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private void genPicker()
        {
            for (int i = 50; i <= 500; i++)
            {
                weightPicker.Items.Add(i + "");
            }
        }

        private void genFeetPicker()
        {
            for (int i = 1; i <= 10; i++)
            {
                feetPicker.Items.Add(i + "");
            }
        }

        private void genInchesPicker()
        {
            for (int i = 1; i <= 11; i++)
            {
                inchesPicker.Items.Add(i + "");
            }
        }

        public async void UpdateUserWeightAndHeight(string uniqueId, string idToken, int weight, int height)
        {
            //uniqueId = string & tokenId = string & weight = string & height = string
            string content = $"uniqueId={uniqueId}&tokenId={idToken}&weight={weight}&height={height}";
            string requestUri = apiEndpoint + "UpdateWeightHeight";
            await _restService.UpdateWeightAndHeightForUser(requestUri, content);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string id = await SecureStorage.GetAsync("id_token");
                int weight = Int32.Parse(weightPicker.SelectedItem.ToString());
                int height = (Int32.Parse(feetPicker.SelectedItem.ToString())*12) + Int32.Parse(inchesPicker.SelectedItem.ToString());
                UpdateUserWeightAndHeight(name.Text, id, weight, height);

            }
            catch { };

        }
    }
}