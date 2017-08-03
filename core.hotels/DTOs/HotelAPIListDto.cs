using System;
using System.Collections.Generic;

namespace core.hotels.dtos
{
public class CachedSupplierResponse
	{
		public string supplierCacheTolerance { get; set; }
		public string cachedTime { get; set; }
		public string candidatePreptime { get; set; }
		public string otherOverheadTime { get; set; }
		public string tpidUsed { get; set; }
		public string matchedCurrency { get; set; }
		public string matchedLocale { get; set; }
	}

	public class ChargeableNightlyRate
	{
		public string baseRate { get; set; }
		public string rate { get; set; }
		public string promo { get; set; }
		public string fenced { get; set; }
	}

	public class Room
	{
		public int numberOfAdults { get; set; }
		public int numberOfChildren { get; set; }
		public string rateKey { get; set; }
		public List<ChargeableNightlyRate> ChargeableNightlyRates { get; set; }
	}

	public class RoomGroup
	{
		public Room Room { get; set; }
	}

	public class NightlyRate
	{
		public string baseRate { get; set; }
		public string rate { get; set; }
		public string promo { get; set; }
		public string fenced { get; set; }
	}

	public class NightlyRatesPerRoom
	{
		public string size { get; set; }
		public List<NightlyRate> NightlyRate { get; set; }
	}

	public class Surcharge
	{
		public string type { get; set; }
		public string amount { get; set; }
	}

	public class Surcharges
	{
		public string size { get; set; }
		public Surcharge Surcharge { get; set; }
	}

	public class ChargeableRateInfo
	{
		public string averageBaseRate { get; set; }
		public string averageRate { get; set; }
		public string commissionableUsdTotal { get; set; }
		public string currencyCode { get; set; }
		public string maxNightlyRate { get; set; }
		public string nightlyRateTotal { get; set; }
		public string surchargeTotal { get; set; }
		public string total { get; set; }
		public NightlyRatesPerRoom NightlyRatesPerRoom { get; set; }
		public Surcharges Surcharges { get; set; }
	}

	public class HotelFee
	{
		public string description { get; set; }
		public string amount { get; set; }
	}

	public class HotelFees
	{
		public string size { get; set; }
		public HotelFee HotelFee { get; set; }
	}

	public class RateInfo
	{
		public string priceBreakdown { get; set; }
		public string promo { get; set; }
		public string rateChange { get; set; }
		public RoomGroup RoomGroup { get; set; }
		public ChargeableRateInfo ChargeableRateInfo { get; set; }
		public bool nonRefundable { get; set; }
		public string rateType { get; set; }
		public object currentAllotment { get; set; }
		public HotelFees HotelFees { get; set; }
		public int? promoId { get; set; }
		public string promoDescription { get; set; }
		public string promoType { get; set; }
	}

	public class RateInfos
	{
		public string size { get; set; }
		public RateInfo RateInfo { get; set; }
	}

	public class ValueAdds
	{
		public string size { get; set; }
		public object ValueAdd { get; set; }
	}

	public class RoomRateDetails
	{
		public int roomTypeCode { get; set; }
		public int rateCode { get; set; }
		public int maxRoomOccupancy { get; set; }
		public int quotedRoomOccupancy { get; set; }
		public int minGuestAge { get; set; }
		public string roomDescription { get; set; }
		public bool propertyAvailable { get; set; }
		public bool propertyRestricted { get; set; }
		public int expediaPropertyId { get; set; }
		public RateInfos RateInfos { get; set; }
		public ValueAdds ValueAdds { get; set; }
	}

	public class RoomRateDetailsList
	{
		public RoomRateDetails RoomRateDetails { get; set; }
	}

	public class HotelSummary
	{
		public string order { get; set; }
		public string ubsScore { get; set; }
		public int hotelId { get; set; }
		public string name { get; set; }
		public string address1 { get; set; }
		public string city { get; set; }
		public string stateProvinceCode { get; set; }
		public object postalCode { get; set; }
		public string countryCode { get; set; }
		public string airportCode { get; set; }
		public string supplierType { get; set; }
		public int propertyCategory { get; set; }
		public double hotelRating { get; set; }
		public string hotelRatingDisplay { get; set; }
		public int confidenceRating { get; set; }
		public int amenityMask { get; set; }
		public double tripAdvisorRating { get; set; }
		public int tripAdvisorReviewCount { get; set; }
		public string tripAdvisorRatingUrl { get; set; }
		public string locationDescription { get; set; }
		public string shortDescription { get; set; }
		public double highRate { get; set; }
		public double lowRate { get; set; }
		public string rateCurrencyCode { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public double proximityDistance { get; set; }
		public string proximityUnit { get; set; }
		public bool hotelInDestination { get; set; }
		public string thumbNailUrl { get; set; }
		public string deepLink { get; set; }
		public RoomRateDetailsList RoomRateDetailsList { get; set; }
	}

	public class HotelList
	{
		public int size { get; set; }
		public int activePropertyCount { get; set; }
		public List<HotelSummary> HotelSummary { get; set; }
	}

	public class HotelListResponse
	{
		public string customerSessionId { get; set; }
		public int numberOfRoomsRequested { get; set; }
		public bool moreResultsAvailable { get; set; }
		public string cacheKey { get; set; }
		public string cacheLocation { get; set; }
		public CachedSupplierResponse cachedSupplierResponse { get; set; }
		public HotelList HotelList { get; set; }
	}

	public class HotelAPIListDto
	{
		public int HotelCount { get; set; }
		public HotelListResponse HotelListResponse { get; set; }
	}
}
