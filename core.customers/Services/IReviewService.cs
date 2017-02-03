using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IReviewService
    {
        void InsertNewReview(string reference, ReviewDetail review);
        ReviewDetail ReturnReviewByReference(string reference);
        ReviewDto ReturnReviewsByCustomerReference(string customerReference);
        ReviewDto ReturnReviewsByPlaceReference(string type, string placeReference);
        void ArchiveReviewById(string reference);
    }
}
