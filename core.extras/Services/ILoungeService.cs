using core.extras.dtos;
using System;

namespace core.extras.services
{
    public interface ILoungeService
    {
        LoungeAvailabilityResponseDto AvailabilityAtDestination(string locationName, string arrivalDate, string arrivalTime, string flightTime, int adults, int children, int infants, string language);  
    }
}
