using core.extras.dtos;
using System;

namespace core.extras.services
{
    public interface IParkingService
    {
        ParkingAvailabilityResponseDto AvailabilityAtDestination(string locationName, string dropoffDate, string dropoffTime, string pickupDate, string pickupTime, string initials, string language, int passengerCount);  
    }
}
