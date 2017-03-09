using System;
using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;

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
        public List<ReviewDto> ReturnReviewsByPlaceReference(string type, string placeReference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE placeReference = '" + type + ":" + placeReference + "'";
            return ProcessQuery(q, 0, 0);
        }

        /// <summary>
        /// Return review by reference
        /// </summary>
        public ReviewDetail ReturnReviewByReference(string reference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE reference = '" + reference + "'";
            var response = ProcessQuery(q, 0, 0);

            if (response.Count > 0)
            {
                if (response[0].TriperooReviews != null)
                {
                    return response[0].TriperooReviews;
                }
            }

            return null;
        }

        /// <summary>
        /// Return reviews by customer reference
        /// </summary>
        public List<ReviewDto> ReturnReviewsByCustomerReference(string customerReference)
        {
            var q = "SELECT * FROM " + _bucketName + " WHERE customerReference = '" + customerReference + "'";
            return ProcessQuery(q, 0, 0);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<ReviewDto> ProcessQuery(string q, int limit, int offset)
        {
            if (limit > 0)
            {
                q += " LIMIT " + limit + " OFFSET " + offset;
            }

            var result = _couchbaseHelper.ReturnQuery<ReviewDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        public List<ReviewDto> ReturnReviewsByType(string type, int offset, int limit)
        {
            //var q = "SELECT tr.*, tc.profile.name as userName, tc.profile.imageUrl as ProfileImage, tc.profile.profileUrl, th.HotelName, th.Url as HotelUrl, th.HotelImage, th.Address1, th.HotelCity, th.HotelCountry FROM " + _bucketName + " AS tr INNER JOIN TriperooCustomers AS tc ON KEYS tr.customerReference INNER JOIN TriperooHotels AS th ON KEYS tr.reference";
            var q = "SELECT * FROM TriperooReviews";

            if (type.ToLower() != "all")
            {
                q += " WHERE placeType = '" + type + "' ORDER BY dateCreated DESC";
            }

            return ProcessQuery(q, limit, offset);
        }
    }
}
