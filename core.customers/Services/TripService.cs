using core.customers.dtos;
using library.couchbase;
using System.Linq;
using System.Collections.Generic;

namespace core.customers.services
{
    public class TripService : ITripService
    {
		private CouchBaseHelper _couchbaseHelper;
		private CustomerService _customerService;

		public TripService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
		}

        public void ArchiveExistingTrip(int tripId, string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
				customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId).IsArchived = true;
				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }
        }

        public int InsertNewTrip(string token, TripDto trip)
		{
            int tripId = 0;
			var customer = _customerService.ReturnCustomerByToken(token);

            if (customer != null)
            {
                tripId = customer.TriperooCustomers.Trips.Count + 1;
                trip.Id = tripId;
                trip.Url = customer.TriperooCustomers.Profile.ProfileUrl + "/trips/" + trip.Id + "/" + trip.ListName.Replace(" ", "-");
				customer.TriperooCustomers.Trips.Add(trip);
				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
            }

            return tripId;
        }

        public void EditExistingTrip(string token, int tripId, TripDto trip)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
                var existingTrip = customer.TriperooCustomers.Trips.FirstOrDefault(q => q.Id == tripId);
                existingTrip.RegionId = trip.RegionId;
                existingTrip.RegionName = trip.RegionName;
                existingTrip.StartDate = trip.StartDate;
                existingTrip.EndDate = trip.EndDate;
				existingTrip.Description = trip.Description;
				existingTrip.ListName = trip.ListName;
				existingTrip.Url = customer.TriperooCustomers.Profile.ProfileUrl + "/trips/" + existingTrip.Id + "/" + trip.ListName.Replace(" ", "-");

				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
			}
		}

		/// <summary>
		/// Return Trips by token
		/// </summary>
		public List<TripDto> ReturnTripsByToken(string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				return customer.TriperooCustomers.Trips.Where(q => q.IsArchived == false).ToList();
			}

			return null;
		}

		/// <summary>
		/// Return Trip By Id
		/// </summary>
		public TripDto ReturnTripById(int tripId, string token)
		{
			var list = ReturnTripsByToken(token);

			if (list != null)
			{
				return list.FirstOrDefault(q => q.Id == tripId);
			}

			return null;
		}
    }
}
