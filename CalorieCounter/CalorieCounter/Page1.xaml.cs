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
        public Page1()
        {
            InitializeComponent();
            genPicker();
            genFeetPicker();
            genInchesPicker();
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
    }
}