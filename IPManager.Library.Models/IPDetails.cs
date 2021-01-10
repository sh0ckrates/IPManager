using Newtonsoft.Json;

namespace IPManager.Library.Models
{
    public class IPDetails
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_name")]
        public string Country { get; set; }

        [JsonProperty("continent_name")]
        public string Continent { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
