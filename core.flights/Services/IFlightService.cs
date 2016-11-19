using core.flights.dtos;

namespace core.flights.services
{
    public interface IFlightService
    {
        FlightListDto ReturnCachedFlights(string from, string to, string outboundDate, string inboundDate, string marketCountry = "gb", string currency = "gbp", string locale = "en");
    }
}
