using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CalorieCounter
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU4MzE3QDMxMzcyZTMzMmUzMGtxYThmb2NzaGxuMWJhZy9Eemk5WUhnMmZ2S3N3ZUZNU2VTdTdvWjRWaDg9");
            InitializeComponent();
            ColorTypeConverter color = new ColorTypeConverter();
            Color.FromHex("ADC698");
            //MainPage = new NavigationPage(new MainPage())
            MainPage = new NavigationPage(new SignUpPage())
            {

                //#CBDFBD
                //#437C90
                BarBackgroundColor = Color.FromHex("#CBDFBD"),
            
            };
        }

        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
