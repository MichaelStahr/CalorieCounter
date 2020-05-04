using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// A user's characteristics
    /// </summary>
    [JsonObject]
    public class User
    {
        [JsonProperty("UniqueID")]
        public string UnqiueId { get; set; }

        [JsonProperty("FName")]
        public string FirsName { get; set; }

        [JsonProperty("LName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("TokenID")]
        public string TokenID { get; set; }

        [JsonProperty("Weight", NullValueHandling = NullValueHandling.Ignore)]
        public int Weight { get; set; }

        [JsonProperty("Height", NullValueHandling = NullValueHandling.Ignore)]
        public int Height { get; set; }

    }
}
