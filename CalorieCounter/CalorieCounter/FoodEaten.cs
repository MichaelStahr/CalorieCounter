using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    [JsonObject]
    public class FoodEaten
    {
        [JsonProperty("uniqueId")]
        public string UniqueID { get; set; }

        [JsonProperty("foodId")]
        public int FoodID { get; set; }

        [JsonProperty("eatsDate")]
        public string EatsDate { get; set; }

        [JsonProperty("location_Id")]
        public int LocationID { get; set; }

        [JsonProperty("multiplier")]
        public int Multiplier { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

    }
}
