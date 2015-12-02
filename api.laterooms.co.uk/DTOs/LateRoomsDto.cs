using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace laterooms.api.triperoo.co.uk.DTOs
{
    public class LateRoomsDto
    {
        [JsonProperty(propertyName: "hotel_search")]
        public HotelList HotelList { get; set; }
    }

    public class HotelList
    {
        public List<Hotel> Hotel { get; set; }
    }

    public class GeoCode
    {
        [JsonProperty(propertyName: "long")]
        public double lon { get; set; }

        [JsonProperty(propertyName: "lat")]
        public double lat { get; set; }
    }

    public class Rate
    {
        [JsonProperty(propertyName: "date")]
        public string Date { get; set; }

        [JsonProperty(propertyName: "formatted_date")]
        public string FormattedDate { get; set; }

        [JsonProperty(propertyName: "price")]
        public string Price { get; set; }

        [JsonProperty(propertyName: "roomtype")]
        public string RoomType { get; set; }

        [JsonProperty(propertyName: "breakfast_included")]
        public bool IncludesBreakfast { get; set; }

        [JsonProperty(propertyName: "dinner_included")]
        public bool IncludesDinner { get; set; }

        [JsonProperty(propertyName: "cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty(propertyName: "cancellation_days")]
        public int? CancellationDays { get; set; }

        [JsonProperty(propertyName: "cancellation_hours")]
        public string CancellationHours { get; set; }

        [JsonProperty(propertyName: "room_terms")]
        public string RoomTerms { get; set; }

        [JsonProperty(propertyName: "special_offer_name")]
        public string SpecialOfferName { get; set; }

        [JsonProperty(propertyName: "special_offer_description")]
        public string SpecialOfferDescription { get; set; }

        [JsonProperty(propertyName: "breakfasts")]
        public string Breakfasts { get; set; }

        [JsonProperty(propertyName: "sleeps")]
        public int? Sleeps { get; set; }

        [JsonProperty(propertyName: "adults")]
        public int? Adults { get; set; }

        [JsonProperty(propertyName: "children")]
        public int? Children { get; set; }

        [JsonProperty(propertyName: "rooms_available")]
        public int? RoomsAvailable { get; set; }
    }

    public class HotelRooms
    {
        public List<Rate> Rate { get; set; }
    }

    public class HotelFacilities
    {
        public List<string> Facility { get; set; }
    }
    public class HotelImages
    {
        public List<string> Url { get; set; }
    }

    public class Tags
    {
        [JsonProperty(propertyName: "appeal")]
        public List<string> Tag { get; set; }
    }

    public class CreditCards
    {
        [JsonProperty(propertyName: "credit_card")]
        public List<string> CardName { get; set; }
    }

    public class Hotel
    {
        [JsonProperty(propertyName: "hotel_ref")]
        public string Ref { get; set; }

        [JsonProperty(propertyName: "hotel_name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "hotel_star")]
        public string StarRating { get; set; }

        [JsonProperty(propertyName: "hotel_address")]
        public string Address { get; set; }

        [JsonProperty(propertyName: "hotel_city")]
        public string City { get; set; }

        [JsonProperty(propertyName: "hotel_county")]
        public string County { get; set; }

        [JsonProperty(propertyName: "hotel_pcode")]
        public string Postcode { get; set; }

        [JsonProperty(propertyName: "hotel_max_child_age")]
        public string MaxChildAge { get; set; }

        [JsonProperty(propertyName: "hotel_description")]
        public string Description { get; set; }

        [JsonProperty(propertyName: "hotel_directions")]
        public string Directions { get; set; }

        [JsonProperty(propertyName: "hotel_link")]
        public string HotelLink { get; set; }

        [JsonProperty(propertyName: "check_in")]
        public string CheckIn { get; set; }

        [JsonProperty(propertyName: "check_out")]
        public string CheckOut { get; set; }

        [JsonProperty(propertyName: "hotel_no_of_rooms")]
        public int? NoOfRooms { get; set; }

        [JsonProperty(propertyName: "geo_code")]
        public GeoCode Coordinates { get; set; }

        [JsonProperty(propertyName: "hotel_distance")]
        public double Distance { get; set; }

        [JsonProperty(propertyName: "customer_rating")]
        public int? CustomerRating { get; set; }

        [JsonProperty(propertyName: "prices_from")]
        public string PricesFrom { get; set; }

        [JsonProperty(propertyName: "star_awarded_by")]
        public string StarAwardedBy { get; set; }

        [JsonProperty(propertyName: "star_accomodation_type")]
        public string StarAccommodationType { get; set; }

        [JsonProperty(propertyName: "rack_rate")]
        public string RackRate { get; set; }

        [JsonProperty(propertyName: "hotel_rooms")]
        public HotelRooms Rooms { get; set; }

        [JsonProperty(propertyName: "cancellation_type")]
        public string CancellationType { get; set; }

        [JsonProperty(propertyName: "cancellation_policy")]
        public string CancellationPolicy { get; set; }

        [JsonProperty(propertyName: "hotel_appeals")]
        public Tags Tags { get; set; }

        //[JsonProperty(propertyName: "hotel_credit_cards")]
        //public CreditCards AcceptedCreditCards { get; set; }

        //[JsonProperty(propertyName: "hotel_credit_cards_payment")]
        //public CreditCards AcceptedPaymentCreditCards { get; set; }

        [JsonProperty(propertyName: "hotel_facilities")]
        public HotelFacilities Facilities { get; set; }

        [JsonProperty(propertyName: "hotel_important_information")]
        public string ImportantInformation { get; set; }

        [JsonProperty(propertyName: "privacy_policy")]
        public string PrivacyPolicy { get; set; }

        [JsonProperty(propertyName: "terms_conditions")]
        public string TermsCondition { get; set; }

        [JsonProperty(propertyName: "disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty(propertyName: "image")]
        public string MainImage { get; set; }

        [JsonProperty(propertyName: "images")]
        public HotelImages ImageList { get; set; }
    }

}
