using core.flights.dtos;

namespace core.flights.services
{
    public interface IFlightService
    {
        FlightListDto ReturnFlights(string flyFrom, string flyTo, string dateFrom, string dateTo, string returnFrom, string returnTo, string flightType, int passengerTotal, int adultTotal, int childTotal, int infantTotal, decimal priceFrom, decimal priceTo, string departureTimeFrom, string departureTimeTo, string arrivalTimeFrom, string arrivalTimeTo, string returnDepartureTimeFrom, string returnDepartureTimeTo, string returnArrivalTimeFrom, string returnArrivalTimeTo, string stopOverFrom, string stopOverTo, string sortBy, string asc, string selectedAirlines = null, int offset = 0, int limit = 30, int directFlightsOnly = 1, string marketCountry = "gb", string currency = "gbp", string locale = "en");
        AirportLocationDto ReturnAirports(string term, string locale);
    }
}
