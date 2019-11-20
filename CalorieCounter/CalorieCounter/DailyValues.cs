using Newtonsoft.Json;

namespace CalorieCounter { 

    [JsonObject]
    public class DailyValues
    {
        [JsonProperty("totalCalories")]
        public int TotalCalories { get; set; }

        [JsonProperty("totalTrans_fat")]
        public int TotalTrans_Fat { get; set; }

        [JsonProperty("totalSat_fat")]
        public int TotalSat_Fat { get; set; }

        [JsonProperty("totalcholesterol")]
        public int TotalCholesterol { get; set; }

        [JsonProperty("totalsodium")]
        public int TotalSodium { get; set; }

        [JsonProperty("totalcarbs")]
        public int TotalCarbs { get; set; }

        [JsonProperty("totalfiber")]
        public int TotalFiber { get; set; }

        [JsonProperty("totalsugars")]
        public int TotalSugars { get; set; }

        [JsonProperty("totalprotein")]
        public int TotalProtein { get; set; }
    }
}
