using System;
using System.Collections.Generic;

namespace core.customers.dtos
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
    public class ReviewDetail
    {
        public ReviewDetail()
        {
            Place = new PlaceDto();
            Reviewer = new ReviewerDto();
        }

        public string Type {  get { return "review"; } }
        public string Reference { get; set; }
        public string CustomerReference { get; set; }
        public string PlaceReference { get; set; }
        public string PlaceType { get; set; }
        public int StarRating { get; set; }
        public string Comment { get; set; }
        public List<TagDto> Tags { get; set; }
        public bool IsArchived { get; set; }
        public DateTime DateCreated { get; set; }
        public PlaceDto Place { get; set; }
        public ReviewerDto Reviewer { get; set; }
    }

    public class ReviewerDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ProfileUrl { get; set; }
    }

    public class PlaceDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Url { get; set; }
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
