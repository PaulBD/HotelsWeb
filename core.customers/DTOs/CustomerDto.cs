using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class ProfileDto
    {
        public string PrefixName { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Locale { get; set; }
        public int CurrentLocationId { get; set; }
        public string CurrentLocation { get; set; }
        public string EmailAddress { get; set; }
        public string Pass { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
    }

    public class TripDto
    {
        public int Id { get; set; }
        public TripDto()
        {
            Bookmarks = new List<CustomerLocationDto>();
        }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string ListName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<CustomerLocationDto> Bookmarks { get; set; }
		public DateTime DateCreated { get; set; }
		public string Url { get; set; }
        public bool IsArchived { get; set; }
    }

    public class CustomerLocationDto
    {
        public int Id { get; set; }
        public string SubClass { get; set; }
        public int RegionID { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string RegionName { get; set; }
        public string RegionNameLong { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsArchived { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            Profile = new ProfileDto();
            Trips = new List<TripDto>();
			Likes = new List<CustomerLocationDto>();
			VisitedLocations = new List<CustomerLocationDto>();
        }

        public bool IsFacebookSignup { get; set; }
        public int FacebookId { get; set; }
        public string Token { get; set; }
        public string Type { get { return "customer"; } }
        public string CustomerReference { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public ProfileDto Profile { get; set; }
		public List<TripDto> Trips { get; set; }
		public List<CustomerLocationDto> VisitedLocations { get; set; }
		public List<CustomerLocationDto> Likes { get; set; }
    }

    public class CustomerDto
    {
        public CustomerDto()
        {
            TriperooCustomers = new Customer();
        }

        public Customer TriperooCustomers { get; set; }
    }

}
