using core.extras.dtos;

namespace core.extras.services
{
    public interface IAirportHotelService
    {
        AirportHotelResponseDto AvailabilityAtHotel(string locationName, string arrivateDate, string departDate, string flightDate, int nights, string roomType, string secondRoomType, int parkingDays, string language);
    }
}
