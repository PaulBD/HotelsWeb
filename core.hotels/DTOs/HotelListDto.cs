using System.Collections.Generic;

namespace core.hotels.dtos
{
	public class Address
	{
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string PostalCode { get; set; }
		public string StateProvince { get; set; }
	}

	public class Text
	{
		public string En { get; set; }
	}

	public class Chain
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class LocationCoordinates
	{
		public double latitude { get; set; }
		public double longitude { get; set; }
	}

	public class HotelDto
	{
		public Address address { get; set; }
		public string airportCode { get; set; }
		public Text amenitiesDescription { get; set; }
		public Chain chain { get; set; }
		public long? chainCodeID { get; set; }
		public string checkInTime { get; set; }
		public string checkOutTime { get; set; }
		public long confidence { get; set; }
		public Text description { get; set; }
		public Text diningDescription { get; set; }
		public string doctype { get; set; }
		public long eanHotelID { get; set; }
		public double highRate { get; set; }
		public string lastUpdatedDate { get; set; }
		public string location { get; set; }
		public LocationCoordinates locationCoordinates { get; set; }
		public Text locationDescription { get; set; }
		public Text locationSummary { get; set; }
		public double lowRate { get; set; }
		public Text mandatoryFeesDescription { get; set; }
		public string name { get; set; }
		public Text nationalRatingsDescription { get; set; }
		public Text policyDescription { get; set; }
		public int propertyCategory { get; set; }
		public string propertyCurrency { get; set; }
		public Text propertyFeesDescription { get; set; }
		public Text recreationDescription { get; set; }
		public long regionID { get; set; }
		public Text renovationDescription { get; set; }
		public Text roomDescription { get; set; }
		public int sequenceNumber { get; set; }
		public Text spaDescription { get; set; }
		public decimal starRating { get; set; }
		public string supplierType { get; set; }
		public Text whatToExpect { get; set; }
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
