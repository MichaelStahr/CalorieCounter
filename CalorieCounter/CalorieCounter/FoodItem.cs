using Newtonsoft.Json;

namespace CalorieCounter
{
    [JsonObject]
    public class FoodItem
    {

        [JsonProperty("Food_ID")]
        public int Food_Id { get; set; }

        [JsonProperty("FL_ID")]
        public int FL_Id { get; set; }

        [JsonProperty("calories")]
        public int Calories { get; set; }

        [JsonProperty("FoodName")]
        public string FoodName { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", FoodName, LocationName);
        }
    }
}
