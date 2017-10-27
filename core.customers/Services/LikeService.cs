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
        private readonly string _bucketName = "TriperooCommon";
        private string _query;

		public LikeService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
            _query = "SELECT tr.likeCount, tr.comment, tr.reviewReference, tr.reviewUrl, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.place.imageUrl, tr.place.type as placeType, tr.place.name as placeName, tr.place.nameShort as placeNameShort, tr.place.address as placeAddress, tr.place.profileUrl as placeUrl, tr.profile.type as reviewType, tr.starRating, tr.tags, tr.type, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl, tc.profile.currentLocation as customerLocation FROM " + _bucketName + " tr JOIN " + _bucketName + " tc ON KEYS tr.customerReference";
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
