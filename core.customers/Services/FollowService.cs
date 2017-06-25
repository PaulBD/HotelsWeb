using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Linq;
using System;

namespace core.customers.services
{
    public class FollowService : IFollowService
	{
		private CouchBaseHelper _couchbaseHelper;
		private CustomerService _customerService;
		private readonly string _bucketName = "TriperooCustomers";

		public FollowService()
		{
			_couchbaseHelper = new CouchBaseHelper();
			_customerService = new CustomerService();
		}

        /// <summary>
        /// Follows a friend
        /// </summary>
        public void FollowFriend(string reference, string token)
        {
            var customer = _customerService.ReturnCustomerByToken(token);

            customer.TriperooCustomers.Following.Add(
                new CustomerFollowsDto{
                 CustomerReference = reference,
                DateAdded = DateTime.Now
				});

			customer.TriperooCustomers.Stats.FollowingCount += 1;
			_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

            // Update the customer I want to follow showing that I'm following them
			var followcustomer = _customerService.ReturnCustomerByReference(reference);

			followcustomer.TriperooCustomers.FollowedBy.Add(
				new CustomerFollowsDto
				{
					CustomerReference = reference,
					DateAdded = DateTime.Now
				});

			followcustomer.TriperooCustomers.Stats.FollowerCount += 1;
			_customerService.InsertUpdateCustomer(followcustomer.TriperooCustomers.CustomerReference, followcustomer.TriperooCustomers);
        }

        /// <summary>
        /// List all friends
        /// </summary>
        public List<FriendDto> ListFriends(string token)
		{
			var q = "SELECT * FROM " + _bucketName + " WHERE token = '" + token + "'";
			return ProcessQuery(q);
        }

        /// <summary>
        /// Unfollows a friend
        /// </summary>
        public void UnfollowFriend(string reference, string token)
		{
			var customer = _customerService.ReturnCustomerByToken(token);

            var friend = customer.TriperooCustomers.Following.FirstOrDefault(q => q.CustomerReference == reference);
            customer.TriperooCustomers.Following.Remove(friend);

            customer.TriperooCustomers.Stats.FollowingCount -= 1;
			_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);


			var followcustomer = _customerService.ReturnCustomerByReference(reference);

			var followFriend = followcustomer.TriperooCustomers.FollowedBy.FirstOrDefault(q => q.CustomerReference == reference);
			followcustomer.TriperooCustomers.FollowedBy.Remove(followFriend);

			followcustomer.TriperooCustomers.Stats.FollowerCount -= 1;
			_customerService.InsertUpdateCustomer(followcustomer.TriperooCustomers.CustomerReference, followcustomer.TriperooCustomers);
        }

		/// <summary>
		/// Process Query
		/// </summary>
		private List<FriendDto> ProcessQuery(string q)
		{
			var result = _couchbaseHelper.ReturnQuery<FriendDto>(q, _bucketName);

			if (result.Count > 0)
			{
				return result;
			}

			return null;
		}
    }
}
