using System.Collections.Generic;

namespace core.hotels.dtos
{
    public class HotelDetailDto
    {
        public int HotelId { get; set; }
        public int PlaceId { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Reference { get; set; }
        public string HotelName { get; set; }
        public string HotelStar { get; set; }
        public string Address1 { get; set; }
        public string HotelCity { get; set; }
        public string HotelCounty { get; set; }
        public string HotelPostcode { get; set; }
        public string HotelCountry { get; set; }
        public int CountryId { get; set; }
        public string CountryIso { get; set; }
        public string HotelDescription { get; set; }
        public string HotelDirections { get; set; }
        public string HotelImage { get; set; }
        public List<string> HotelImages { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string HotelUrl { get; set; }
        public string PricesFrom { get; set; }
        public string MaxPrice { get; set; }
        public string CurrencyCode { get; set; }
        public int ScoreOutOf6 { get; set; }
        public int NoOfReviews { get; set; }
        public string ReviewUrl { get; set; }
        public List<string> Facilities { get; set; }
        public string AccommodationType { get; set; }
        public List<string> Appeals { get; set; }
        public string HotelStarAccreditor { get; set; }
        public string HotelCreatedDate { get; set; }
        public int TotalRooms { get; set; }
        public string CancellationPolicy { get; set; }
        public int CancellationDays { get; set; }
        public string CancellationTerms { get; set; }
        public string CityTaxType { get; set; }
        public object CityTaxValue { get; set; }
        public string CityTaxOptedIn { get; set; }
        public bool IsCityTaxArea { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string LatestCheckInTime { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public bool WifiAvailable { get; set; }
        public string HotelArea { get; set; }
    }

    public class HotelDto
    {
        public HotelDetailDto TriperooHotels { get; set; }
    }

    public class HotelListDto
    {
        public HotelListDto()
        {
            HotelList = new List<HotelDto>();
        }

        public List<HotelDto> HotelList { get; set; }
        public int HotelCount { get; set; }
    }

}
