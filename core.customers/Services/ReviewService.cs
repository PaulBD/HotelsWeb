using System;
using core.customers.dtos;
using library.couchbase;
using System.Collections.Generic;

namespace core.customers.services
{
    public class ReviewService : IReviewService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCustomers";
        private string _query;

        public ReviewService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT tr.likeCount, tr.comment, tr.reviewReference, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.place.imageUrl, tr.place.name as placeName, tr.place.nameShort as placeNameShort, tr.place.address as placeAddress, tr.place.profileUrl as placeUrl, tr.profile.type as reviewType, tr.placeType, tr.starRating, tr.tags, tr.type, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl, tc.profile.currentCity as customerLocation FROM " + _bucketName + " tr JOIN " + _bucketName + " tc ON KEYS tr.customerReference";
        }

        /// <summary>
        /// Insert new review
        /// </summary>
        public void InsertNewReview(string reference, ReviewDetailDto review)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, review, _bucketName);
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

        public List<ReviewDto> ReturnReviewsByType(string type, int limit, int offset)
        {
            var q = _query;

            if (type.ToLower() != "all")
            {
                q += " WHERE tr.placeType = '" + type + "' AND tr.type = 'review' ORDER BY tr.dateCreated DESC";
            }
            else {
                q += " WHERE tr.type = 'review' ORDER BY tr.dateCreated DESC";
            }

            return ProcessQuery(q, limit, offset);
        }

        public List<ReviewDto> ReturnReviewsByLocationId(int id, int limit, int offset)
        {
            var q = _query + " WHERE tr.type = 'review' AND tr.inventoryReference = " + id + " ORDER BY tr.dateCreated DESC";

            return ProcessQuery(q, limit, offset);
        }

        public void LikeReview(string reviewReference)
        {
            var q = "UPDATE " + _bucketName + " SET likeCount = likeCount + 1 WHERE reviewReference = '" + reviewReference + "'";
            
            ProcessQuery(q, 0, 0);
        }
    }
}
