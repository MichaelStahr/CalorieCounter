using Google.Apis.Auth;
using Jose;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
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
        private string client_id;
        private string redirect_uri;
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
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            IdToken idToken = await AuthenticateUser();
            try
            {
                await SecureStorage.SetAsync("id_token", idToken.Sub);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            string uniqueId = idToken.Email.Substring(0, idToken.Email.IndexOf('@'));
            Preferences.Set("user", uniqueId);
            // if idToken.sub = id field of user in DB, login
            // if not, direct user to sign up page with prefilled entries for name and email 
            await Navigation.PushModalAsync(new MainPage(idToken));
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private void AboutButton_Clicked(object sender, EventArgs e)
        {

        }

        public async Task<IdToken> AuthenticateUser()
        {
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

            // while token is fetched to gather user info, disable page and show loading symbol 
            activityIndicator.IsRunning = true;
            LoggingIn.IsVisible = true;
            this.Content.IsEnabled = false;

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

            activityIndicator.IsRunning = false;
            LoggingIn.IsVisible = false;

            // if user exists (idToken.sub matches token field of user in DB) then login
            return idToken;

        }

    }
}