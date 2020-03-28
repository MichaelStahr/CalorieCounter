using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    public class AddItemPopUpModel
    {
        public MiamiItem Item { get; set; }

        public int Count { get; set; }
        public AddItemPopUpModel(MiamiItem item, int count)
        {
            this.Item = item;
            this.Count = count;
        }

        public MiamiItem GetItems()
        {
            return Item;
        }
    }
}
