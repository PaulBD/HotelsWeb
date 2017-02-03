using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class ProfileDto
    {
        public string PrefixName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Locale { get; set; }
        public int CurrentCityId { get; set; }
        public string EmailAddress { get; set; }
        public string Pass { get; set; }
    }

    public class FavouriteDto
    {
        public int Id { get; set; }
        public string PlaceType { get; set; }
        public int PlaceId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsArchived { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            Profile = new ProfileDto();
            Favourites = new List<FavouriteDto>();
        }

        public string Token { get; set; }
        public string Type { get { return "customer"; } }
        public string Reference { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public ProfileDto Profile { get; set; }
        public List<FavouriteDto> Favourites { get; set; }
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
