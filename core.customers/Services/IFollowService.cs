using System;
using core.customers.dtos;
using System.Collections.Generic;
  
namespace core.customers.services
{
    public interface IFollowService
    {
		void FollowFriend(string reference, string token);
        void UnfollowFriend(string reference, string token);
		List<FollowerDto> ListFriendsFollowedByCustomer(string customerReference);
		List<FollowerDto> ListFriendsFollowingCustomer(string customerReference);
    }
}
