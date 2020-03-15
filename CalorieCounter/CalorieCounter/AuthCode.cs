using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalorieCounter
{
    [JsonObject]
    class AuthCode
    {
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
