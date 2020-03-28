using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CalorieCounter
{
    public class AddPopUpViewModel
    {
       //public ObservableCollection<MiamiItem> ItemData { get; set; }

       public ObservableCollection<AddItemPopUpModel> ItemData { get; set; }
        public AddPopUpViewModel()
        {
            //ItemData = new ObservableCollection<MiamiItem>();
            ItemData = new ObservableCollection<AddItemPopUpModel>();
        }

       

        
       
    }
}
