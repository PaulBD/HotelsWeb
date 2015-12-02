using Newtonsoft.Json;

namespace dtos.laterooms.co.uk
{
    public class Response
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
        [JsonProperty("number_of_hotels")]
        public string HotelCount { get; set; }
    }
}
