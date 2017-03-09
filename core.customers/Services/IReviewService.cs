using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IReviewService
    {
        void InsertNewReview(string reference, ReviewDetail review);
        ReviewDetail ReturnReviewByReference(string reference);
        List<ReviewDto> ReturnReviewsByCustomerReference(string customerReference);
        List<ReviewDto> ReturnReviewsByPlaceReference(string type, string placeReference);
        List<ReviewDto> ReturnReviewsByType(string type, int offset, int limit);
        void ArchiveReviewById(string reference);
    }
}
