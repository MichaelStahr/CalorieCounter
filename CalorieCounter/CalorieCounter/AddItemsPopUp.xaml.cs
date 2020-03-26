using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class AddItemsPopUp : PopupPage
    {
        public AddItemsPopUp()
        {
            InitializeComponent();
        }

        private async void Close_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }
    }
}