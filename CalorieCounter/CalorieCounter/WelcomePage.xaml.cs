using Google.Apis.Auth;
using Jose;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
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
            _restService = new RestService();
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Login();
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {

        }

        public async void Login()
        {
            // authenticate
            string client_id = "";
            string redirect_uri = "";
            if (Device.RuntimePlatform == Device.Android)
            {
                client_id = "1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf.apps.googleusercontent.com";
                redirect_uri = "com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf:/oauth2redirect";

            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                client_id = "1041253101002-hbmvlv5gofcv8fkh3du3eb1sd0jputfp.apps.googleusercontent.com";
                redirect_uri = "caloriecounter.oauth2:/oauth2redirect";

            }
           
            string requestUri = "https://accounts.google.com/o/oauth2/v2/auth?";
            requestUri += $"scope=openid%20email%20profile";
            requestUri += $"&response_type=code";
            requestUri += $"&redirect_uri={redirect_uri}";
            requestUri += $"&client_id={client_id}";
            requestUri += "&hd=miamioh.edu";
            requestUri += "&prompt=select_account";
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri(requestUri),
                new Uri(redirect_uri));

            activityIndicator.IsRunning = true;
            string code = authResult.Properties["code"];
            string content = $"code={code}&client_id={client_id}&redirect_uri={redirect_uri}&grant_type=authorization_code";
            string tokenUrl = "https://oauth2.googleapis.com/token";

            IdToken idToken = null;
            TokenResponse result = await _restService.ObtainAccessToken(tokenUrl, content);
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                string j = decoder.Decode(result.IdToken);
                idToken = JsonConvert.DeserializeObject<IdToken>(j);
                
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }

            // if user exists (if idToken matches idToken of user in DB) then login
            activityIndicator.IsRunning = false;
            await Navigation.PushModalAsync(new MainPage(idToken));

        }

    }
}