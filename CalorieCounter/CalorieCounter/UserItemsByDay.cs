using Newtonsoft.Json;

namespace CalorieCounter
{
    [JsonObject]
    public class UserItemsByDay
    {
        [JsonProperty("food_id")]
        public int Food_id { get; set; }

        [JsonProperty("foodname")]
        public string Foodname { get; set; }

        [JsonProperty("FL_ID")]
        public int FL_ID { get; set; }

        [JsonProperty("LocationName")]
        public string LocationName { get; set; }

        [JsonProperty("calories")]
        public int Calories { get; set; }

        [JsonProperty("trans_fat")]
        public int Trans_Fat { get; set; }

        [JsonProperty("sat_fat")]
        public int Sat_Fat { get; set; }

        [JsonProperty("cholesterol")]
        public int Cholesterol { get; set; }

        [JsonProperty("sodium")]
        public int Sodium { get; set; }

        [JsonProperty("carbs")]
        public int Carbs { get; set; }

        [JsonProperty("fiber")]
        public int Fiber { get; set; }

        [JsonProperty("sugars")]
        public int Sugars { get; set; }

        [JsonProperty("protein")]
        public int Protein { get; set; }
    }
}
