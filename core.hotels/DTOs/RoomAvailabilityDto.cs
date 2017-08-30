using System.Collections.Generic;

namespace core.hotels.dtos
{
    public class RoomRateBedType
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class RoomRateBedTypes
    {
        public string size { get; set; }
        public RoomRateBedType BedType { get; set; }
    }

    public class RoomRateChargeableNightlyRate
    {
        public string baseRate { get; set; }
        public string rate { get; set; }
        public string promo { get; set; }
        public string fenced { get; set; }
    }

    public class RoomRateRoom
    {
        public int numberOfAdults { get; set; }
        public int numberOfChildren { get; set; }
        public string rateKey { get; set; }
        public List<RoomRateChargeableNightlyRate> chargeableNightlyRates { get; set; }
    }

    public class RoomRateRoomGroup
    {
        public RoomRateRoom Room { get; set; }
    }

    public class RoomRateNightlyRate
    {
        public string baseRate { get; set; }
        public string rate { get; set; }
        public string promo { get; set; }
        public string fenced { get; set; }
    }

    public class RoomRateNightlyRatesPerRoom
    {
        public string size { get; set; }
        public List<RoomRateNightlyRate> nightlyRate { get; set; }
    }

    public class RoomRateSurcharge
    {
        public string type { get; set; }
        public string amount { get; set; }
    }

    public class RoomRateSurcharges
    {
        public string size { get; set; }
        public RoomRateSurcharge surcharge { get; set; }
    }

    public class RoomRateChargeableRateInfo
    {
        public string averageBaseRate { get; set; }
        public string averageRate { get; set; }
        public string commissionableUsdTotal { get; set; }
        public string currencyCode { get; set; }
        public string maxNightlyRate { get; set; }
        public string nightlyRateTotal { get; set; }
        public string surchargeTotal { get; set; }
        public string total { get; set; }
        public RoomRateNightlyRatesPerRoom NightlyRatesPerRoom { get; set; }
        public RoomRateSurcharges Surcharges { get; set; }
    }

    public class RoomRateCancelPolicyInfo
    {
        public int versionId { get; set; }
        public string cancelTime { get; set; }
        public int startWindowHours { get; set; }
        public int nightCount { get; set; }
        public string currencyCode { get; set; }
        public string timeZoneDescription { get; set; }
    }

    public class RoomRateCancelPolicyInfoList
    {
        public List<RoomRateCancelPolicyInfo> CancelPolicyInfo { get; set; }
    }

    public class RoomRateRateInfo
    {
        public string priceBreakdown { get; set; }
        public string promo { get; set; }
        public string rateChange { get; set; }
        public RoomRateRoomGroup RoomGroup { get; set; }
        public RoomRateChargeableRateInfo ChargeableRateInfo { get; set; }
        public string cancellationPolicy { get; set; }
        public RoomRateCancelPolicyInfoList CancelPolicyInfoList { get; set; }
        public bool nonRefundable { get; set; }
        public string rateType { get; set; }
        public int currentAllotment { get; set; }
        public bool guaranteeRequired { get; set; }
        public bool depositRequired { get; set; }
        public double taxRate { get; set; }
    }

    public class RoomRateRateInfos
    {
        public string size { get; set; }
        public RoomRateRateInfo RateInfo { get; set; }
    }

    public class RoomRateValueAdd
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class RoomRateValueAdds
    {
        public string size { get; set; }
        public List<RoomRateValueAdd> ValueAdd { get; set; }
    }

    public class RoomRateHotelRoomResponse
    {
        public int rateCode { get; set; }
        public int roomTypeCode { get; set; }
        public string rateDescription { get; set; }
        public string roomTypeDescription { get; set; }
        public string supplierType { get; set; }
        public int propertyId { get; set; }
        public RoomRateBedTypes BedTypes { get; set; }
        public string smokingPreferences { get; set; }
        public int rateOccupancyPerRoom { get; set; }
        public int quotedOccupancy { get; set; }
        public int minGuestAge { get; set; }
        public RoomRateRateInfos RateInfos { get; set; }
        public RoomRateValueAdds ValueAdds { get; set; }
        public string deepLink { get; set; }
    }

    public class RoomRateHotelRoomAvailabilityResponse
    {
        public string size { get; set; }
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
        public List<RoomRateHotelRoomResponse> HotelRoomResponse { get; set; }
    }

    public class RoomAvailabilityDto
    {
        public RoomRateHotelRoomAvailabilityResponse HotelRoomAvailabilityResponse { get; set; }
    }
}
