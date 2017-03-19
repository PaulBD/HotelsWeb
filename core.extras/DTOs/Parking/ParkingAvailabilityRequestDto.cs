using System;

namespace core.extras.dtos
{
    public class ParkingAvailabilityRequestDto
    {
        public ParkingAvailabilityRequestDto()
        {
            Location = new LocationDto();
            Passenger = new PassengerDto();
        }

        public LocationDto Location { get; set; }
        public PassengerDto Passenger { get; set; }

        public string UserToken { get; set; }
        public string Filter { get; set; }
        public string Fields { get; set; }
        public string Lang { get; set; }
    }

    public class LocationDto
    {
        public string LocationName { get; set; }
        public bool IsAirport { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartDate { get; set; }
        public bool IsUnitedKingdom { get; set; }
    }

    public class PassengerDto
    {
        public string Initials { get; set; }
        public int PassengerCount { get; set; }
    }
}
