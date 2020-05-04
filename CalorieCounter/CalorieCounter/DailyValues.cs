using Newtonsoft.Json;

namespace CalorieCounter { 

    /// <summary>
    /// The nutritional values associated with each food item returned from Miami's API
    /// </summary>
    [JsonObject]
    public class DailyValues
    {
        [JsonProperty("caloriesk", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalCalories { get; set; }

        [JsonProperty("fat", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalFat { get; set; }

        [JsonProperty("cholesterol", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalCholesterol { get; set; }

        [JsonProperty("sodium", NullValueHandling = NullValueHandling.Ignore), ]
        public double TotalSodium { get; set; }

        [JsonProperty("carbs", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalCarbs { get; set; }

        [JsonProperty("calcium", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalCalcium { get; set; }

        [JsonProperty("sugar", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalSugars { get; set; }

        [JsonProperty("protein", NullValueHandling = NullValueHandling.Ignore)]
        public double TotalProtein { get; set; }
    }
}
