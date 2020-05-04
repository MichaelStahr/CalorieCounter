using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace CalorieCounter
{
    /// <summary>
    /// An item in the food list at the bottom of the Home page
    /// </summary>
    [JsonObject]
    public class SimpleFood
    {
        [JsonProperty("formalName")]
        public string Name { get; set; }

        [JsonProperty("caloriesk")]
        public string Calories { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Calories);
        }
    }
}
