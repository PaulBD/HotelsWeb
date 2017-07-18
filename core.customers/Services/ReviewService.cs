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
            _query = "SELECT tr.likeCount, tr.comment, tr.reviewReference, tr.reviewUrl, tr.customerReference, tr.dateCreated, tr.inventoryReference, tr.isArchived, tr.place.imageUrl, tr.place.type as placeType, tr.place.name as placeName, tr.place.nameShort as placeNameShort, tr.place.address as placeAddress, tr.place.profileUrl as placeUrl, tr.profile.type as reviewType, tr.starRating, tr.tags, tr.type, tc.profile.name as customerName, tc.profile.imageUrl as customerImageUrl, tc.profile.profileUrl as customerProfileUrl, tc.profile.currentLocation as customerLocation FROM " + _bucketName + " tr JOIN " + _bucketName + " tc ON KEYS tr.customerReference";
        }

        /// <summary>
        /// Insert / Update review
        /// </summary>
        public void InsertUpdateReview(string reference, ReviewDetailDto review)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, review, _bucketName);
        }

        /// <summary>
        /// Remove existing review
        /// </summary>
        public void RemoveExistingReview(string reference)
        {
            var q = "DELETE FROM " + _bucketName + " WHERE reviewReference = 'review:" + reference + "'";

            ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<ReviewDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<ReviewDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return new List<ReviewDto>();
        }

        /// <summary>
        /// Returns the review by reference.
        /// </summary>
        public ReviewDetailDto ReturnReviewByReference(string reference)
        {
            var q = "SELECT comment, customerReference, dateCreated, inventoryReference, isARchived, likeCount, place, reviewReference, reviewType, starRating, tags, type FROM " + _bucketName + " WHERE reviewReference = 'review:" + reference + "'";
			return _couchbaseHelper.ReturnQuery<ReviewDetailDto>(q, _bucketName)[0];
        }

        /// <summary>
        /// Returns the type of the reviews by.
        /// </summary>
        public List<ReviewDto> ReturnReviewsByType(string type)
        {
            var q = _query;

            if (type.ToLower() != "all")
            {
                q += " WHERE tr.placeType = '" + type + "' AND tr.type = 'review' ORDER BY tr.dateCreated DESC";
            }
            else {
                q += " WHERE tr.type = 'review' ORDER BY tr.dateCreated DESC";
            }

            return ProcessQuery(q);
        }

        /// <summary>
        /// Returns the reviews by location identifier.
        /// </summary>
        public List<ReviewDto> ReturnReviewsByLocationId(int id)
        {
            var q = _query + " WHERE tr.type = 'review' AND tr.inventoryReference = " + id + " ORDER BY tr.dateCreated DESC";

            return ProcessQuery(q);
		}

        /// <summary>
        /// Return Reviews By Customer
        /// </summary>
		public List<ReviewDto> ReturnReviewsByCustomer(string customerReference)
		{
			if (!customerReference.Contains("customer:"))
			{
				customerReference = "customer:" + customerReference;
			}

			var q = _query + " WHERE tr.type = 'review' AND tr.customerReference = '" + customerReference + "' ORDER BY tr.dateCreated DESC";

			return ProcessQuery(q);
		}

        /// <summary>
        /// Likes the review.
        /// </summary>
        public void LikeReview(string reviewReference)
        {
            var q = "UPDATE " + _bucketName + " SET likeCount = likeCount + 1 WHERE reviewReference = '" + reviewReference + "'";
            
            ProcessQuery(q);
        }
    }
}
