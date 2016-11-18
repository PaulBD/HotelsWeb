using core.hotels.dtos;

namespace core.hotels.services
{
    public interface IHotelService
    {
        HotelListDto ReturnHotelByTown(string town, string country, int limit, int offset);
        HotelListDto ReturnHotelsByProximity(double longitude, double latitude, int radius, int offset, int limit);
        HotelDetailsDto ReturnHotelById(string id);
    }
}
