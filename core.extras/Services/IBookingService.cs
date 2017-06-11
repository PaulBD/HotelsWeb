using System;
namespace core.extras.Services
{
    public interface IBookingService
    {
        void RetrieveBooking(string reference, string emailAddress);
    }
}
