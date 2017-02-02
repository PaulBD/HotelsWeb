using core.hotels.dtos;
using System.Collections.Generic;

namespace core.hotels.services
{
    public interface IHotelService
    {
        List<HotelDto> ReturnHotelsByTown(string town, string country, int limit, int offset);
        List<HotelDto> ReturnHotelsByPlaceId(int placeId, int limit, int offset);
        List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, int radius, int offset, int limit);
        HotelDetailDto ReturnHotelById(string id);
    }
}
