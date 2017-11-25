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
            _serviceKey = ConfigurationManager.AppSettings["flights.kiwi.key"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["flights.kiwi.baseUrl"])
            };
        }


        /// <summary>
        /// Return cached flights
        /// </summary>
        public FlightListDto ReturnFlights(string flyFrom, string flyTo, string dateFrom, string dateTo, string returnFrom, string returnTo, string flightType, int passengerTotal, int adultTotal, int childTotal, int infantTotal, decimal priceFrom, decimal priceTo, string departureTimeFrom, string departureTimeTo, string arrivalTimeFrom, string arrivalTimeTo, string returnDepartureTimeFrom, string returnDepartureTimeTo, string returnArrivalTimeFrom, string returnArrivalTimeTo, string stopOverFrom, string stopOverTo, string sortBy, string asc, string selectedAirlines = null, int offset = 0, int limit = 30, int directFlightsOnly = 1, string marketCountry = "gb", string currency = "gbp", string locale = "en")
        {
            var request = "flights?flyFrom=" + flyFrom + "&to=" + flyTo + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&returnFrom=" + returnFrom + "&returnTo=" + returnTo + "&typeFlight=" + flightType + "&oneforcity=0&one_per_date=0&passengers=" + passengerTotal + "&adults=" + adultTotal + "&children=" + childTotal + "&infants=" + infantTotal;
            request += "&directFlights=" + directFlightsOnly + "&partner=" + _serviceKey + "&partner_market=" + marketCountry + "&v=2&xml=0&curr=" + currency + "&locale=" + locale;
            request += "&price_from=" + priceFrom + "&price_to=" + priceTo + "&dtimefrom=" + departureTimeFrom + "&dtimeto=" + departureTimeTo + "&atimefrom=" + arrivalTimeFrom;
            request += "&atimeto=" + arrivalTimeTo + "&returndtimefrom=" + returnDepartureTimeFrom + "&returndtimeto=" + returnDepartureTimeTo + "&returnatimefrom=" + returnArrivalTimeFrom;
            request += "&returnatimeto=" + returnArrivalTimeTo + "&stopoverfrom=" + stopOverFrom + "&stopoverto=" + stopOverTo;
            request += "&selectedAirlines=" + selectedAirlines;

            if (!string.IsNullOrEmpty(selectedAirlines))
            {
                request += "&selectedAirlinesExclude=true";
            }

            request += "&offset=" + offset + "&limit=" + limit + "&sort=" + sortBy + "&asc=" + asc;

            var response = _client.GetAsync(request).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<FlightListDto>(result);
        }
    }
}
