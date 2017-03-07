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
