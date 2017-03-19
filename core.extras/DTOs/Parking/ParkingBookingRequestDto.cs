using System;

namespace core.extras.dtos
{
    public class ParkingBookingRequestDto
    {
        public ParkingBookingRequestDto()
        {
            Car = new CarDto();
            Flight = new FlightDto();
        }

        public CarDto Car { get; set; }
        public FlightDto Flight { get; set; }
    }

    public class CarDto
    {
        public string Registration { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Colour { get; set; }
    }

    public class FlightDto
    {
        public FlightDto()
        {
            Out = new FlightDetailDto();
            Return = new FlightDetailDto();
        }

        public FlightDetailDto Out { get; set; }
        public FlightDetailDto Return { get; set; }
    }

    public class FlightDetailDto
    {
        public string Flight { get; set; }
        public string Terminal { get; set; }
        public DateTime Date { get; set; }
    }

    public class CustomerProfileDto
    {
        public string Initials { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
}
