using System.Collections.Generic;
using System;
using core.hotels.dtos;

namespace core.hotels.services
{
    public interface IHotelService
    {
        List<HotelDto> ReturnHotelsByLocationId(int locationId);
		HotelAPIListDto ReturnHotelsByLocationId(string sessionId, string locale, string currencyCode, int locationId, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3);

		List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, double radius);
        HotelDto ReturnHotelById(int id);

    }
}
