using Google.Apis.Auth;
using Jose;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {

        public static string BaseAddress = "http://caloriecounter.mikestahr.com";
        //Device.RuntimePlatform == Device.Android ? "https://10.0.2.2:44341" : "https://localhost:44341";
        public static string apiEndpoint = $"{BaseAddress}/api.asmx/";
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
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 3,
                Children =
                {

                    new Label {Text = "Miami University",
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.FromHex("C05746"),
                    FontSize = 25,
                    FontFamily = Device.RuntimePlatform == Device.Android ? "Acme-Regular.ttf#Acme-Regular" : null,
                    HorizontalOptions = LayoutOptions.EndAndExpand},

                        
                    
                }
            };
            //NavigationPage.SetTitleView(this, header);
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
            try
            {
                IdToken idToken = await AuthenticateUser();
                welcomeName.Text = "Welcome " + idToken.GivenName + "!";
                welcomeName.IsVisible = true;
                string uniqueId = idToken.Email.Substring(0, idToken.Email.IndexOf('@'));
                // if user exists (idToken.sub matches token field of user in DB) then login
                bool userExists = await _restService.GetUser(GetUserUri(uniqueId, idToken.Sub));

                // user doesn't exist, sign up user with idToken info
                if (!userExists)
                {
                    SignUpUser(uniqueId, idToken);
                }

                // store their tokenID returned from Google Auth (always unique)
                try
                {
                    await SecureStorage.SetAsync("id_token", idToken.Sub);
                }
                catch (Exception ex)
                {
                    // Possible that device doesn't support secure storage on device.
                }
                Preferences.Set("user", uniqueId);
                Preferences.Set("firstName", idToken.GivenName);
                Preferences.Set("lastName", idToken.FamilyName);
                Preferences.Set("email", idToken.Email);
                Preferences.Set("picture", idToken.Picture);
                await Navigation.PushModalAsync(new MainPage());
            } catch { }

            
        }

        private async void SignUpButton_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new SignUpPage());
            
            try
            {
                await SecureStorage.SetAsync("id_token", "");
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
            Preferences.Set("user", "birdaj");
            Preferences.Set("firstName", "Alec");
            Preferences.Set("lastName", "Bird");
            Preferences.Set("email", "birdaj@miamioh.edu");
            Preferences.Set("picture", "https://lh3.googleusercontent.com/-P5lBiBQZ1i0/AAAAAAAAAAI/AAAAAAAAAAA/AAKWJJMf0a_ABrMXci_Omledzhi_YGtBTw/s96-c/photo.jpg");
            await Navigation.PushModalAsync(new MainPage());
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

            //activityIndicator.IsRunning = false;
            //LoggingIn.IsVisible = false;
            return idToken;

        }

        public string GetUserUri(string uniqueId, string tokenId)
        {
            // api.asmx/GetUser?uniqueId=string&tokenId=string
            string requestUri = apiEndpoint;
            requestUri += "GetUser";
            requestUri += $"?uniqueId={uniqueId}";
            requestUri += $"&tokenId={tokenId}";

            return requestUri;
        }

        public async void SignUpUser(string uniqueId, IdToken idToken)
        {
            string content = $"uniqueId={uniqueId}&firstName={idToken.GivenName}&lastName={idToken.FamilyName}&email={idToken.Email}&tokenId={idToken.Sub}";
            string requestUri = apiEndpoint + "InsertNewUser";
            await _restService.InsertNewsUser(requestUri, content);
        }

    }
}