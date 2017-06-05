using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface ITripService
    {
        int InsertNewTrip(string token, TripDto trip);
        void EditExistingTrip(string token, int tripId, TripDto trip);
		void ArchiveExistingTrip(int tripId, string token);
		List<TripDto> ReturnTripsByToken(string token);
		TripDto ReturnTripById(int tripId, string token);
    }
}
