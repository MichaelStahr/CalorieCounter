using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    // totalCalories, totalTrans_fat,totalSat_fat, totalcholesterol, totalsodium,totalcarbs, totalfiber, totalsugars, totalprotein 
    
    [JsonObject]
    public class UserLogData
    {
        [JsonProperty("totalCalories")]
        public int TotalCalories;

        [JsonProperty("totalTrans_fat")]
        public int TotalTrans_Fat;

        [JsonProperty("totalSat_fat")]
        public int TotalSat_Fat;

        [JsonProperty("totalcholesterol")]
        public int TotalCholesterol;

        [JsonProperty("totalsodium")]
        public int TotalSodium;

        [JsonProperty("totalcarbs")]
        public int TotalCarbs;

        [JsonProperty("totalfiber")]
        public int TotalFiber;

        [JsonProperty("totalsugars")]
        public int TotalSugars;

        [JsonProperty("totalprotein")]
        public int TotalProtein;

    }
}
