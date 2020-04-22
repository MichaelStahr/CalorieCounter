using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
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

        [JsonProperty("Weight")]
        public int Weight { get; set; }

        [JsonProperty("Height")]
        public int Height { get; set; }

    }
}
