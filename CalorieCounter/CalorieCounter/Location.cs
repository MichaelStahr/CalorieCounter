using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    /// <summary>
    /// Location name and number of items in that location to determine if it is open or not
    /// </summary>
    [JsonObject]
    public class Location
    {
        [JsonProperty("serviceUnit")]
        public string ServiceUnit { get; set; }

        [JsonProperty("itemCount")]
        public string ItemCount { get; set; }
    }
}
