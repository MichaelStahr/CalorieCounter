
namespace CalorieCounter
{
    /// <summary>
    /// A model for an item appearing in the listview of AddItemsPopUp page
    /// </summary>
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
