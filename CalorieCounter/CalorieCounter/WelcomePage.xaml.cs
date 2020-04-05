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
    public partial class WelcomePage : ContentPage
    {
        RestService _restService;
        public WelcomePage()
        {
            InitializeComponent();
            _restService = new RestService();
            StackLayout header = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Spacing = 3,
                Children =
                {
                    
                    new Label {Text = "Miami University",
                    FontSize = 25,
                    FontFamily = Device.RuntimePlatform == Device.Android ? "Acme-Regular.ttf#Acme-Regular" : null,
                    HorizontalOptions = LayoutOptions.EndAndExpand},

                        
                    
                }
            };
            NavigationPage.SetTitleView(this, header);
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Login(email.Text, password.Text);
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {

        }

        public async void Login(string username, string password)
        {
            // to do: verify login through stored procedure
            //Navigation.PushModalAsync(new MainPage());
            
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri("https://accounts.google.com/o/oauth2/v2/auth?client_id=1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf.apps.googleusercontent.com&response_type=code&scope=email%20profile&redirect_uri=com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf:/oauth2redirect"),
                new Uri("com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf:/oauth2redirect"));
            string code = authResult.Properties["code"];
            var accessToken = authResult?.AccessToken;
        }

    }
}