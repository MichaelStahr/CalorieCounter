using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<MiamiItem> ItemList;
        public AddItemsPopUp(AddPopUpViewModel viewModel)
        {

            InitializeComponent();
            this.ItemList = viewModel.ItemData;
            addedItemsLv.ItemsSource = ItemList;

        }

        private async void Close_Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync(true);
        }


    }
}