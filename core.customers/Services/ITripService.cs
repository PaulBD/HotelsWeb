using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface ITripService
    {
        int InsertNewTrip(string token, TripDto trip);
        void EditExistingTrip(string token, int tripId, TripDto trip);
		void ArchiveExistingTrip(int tripId, string token);
        List<TripDto> ReturnTripsByCustomerReference(string customerReference);
        TripDto ReturnTripByCustomerReferenceAndId(string customerReference, int tripId);
        void InsertUpdateTrip(string customerReference, TripDto trip);
    }
}
