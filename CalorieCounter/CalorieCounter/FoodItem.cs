using Newtonsoft.Json;

namespace CalorieCounter
{
    [JsonObject]
    public class FoodItem
    {
        [JsonProperty("calories")]
        public int Calories { get; set; }
    }
}
