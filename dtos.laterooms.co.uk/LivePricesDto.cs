using System.Collections.Generic;
using Newtonsoft.Json;

namespace dtos.laterooms.co.uk
{
    public class RoomRate
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("formatted_date")]
        public string FormattedDate { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("hotelcurrencyprice")]
        public string HotelCurrencyPrice { get; set; }

        [JsonProperty("numeric_price")]
        public string NumericPrice { get; set; }

        [JsonProperty("numeric_hotelcurrencyprice")]
        public string NumericHotelCurrencyPrice { get; set; }

        [JsonProperty("requested_currency")]
        public string RequestedCurrency { get; set; }
    }

    public class Room
    {
        [JsonProperty("ref")]
        public string RoomRef { get; set; }

        [JsonProperty("type")]
        public string RoomType { get; set; }

        [JsonProperty("type_description")]
        public string TypeDescription { get; set; }

        [JsonProperty("sleeps")]
        public int? Sleeps { get; set; }

        [JsonProperty("rooms_available")]
        public int? RoomsAvailable { get; set; }

        [JsonProperty("adults")]
        public int? Adults { get; set; }

        [JsonProperty("children")]
        public int? Children { get; set; }

        [JsonProperty("breakfast")]
        public bool IncludesBreakfast { get; set; }

        [JsonProperty("dinner")]
        public bool IncludesDinner { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("alternate_description")]
        public object AlternateDescription { get; set; }

        [JsonProperty("rack_rate")]
        public string RackRate { get; set; }

        [JsonProperty("rate")]
        public IList<RoomRate> Rate { get; set; }

        [JsonProperty("available_online")]
        public bool AvailableOnline { get; set; }

        [JsonProperty("minimum_nights")]
        public int? MinimumNights { get; set; }

        [JsonProperty("bed_type")]
        public string BedType { get; set; }

        [JsonProperty("cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty("cancellation_days")]
        public string CancellationDays { get; set; }

        [JsonProperty("cancellation_hours")]
        public string CancellationHours { get; set; }

        [JsonProperty("room_terms")]
        public string RoomTerms { get; set; }

        [JsonProperty("special_offer_name")]
        public string SpecialOfferName { get; set; }

        [JsonProperty("special_offer_description")]
        public string SpecialOfferDescription { get; set; }
    }

    public class Rooms
    {
        [JsonProperty("room")]
        public IList<Room> Room { get; set; }
    }

    public class HotelBasic
    {
        [JsonProperty("hotel_ref")]
        public string HotelRef { get; set; }

        [JsonProperty("hotel_currency")]
        public string HotelCurrency { get; set; }

        [JsonProperty("hotel_rooms")]
        public Rooms Rooms { get; set; }

        [JsonProperty("cancellation_type")]
        public string CancellationType { get; set; }

        [JsonProperty("cancellation_policy")]
        public string CancellationPolicy { get; set; }
    }

    public class LrRates
    {
        [JsonProperty("hotel")]
        public HotelBasic Hotel { get; set; }
    }

    public class LivePricesDto
    {
        [JsonProperty("search")]
        public Search Results { get; set; }
    }

    public class Search
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
        
        [JsonProperty("lr_rates")]
        public LrRates Rates { get; set; }
    }
}
