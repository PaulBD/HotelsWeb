using core.extras.dtos;
using System;

namespace core.extras.services
{
    public interface IParkingService
    {
        ParkingAvailabilityResponseDto AvailabilityAtDestination(ParkingAvailabilityRequestDto request);

        //ParkingCountDto ReturnSpaceCount(string destination, DateTime arrivalDate, DateTime departDate, string initials, string userToken);

        //ParkingUpgradesDto ParkingUpgrade(string productCode, DateTime arrivalDate, DateTime departDate, int adultCount, string[] roomCodes, string userToken);

        //void BookCarPark(ParkingBookingRequestDto parkingRequestDto);
    }
}
