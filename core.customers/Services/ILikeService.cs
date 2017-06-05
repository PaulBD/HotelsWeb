using System;
using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface ILikeService
	{
		void InsertNewLike(string token, CustomerLocationDto location);
		void ArchiveLikeByLocationId(int locationId, string token);
		List<CustomerLocationDto> ReturnLikesByToken(string token);
    }
}
