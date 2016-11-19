using core.flights.dtos;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;

namespace core.flights.services
{
    public class FlightService : IFlightService
    {
        private readonly HttpClient _client;

        private string _serviceKey;

        public FlightService()
        {
            _serviceKey = ConfigurationManager.AppSettings["flights.skyscanner.key"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["flights.skyscanner.baseUrl"])
            };
        }


        /// <summary>
        /// Return cached flights
        /// </summary>
        public FlightListDto ReturnCachedFlights(string from, string to, string outboundDate, string inboundDate, string marketCountry = "gb", string currency = "gbp", string locale = "en")
        {
            var request = "browsequotes/v1.0/" + marketCountry + "/" + currency + "/" + locale + "/" + from + "/" + to + "/" + outboundDate;

            if (inboundDate != null)
            {
                request += "/" + inboundDate;
            }
                
            request += "?apiKey=" + _serviceKey;
            
            var response = _client.GetAsync(request).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<FlightListDto>(result);
        }
    }
}
