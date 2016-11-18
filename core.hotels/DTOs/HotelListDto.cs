using Newtonsoft.Json;
using System.Collections.Generic;

namespace core.hotels.dtos
{
    public class HotelDetailsDto
    {
        public string AccommodationType { get; set; }
        public string Address1 { get; set; }
        public List<string> Appeals { get; set; }
        public string CancellationDays { get; set; }
        public string CancellationPolicy { get; set; }
        public string CancellationTerms { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string CityTaxOptedIn { get; set; }
        public string CityTaxType { get; set; }
        public string CityTaxValue { get; set; }
        public string CountryIso { get; set; }
        public string CurrencyCode { get; set; }
        public List<string> Facilities { get; set; }
        public string Fax { get; set; }
        public string HotelCity { get; set; }
        public string HotelCountry { get; set; }
        public string HotelCounty { get; set; }
        public string HotelCreatedDate { get; set; }
        public string HotelDescription { get; set; }
        public string HotelDirections { get; set; }
        public int HotelID { get; set; }
        public string HotelImage { get; set; }
        public List<string> HotelImages { get; set; }
        public string HotelName { get; set; }
        public string HotelPostcode { get; set; }
        public string HotelStar { get; set; }
        public string HotelStarAccreditor { get; set; }
        public string HotelUrl { get; set; }
        public string IsCityTaxArea { get; set; }
        public string LatestCheckInTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MaxPrice { get; set; }
        public int NoOfReviews { get; set; }
        public string PricesFrom { get; set; }
        public string ReviewUrl { get; set; }
        public int ScoreOutOf6 { get; set; }
        public string Telephone { get; set; }
        public int TotalRooms { get; set; }

        [JsonProperty("Wifi Available")]
        public int WifiAvailable { get; set; }
        public string type { get; set; }
    }

    public class HotelDto
    {
        [JsonProperty("Triperoo")]
        public HotelDetailsDto HotelDetails { get; set; }
    }

    public class HotelListDto
    {
        public HotelListDto()
        {
            HotelList = new List<HotelDto>();
        }

        public List<HotelDto> HotelList { get; set; }
        public int Count { get { return HotelList.Count;  } }
    }
}
