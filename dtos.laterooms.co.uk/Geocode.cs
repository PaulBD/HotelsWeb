using System;
using Newtonsoft.Json;

namespace dtos.laterooms.co.uk
{
    public class GeoCode
    {
        [JsonProperty("long")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
