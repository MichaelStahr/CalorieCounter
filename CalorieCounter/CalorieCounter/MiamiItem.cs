using Newtonsoft.Json;

namespace CalorieCounter
{
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

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", ServiceUnit, Formalname, PortionSize);
        }
    }
}