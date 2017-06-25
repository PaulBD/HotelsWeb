using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;
using System;

namespace core.customers.services
{
    public class LikeService : ILikeService
	{
		private CouchBaseHelper _couchbaseHelper;
		private CustomerService _customerService;

		public LikeService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
		}

        /// <summary>
        /// Archives the like by location identifier.
        /// </summary>
        public void ArchiveLikeByLocationId(int locationId, string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				customer.TriperooCustomers.Likes.FirstOrDefault(q => q.RegionID == locationId).IsArchived = true;

				customer.TriperooCustomers.Stats.LikeCount -= 1;
				var newCustomer = _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
			}
        }

        /// <summary>
        /// Inserts the new like.
        /// </summary>
        public void InsertNewLike(string token, CustomerLocationDto location)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
                var foundLike = customer.TriperooCustomers.Likes.FirstOrDefault(q => q.RegionID == location.RegionID);

                if (foundLike != null)
                {
                    customer.TriperooCustomers.Likes.Remove(foundLike);
                }

				location.Id = customer.TriperooCustomers.Likes.Count + 1;
				location.DateCreated = DateTime.Now;
				customer.TriperooCustomers.Likes.Add(location);

                customer.TriperooCustomers.Stats.LikeCount += 1;

				_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
			}
        }

        /// <summary>
        /// Returns the likes by token.
        /// </summary>
        public List<CustomerLocationDto> ReturnLikesByToken(string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

			if (customer != null)
			{
				return customer.TriperooCustomers.Likes.Where(q => q.IsArchived == false).ToList();
			}

			return null;
        }
    }
}
