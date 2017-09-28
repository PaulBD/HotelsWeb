using System;
using System.Collections.Generic;
using core.hotels.dtos;

namespace core.hotels.services
{
    public interface IHotelService
    {
        RoomAvailabilityDto ReturnRoomAvailability(int hotelId, string locale, string currencyCode, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3);
		HotelAPIListDto ReturnHotelsByLocationId(string locale, string currencyCode, string location, List<int> propertyCategory, float minRate, float maxRate, int minStarRating, int maxStarRating, int minTripAdvisorRating, int maxTripAdvisorRating, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3, string sortBy, int exclude);
        HotelAPIListDto ReturnHotelsByProximity(string locale, string currencyCode, double longitude, double latitude, double radius, List<int> propertyCategory, float minRate, float maxRate, int minStarRating, int maxStarRating, int minTripAdvisorRating, int maxTripAdvisorRating, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3, string sortBy, int exclude);
		HotelDto ReturnHotelById(int hotelId, string locale, string currencyCode);

    }
}
