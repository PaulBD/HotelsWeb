using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class ReviewDetail
    {
        public string Type { get { return "review"; } }
        public string ReviewReference { get; set; }
        public string CustomerReference { get; set; }
        public string Reference { get; set; }
        public string PlaceType { get; set; }
        public int StarRating { get; set; }
        public string Comment { get; set; }
        public List<string> Tags { get; set; }
        public bool IsArchived { get; set; }
        public DateTime DateCreated { get; set; }


        public string PlaceAddress { get; set; }
        public string PlaceCity { get; set; }
        public string PlaceCountry { get; set; }
        public string PlaceImage { get; set; }
        public string PlaceName { get; set; }
        public string PlaceUrl { get; set; }


        public string CustomerName { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileImage { get; set; }
    }

    public class ReviewDto
    {
        public ReviewDto()
        {
            TriperooReviews = new ReviewDetail();
        }

        public ReviewDetail TriperooReviews { get; set; }
    }
}
