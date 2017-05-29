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

    public class BookmarkDto
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
            Bookmarks = new List<BookmarkDto>();
        }

        public bool IsFacebookSignup { get; set; }
        public int FacebookId { get; set; }
        public string Token { get; set; }
        public string Type { get { return "customer"; } }
        public string CustomerReference { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public ProfileDto Profile { get; set; }
        public List<BookmarkDto> Bookmarks { get; set; }
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
