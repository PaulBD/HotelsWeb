using core.extras.dtos;
using System;

namespace core.extras.services
{
    public interface IParkingService
    {
        ParkingDto AvailabilityAtDestination(string destination, bool isAirport, DateTime arrivalDate, DateTime departDate, bool isUnitedKingdom, string userToken, string initials, int passengerCount, string filter, string fields, string lang);

        ParkingCountDto CheckSpaceCount(string destination, DateTime arrivalDate, DateTime departDate, string initials, string userToken);

        ParkingUpgradesDto ParkingUpgrade(string productCode, DateTime arrivalDate, DateTime departDate, int adultCount, string[] roomCodes, string userToken);
    }
}
