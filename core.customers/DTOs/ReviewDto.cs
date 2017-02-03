using System.Collections.Generic;

namespace core.customers.dtos
{
    public class ReviewDetail
    {
        public string Type {  get { return "review"; } }
        public string Reference { get; set; }
        public string CustomerReference { get; set; }
        public string PlaceReference { get; set; }
        public string PlaceType { get; set; }
        public int StarRating { get; set; }
        public string Review { get; set; }
        public bool IsArchived { get; set; }
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
