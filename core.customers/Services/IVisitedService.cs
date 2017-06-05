using System;
using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
	public interface IVisitedService
	{
		void InsertNewVisitedLocation(string token, CustomerLocationDto location);
		void ArchiveVisitByLocationId(int locationId, string token);
		List<CustomerLocationDto> ReturnVisitedLocationsByToken(string token);
	}
}
