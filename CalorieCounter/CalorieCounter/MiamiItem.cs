using Newtonsoft.Json;

namespace CalorieCounter
{
    /// <summary>
    /// The values accociated with food items in our database
    /// </summary>
    [JsonObject]
    public class MiamiItem
    {

        [JsonProperty("offered_id")]
        public int Offered_id { get; set; }

        [JsonProperty("serviceUnit")]
        public string ServiceUnit { get; set; }

        [JsonProperty("formalname")]
        public string Formalname { get; set; }

        [JsonProperty("portionSize")]
        public string PortionSize { get; set; }

        [JsonProperty("menuplangrp")]
        public string Menuplangrp { get; set; }

        [JsonProperty("protein")]
        public string Protein { get; set; }

        [JsonProperty("fat")]
        public string Fat { get; set; }

        [JsonProperty("chol")]
        public string Chol { get; set; }

        [JsonProperty("caloriesK")]
        public string CaloriesK { get; set; }

        [JsonProperty("calcium")]
        public string Calcium { get; set; }

        [JsonProperty("sodium")]
        public string Sodium { get; set; }

        [JsonProperty("cholesterol")]
        public string Cholesterol { get; set; }

        [JsonProperty("sugar")]
        public string Sugar { get; set; }


        public override string ToString()
        {
            return string.Format("{0} {1} {2}", ServiceUnit, Formalname, PortionSize);
        }
    }
}