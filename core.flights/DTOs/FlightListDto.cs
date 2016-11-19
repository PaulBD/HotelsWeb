using System.Collections.Generic;

namespace core.flights.dtos
{
    public class OutboundLeg
    {
        public List<object> CarrierIds { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string DepartureDate { get; set; }
    }

    public class InboundLeg
    {
        public List<int> CarrierIds { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public string DepartureDate { get; set; }
    }

    public class Quote
    {
        public int QuoteId { get; set; }
        public double MinPrice { get; set; }
        public bool Direct { get; set; }
        public OutboundLeg OutboundLeg { get; set; }
        public InboundLeg InboundLeg { get; set; }
        public string QuoteDateTime { get; set; }
    }

    public class Place
    {
        public int PlaceId { get; set; }
        public string IataCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SkyscannerCode { get; set; }
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string CountryName { get; set; }
    }

    public class Carrier
    {
        public int CarrierId { get; set; }
        public string Name { get; set; }
    }

    public class Currency
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string ThousandsSeparator { get; set; }
        public string DecimalSeparator { get; set; }
        public bool SymbolOnLeft { get; set; }
        public bool SpaceBetweenAmountAndSymbol { get; set; }
        public int RoundingCoefficient { get; set; }
        public int DecimalDigits { get; set; }
    }

    public class FlightListDto
    {
        public List<Quote> Quotes { get; set; }
        public List<Place> Places { get; set; }
        public List<Carrier> Carriers { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Message { get; set; }
    }
}
