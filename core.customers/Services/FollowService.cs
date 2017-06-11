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

            customer.TriperooCustomers.Follows.Add(
                new CustomerFollowsDto{
                 CustomerReference = reference,
                DateAdded = DateTime.Now
				});

			_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
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

            var friend = customer.TriperooCustomers.Follows.FirstOrDefault(q => q.CustomerReference == reference);
            customer.TriperooCustomers.Follows.Remove(friend);

			_customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);
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
