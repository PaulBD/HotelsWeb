using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;
using System;

namespace core.customers.services
{
	public class VisitedService : IVisitedService
	{
		private CouchBaseHelper _couchbaseHelper;
		private CustomerService _customerService;

		public VisitedService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
		}

		/// <summary>
		/// Archives the visited location by location identifier.
		/// </summary>
		public void ArchiveVisitByLocationId(int locationId, string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				customer.TriperooCustomers.VisitedLocations.FirstOrDefault(q => q.RegionID == locationId).IsArchived = true;
				var newCustomer = _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
			}
		}

		/// <summary>
		/// Inserts the new visited location.
		/// </summary>
		public void InsertNewVisitedLocation(string token, CustomerLocationDto location)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				var foundLike = customer.TriperooCustomers.VisitedLocations.FirstOrDefault(q => q.RegionID == location.RegionID);

				if (foundLike != null)
				{
					customer.TriperooCustomers.VisitedLocations.Remove(foundLike);
				}

				location.Id = customer.TriperooCustomers.Likes.Count + 1;
				location.DateCreated = DateTime.Now;
				customer.TriperooCustomers.VisitedLocations.Add(location);

				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
			}
		}

		/// <summary>
		/// Returns the visited locations by token.
		/// </summary>
		public List<CustomerLocationDto> ReturnVisitedLocationsByToken(string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				return customer.TriperooCustomers.VisitedLocations.Where(q => q.IsArchived == false).ToList();
			}

			return null;
		}
	}
}
