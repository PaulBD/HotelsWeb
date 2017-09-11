using System.Collections.Generic;

namespace core.hotels.dtos
{
	public class RoomAvailabilityBedType
	{
		public string id { get; set; }
		public string description { get; set; }
	}

	public class RoomAvailabilityBedTypes
	{
		public string size { get; set; }
		public List<RoomAvailabilityBedType> bedType { get; set; }
	}

	public class RoomAvailabilityChargeableNightlyRate
	{
		public string baseRate { get; set; }
		public string rate { get; set; }
		public bool promo { get; set; }
		public bool fenced { get; set; }
	}

	public class RoomAvailabilityRoom
	{
		public int numberOfAdults { get; set; }
		public int numberOfChildren { get; set; }
		public string rateKey { get; set; }
		public List<RoomAvailabilityChargeableNightlyRate> chargeableNightlyRates { get; set; }
	}

	public class RoomAvailabilityRoomGroup
	{
		public RoomAvailabilityRoom room { get; set; }
	}

	public class RoomAvailabilityNightlyRate
	{
		public string baseRate { get; set; }
		public string rate { get; set; }
		public bool promo { get; set; }
		public bool fenced { get; set; }
	}

	public class RoomAvailabilityNightlyRatesPerRoom
	{
		public string size { get; set; }
		public List<RoomAvailabilityNightlyRate> nightlyRate { get; set; }
	}

	public class RoomAvailabilitySurcharge
	{
		public string type { get; set; }
		public string amount { get; set; }
	}

	public class RoomAvailabilitySurcharges
	{
		public string size { get; set; }
		public List<RoomAvailabilitySurcharge> surcharge { get; set; }
	}

	public class RoomAvailabilityChargeableRateInfo
	{
		public string averageBaseRate { get; set; }
		public string averageRate { get; set; }
		public string commissionableUsdTotal { get; set; }
		public string currencyCode { get; set; }
		public string maxNightlyRate { get; set; }
		public string nightlyRateTotal { get; set; }
		public string surchargeTotal { get; set; }
		public string total { get; set; }
		public RoomAvailabilityNightlyRatesPerRoom nightlyRatesPerRoom { get; set; }
		public RoomAvailabilitySurcharges surcharges { get; set; }
	}

	public class RoomAvailabilityCancelPolicyInfo
	{
		public int versionId { get; set; }
		public string cancelTime { get; set; }
		public int startWindowHours { get; set; }
		public int nightCount { get; set; }
		public string currencyCode { get; set; }
		public string timeZoneDescription { get; set; }
	}

	public class RoomAvailabilityCancelPolicyInfoList
	{
		public List<RoomAvailabilityCancelPolicyInfo> cancelPolicyInfo { get; set; }
	}

	public class RoomAvailabilityHotelFee
	{
		public string description { get; set; }
		public string amount { get; set; }
	}

	public class RoomAvailabilityHotelFees
	{
		public string size { get; set; }
		public List<RoomAvailabilityHotelFee> hotelFee { get; set; }
	}

	public class RoomAvailabilityRateInfo
	{
		public bool priceBreakdown { get; set; }
		public bool promo { get; set; }
		public bool rateChange { get; set; }
		public RoomAvailabilityRoomGroup roomGroup { get; set; }
		public RoomAvailabilityChargeableRateInfo chargeableRateInfo { get; set; }
		public string cancellationPolicy { get; set; }
		public RoomAvailabilityCancelPolicyInfoList cancelPolicyInfoList { get; set; }
		public bool nonRefundable { get; set; }
		public RoomAvailabilityHotelFees hotelFees { get; set; }
		public string rateType { get; set; }
		public int promoId { get; set; }
		public string promoDescription { get; set; }
		public string promoType { get; set; }
		public int currentAllotment { get; set; }
		public bool guaranteeRequired { get; set; }
		public bool depositRequired { get; set; }
		public double taxRate { get; set; }
	}

	public class RoomAvailabilityRateInfos
	{
		public string size { get; set; }
		public List<RoomAvailabilityRateInfo> rateInfo { get; set; }
	}

	public class RoomAvailabilityRoomImage
	{
		public string url { get; set; }
		public string highResolutionUrl { get; set; }
		public bool heroImage { get; set; }
	}

	public class RoomAvailabilityRoomImages
	{
		public string size { get; set; }
		public List<RoomAvailabilityRoomImage> roomImage { get; set; }
	}

	public class HotelRoomResponse
	{
		public int rateCode { get; set; }
		public int roomTypeCode { get; set; }
		public string rateDescription { get; set; }
		public string roomTypeDescription { get; set; }
		public string supplierType { get; set; }
		public int propertyId { get; set; }
		public RoomAvailabilityBedTypes bedTypes { get; set; }
		public string smokingPreferences { get; set; }
		public int rateOccupancyPerRoom { get; set; }
		public int quotedOccupancy { get; set; }
		public int minGuestAge { get; set; }
		public RoomAvailabilityRateInfos rateInfos { get; set; }
		public string deepLink { get; set; }
		public RoomAvailabilityRoomImages roomImages { get; set; }
	}

	public class HotelRoomAvailabilityResponse
	{
		public int size { get; set; }
		public string customerSessionId { get; set; }
		public int hotelId { get; set; }
		public string arrivalDate { get; set; }
		public string departureDate { get; set; }
		public string hotelName { get; set; }
		public string hotelAddress { get; set; }
		public string hotelCity { get; set; }
		public string hotelStateProvince { get; set; }
		public string hotelCountry { get; set; }
		public int numberOfRoomsRequested { get; set; }
		public string checkInInstructions { get; set; }
		public string specialCheckInInstructions { get; set; }
		public double tripAdvisorRating { get; set; }
		public int tripAdvisorReviewCount { get; set; }
		public string tripAdvisorRatingUrl { get; set; }
		public List<HotelRoomResponse> hotelRoomResponse { get; set; }
	}

    public class RoomAvailabilityDto
    {
        public HotelRoomAvailabilityResponse HotelRoomAvailabilityResponse { get; set; }
    }
}
