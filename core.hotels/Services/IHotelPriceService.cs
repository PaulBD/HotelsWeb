using core.hotels.dtos;
using core.hotels.enums;

namespace core.hotels.services
{
    public interface IHotelPriceService
    {
        HotelPriceDto ReturnHotelPrice(string id, Currency currency, string startDate, int nights);
    }
}
