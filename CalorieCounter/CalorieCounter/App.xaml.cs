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
            MainPage = new NavigationPage(new MainPage())
            {
                
                BarBackgroundColor = Color.FromHex("ADC698"),
            
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
