using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class FriendDto
    {
		public string CustomerReference { get; set; }
		public string PrefixName { get; set; }
		public string Name { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Locale { get; set; }
		public int CurrentLocationId { get; set; }
		public string CurrentLocation { get; set; }
		public string ImageUrl { get; set; }
		public string ProfileUrl { get; set; }
    }
}
