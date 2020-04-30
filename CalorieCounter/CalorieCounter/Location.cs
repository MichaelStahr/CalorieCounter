using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    [JsonObject]
    public class Location
    {
        [JsonProperty("serviceUnit")]
        public string ServiceUnit { get; set; }

        [JsonProperty("itemCount")]
        public string ItemCount { get; set; }
    }
}
