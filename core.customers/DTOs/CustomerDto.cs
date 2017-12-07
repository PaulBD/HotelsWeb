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
		public string BackgroundImageUrl
        {
            get
            {
                return "/static/img/locations/" + CurrentLocation.Replace(" ", "-").Replace(",", "").ToLower() + ".png";
            }
        }
		public string ProfileUrl { get; set; }
		public string Bio { get; set; }
    }

    public class CustomerTripDto
    {
        public int Id { get; set; }
        public string Type {get { return "Trip"; } }
		public DateTime DateCreated { get; set; }
		public string Url { get; set; }
        public bool IsArchived { get; set; }
    }

    public class CustomerLocationDto
    {
        public int Id { get; set; }
        public string SubClass { get; set; }
        public Int64 RegionID { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string RegionName { get; set; }
        public string RegionNameLong { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsCity { get; set; }
        public bool IsArchived { get; set; }
    }

    public class StatsDto
    {
		public int FollowerCount { get; set; }
		public int FollowingCount { get; set; }
		public int ReviewCount { get; set; }
		public int LikeCount { get; set; }
    }

    public class CustomerFollowsDto
    {
        public string CustomerReference { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class PhotoList
    {
        public string url { get; set; }
        public string photoReference { get; set; }
        public int regionID { get; set; }
        public int likeCount { get; set; }
        public string[] likedBy { get; set; }
    }

    public class CustomerPhotos
    {
        public CustomerPhotos()
        {
            PhotoList = new List<PhotoList>();
        }
        public int PhotoCount { get { if (PhotoList != null) { return PhotoList.Count; } else { return 0; } } }
        public List<PhotoList> PhotoList { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            Profile = new ProfileDto();
            Trips = new List<CustomerTripDto>();
			Likes = new List<CustomerLocationDto>();
			VisitedLocations = new List<CustomerLocationDto>();
			Following = new List<CustomerFollowsDto>();
			FollowedBy = new List<CustomerFollowsDto>();
			Stats = new StatsDto();
            CustomerPhotos = new CustomerPhotos();
        }

        public bool OptIn { get; set; }
        public bool IsFacebookSignup { get; set; }
        public int FacebookId { get; set; }
        public string AccessToken { get; set; }
        public string Token { get; set; }
        public string Type { get { return "customer"; } }
        public string CustomerReference { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime LastLoginDate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public ProfileDto Profile { get; set; }
        public List<CustomerTripDto> Trips { get; set; }
		public List<CustomerLocationDto> VisitedLocations { get; set; }
		public List<CustomerLocationDto> Likes { get; set; }
		public List<CustomerFollowsDto> Following { get; set; }
		public List<CustomerFollowsDto> FollowedBy { get; set; }
		public StatsDto Stats { get; set; }
        public CustomerPhotos CustomerPhotos { get; set; }
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
