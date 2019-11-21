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
        [JsonProperty("uniqueId")]
        public string UniqueId;

        [JsonProperty("date")]
        public string Date;

        [JsonProperty("token")]
        public string Token;
    }
}
