using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class ReviewDetailDto
    {
        public ReviewDetailDto()
        {
            Place = new PlaceDto();
        }

        public string Type { get { return "review"; } }
        public string ReviewReference { get; set; }
        public string CustomerReference { get; set; }
        public int InventoryReference { get; set; }
        public string ReviewType { get; set; }
        public int StarRating { get; set; }
        public string Comment { get; set; }
        public List<string> Tags { get; set; }
        public bool IsArchived { get; set; }
        public DateTime DateCreated { get; set; }
        public PlaceDto Place { get; set; }
    }

    public class PlaceDto
    {
        public string Name { get; set; }
        public string NameShort { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
    }

    public class ReviewDto
    {
        public string Comment { get; set; }
        public string CustomerImageUrl { get; set; }
        public string CustomerName { get; set; }
        public string CustomerProfileUrl { get; set; }
        public string CustomerReference { get; set; }
        public string ReviewReference { get; set; }
        public string DateCreated { get; set; }
        public string ImageUrl { get; set; }
        public int InventoryReference { get; set; }
        public bool IsArchived { get; set; }
        public string PlaceName { get; set; }
        public string PlaceNameShort { get; set; }
        public string PlaceAddress { get; set; }
        public string PlaceType { get; set; }
        public string PlaceUrl { get; set; }
        public int StarRating { get; set; }
        public List<string> Tags { get; set; }
        public string Type { get; set; }
    }
}
