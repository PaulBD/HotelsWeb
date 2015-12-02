using System.Collections.Generic;
using Newtonsoft.Json;

namespace dtos.laterooms.co.uk
{
    public class LateRoomsDto
    {
        [JsonProperty("hotel_search")]
        public HotelList HotelList { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public class HotelList
    {
        public List<Hotel> Hotel { get; set; }
    }

    public class Rate
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("formatted_date")]
        public string FormattedDate { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("roomtype")]
        public string RoomType { get; set; }

        [JsonProperty("breakfast_included")]
        public bool IncludesBreakfast { get; set; }

        [JsonProperty("dinner_included")]
        public bool IncludesDinner { get; set; }

        [JsonProperty("cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty("cancellation_days")]
        public int? CancellationDays { get; set; }

        [JsonProperty("cancellation_hours")]
        public string CancellationHours { get; set; }

        [JsonProperty("room_terms")]
        public string RoomTerms { get; set; }

        [JsonProperty("special_offer_name")]
        public string SpecialOfferName { get; set; }

        [JsonProperty("special_offer_description")]
        public string SpecialOfferDescription { get; set; }

        [JsonProperty("breakfasts")]
        public string Breakfasts { get; set; }

        [JsonProperty("sleeps")]
        public int? Sleeps { get; set; }

        [JsonProperty("adults")]
        public int? Adults { get; set; }

        [JsonProperty("children")]
        public int? Children { get; set; }

        [JsonProperty("rooms_available")]
        public int? RoomsAvailable { get; set; }
    }

    public class HotelRooms
    {
        public List<Rate> Rate { get; set; }

    }

    public class HotelFacilities
    {
        [JsonProperty("facility")]
        public List<string> Facility { get; set; }
    }

    [JsonArray("url")]
    public class HotelImages
    {
        public List<string> Url { get; set; }
    }

    public class Tags
    {
        [JsonProperty("appeal")]
        public List<string> Tag { get; set; }
    }

    [JsonArray("credit_card")]
    public class CreditCards
    {
        public List<string> CardName { get; set; }
    }

    public class Hotel
    {
        [JsonProperty("hotel_ref")]
        public string Ref { get; set; }

        [JsonProperty("hotel_name")]
        public string Name { get; set; }

        [JsonProperty("hotel_star")]
        public string StarRating { get; set; }

        [JsonProperty("hotel_address")]
        public string Address { get; set; }

        [JsonProperty("hotel_city")]
        public string City { get; set; }

        [JsonProperty("hotel_county")]
        public string County { get; set; }

        [JsonProperty("hotel_country")]
        public string Country { get; set; }

        [JsonProperty("hotel_pcode")]
        public string Postcode { get; set; }

        [JsonProperty("hotel_max_child_age")]
        public string MaxChildAge { get; set; }

        [JsonProperty("hotel_description")]
        public string Description { get; set; }

        [JsonProperty("alternate_description")]
        public string AlternateDescription { get; set; }

        [JsonProperty("hotel_directions")]
        public string Directions { get; set; }

        [JsonProperty("alternate_directions")]
        public string AlternateDirections { get; set; }

        [JsonProperty("hotel_link")]
        public string HotelLink { get; set; }

        [JsonProperty("check_in")]
        public string CheckIn { get; set; }

        [JsonProperty("check_out")]
        public string CheckOut { get; set; }

        [JsonProperty("hotel_no_of_rooms")]
        public int? NoOfRooms { get; set; }

        [JsonProperty("geo_code")]
        public GeoCode Coordinates { get; set; }

        [JsonProperty("hotel_distance")]
        public double Distance { get; set; }

        [JsonProperty("customer_rating")]
        public int? CustomerRating { get; set; }

        [JsonProperty("prices_from")]
        public string PricesFrom { get; set; }

        [JsonProperty("star_awarded_by")]
        public string StarAwardedBy { get; set; }

        [JsonProperty("star_accomodation_type")]
        public string StarAccommodationType { get; set; }

        [JsonProperty("rack_rate")]
        public string RackRate { get; set; }

        [JsonProperty("hotel_rooms")]
        public HotelRooms Rooms { get; set; }

        [JsonProperty("cancellation_type")]
        public string CancellationType { get; set; }

        [JsonProperty("cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty("appeals")]
        public Tags Tags { get; set; }

        //[JsonProperty(propertyName: "hotel_credit_cards")]
        //public CreditCards AcceptedCreditCards { get; set; }

        //[JsonProperty(propertyName: "hotel_credit_cards_payment")]
        //public CreditCards AcceptedPaymentCreditCards { get; set; }

        [JsonProperty("facilities")]
        public HotelFacilities Facilities { get; set; }

        [JsonProperty("hotel_important_information")]
        public string ImportantInformation { get; set; }

        [JsonProperty("privacy_policy")]
        public string PrivacyPolicy { get; set; }

        [JsonProperty("terms_conditions")]
        public string TermsCondition { get; set; }

        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty("image")]
        public string MainImage { get; set; }

        [JsonProperty("images")]
        public HotelImages ImageList { get; set; }
    }

}
