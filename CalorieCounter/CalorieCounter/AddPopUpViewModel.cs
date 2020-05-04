using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// The collection of items on the AddItemsPopUp page
    /// </summary>
    public class AddPopUpViewModel
    {
      
       public ObservableCollection<AddItemPopUpModel> ItemData { get; set; }
        public AddPopUpViewModel()
        {
            ItemData = new ObservableCollection<AddItemPopUpModel>();
        }

       

        
       
    }
}
