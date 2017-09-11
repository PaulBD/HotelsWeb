using System;
using core.hotels.dtos;

namespace core.hotels.services
{
    public interface IHotelService
    {
        RoomAvailabilityDto ReturnRoomAvailability(int hotelId, string locale, string currencyCode, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3);
		HotelAPIListDto ReturnHotelsByLocationId(string locale, string currencyCode, string city, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3);
        HotelAPIListDto ReturnHotelsByProximity(string locale, string currencyCode, double longitude, double latitude, double radius, int propertyCategory, float minRate, float maxRate, float minStarRating, float maxStarRating, int numberOfBedRooms, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3);
		HotelDto ReturnHotelById(int hotelId, string locale, string currencyCode);

    }
}
