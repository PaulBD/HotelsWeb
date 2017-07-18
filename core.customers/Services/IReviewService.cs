using core.customers.dtos;
using System.Collections.Generic;

namespace core.customers.services
{
    public interface IReviewService
    {
        void InsertUpdateReview(string reference, ReviewDetailDto review);
        void RemoveExistingReview(string reference);
        List<ReviewDto> ReturnReviewsByType(string type);
		List<ReviewDto> ReturnReviewsByLocationId(int id);
        ReviewDetailDto ReturnReviewByReference(string reference);
        void LikeReview(string reviewReference);
        List<ReviewDto> ReturnReviewsByCustomer(string customerReference);
    }
}
