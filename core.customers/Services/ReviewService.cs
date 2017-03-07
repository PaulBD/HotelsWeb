using core.customers.dtos;
using library.couchbase;

namespace core.customers.services
{
    public class ReviewService : IReviewService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooReviews";

        public ReviewService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Archive review
        /// </summary>
        public void ArchiveReviewById(string reference)
        {
            var review = ReturnReviewByReference(reference);
            
            if (review != null)
            {
                review.IsArchived = true;
            }

            InsertNewReview(reference, review);
        }

        /// <summary>
        /// Insert new review
        /// </summary>
        public void InsertNewReview(string reference, ReviewDetail review)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, review, _bucketName);
        }

        /// <summary>
        /// Return reviews by place reference
        /// </summary>
        public ReviewDto ReturnReviewsByPlaceReference(string type, string placeReference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE placeReference = '" + type + ":" + placeReference + "'";
            return ProcessQuery(q);
        }

        /// <summary>
        /// Return review by reference
        /// </summary>
        public ReviewDetail ReturnReviewByReference(string reference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE reference = '" + reference + "'";
            var response = ProcessQuery(q);

            if (response.TriperooReviews != null)
            {
                return response.TriperooReviews;
            }

            return null;
        }

        /// <summary>
        /// Return reviews by customer reference
        /// </summary>
        public ReviewDto ReturnReviewsByCustomerReference(string customerReference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE customerReference = '" + customerReference + "'";
            return ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private ReviewDto ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<ReviewDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result[0];
            }

            return null;
        }
    }
}
