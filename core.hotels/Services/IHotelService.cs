using core.hotels.dtos;
using System.Collections.Generic;

namespace core.hotels.services
{
    public interface IHotelService
    {
        List<HotelDto> ReturnHotelsByPlaceId(int placeId);
        List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, double radius);
        HotelDetailDto ReturnHotelById(string id);
    }
}
