using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IReviewService
    {
        void InsertNewReview(string reference, ReviewDetailDto review);
        List<ReviewDto> ReturnReviewsByType(string type, int offset, int limit);
    }
}
