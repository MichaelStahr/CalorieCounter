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
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {

        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {

        }

        public void Login(string username, string password)
        {
            //var client = new RestClient("https://{YOUR-AUTH0-DOMAIN}.auth0.com");
            //var request = new RestRequest("oauth/ro", Method.POST);
            //request.AddParameter("client_id", "{YOUR-AUTH0-CLIENT-ID");
            //request.AddParameter("username", username);
            //request.AddParameter("password", password);
            //request.AddParameter("connection", "{YOUR-CONNECTION-NAME-FOR-USERNAME-PASSWORD-AUTH}");
            //request.AddParameter("grant_type", "password");
            //request.AddParameter("scope", "openid");

            //var client = new RestClient("https://accounts.google.com/o/oauth2/v2/auth");
            //var request = new RestRequest("oauth/ro", Method.POST);

        }
    }
}