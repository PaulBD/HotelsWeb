using core.customers.dtos;
using core.customers.services;
using core.hotels.services;
using core.places.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.review
{
	#region Insert Review Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/review/{reviewReference}", "DELETE")]
    [Route("/v1/review/{reviewReference}", "PUT")]
    [Route("/v1/review", "POST")]
    public class ReviewRequest
    {
        public string ReviewReference { get; set; }
        public ReviewDetailDto Review { get; set; }
    }

    #endregion

    #region Like Review Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/review/like", "PUT")]
    public class LikeReviewRequest
    {
        public string ReviewReference { get; set; }
    }

    #endregion

    #region API logic

    public class ReviewApi : Service
    {
        private readonly IVisitedService _visitedService;
        private readonly IReviewService _reviewService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewApi(IReviewService reviewService, ICustomerService customerService, ILocationService locationService, IVisitedService visitedService)
        {
            _reviewService = reviewService;
            _customerService = customerService;
            _locationService = locationService;
            _visitedService = visitedService;

        }

        #region Insert Review

        /// <summary>
        /// Insert Review
        /// </summary>
        public object Post(ReviewRequest request)
        {
            
            try
            {
                var token = Request.Headers["token"];

                if (string.IsNullOrEmpty(token))
                {
                    return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
                }

                var customer = _customerService.ReturnCustomerByToken(token);

                if (customer == null)
                {
                    return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
                }

                var location = _locationService.ReturnLocationById(request.Review.InventoryReference);

                if (location == null)
                {
                    return new HttpResult("Location not found" + token, HttpStatusCode.Unauthorized);
                }

                if (location != null)
                {
                    request.Review.Place.NameShort = location.RegionName;
                    request.Review.Place.Name = location.RegionNameLong;
                    request.Review.Place.Address = location.RegionNameLong.Replace(location.RegionName + ",", "").Trim();
                    request.Review.Place.ProfileUrl = location.Url;

                    var image = location.Image;

                    if (location.Photos != null)
                    {
                        if (location.Photos.PhotoCount > 0)
                        {
                            image = location.Photos.PhotoList[0].prefix + "400x400" + location.Photos.PhotoList[0].suffix;
                        }
                    }

                    request.Review.Place.ImageUrl = image;
                    request.Review.Place.Type = location.RegionType;
                }
                var guid = Guid.NewGuid();
                var reference = "review:" + guid;
				request.Review.ReviewReference = reference;
				request.Review.ReviewUrl = "/" + guid + "/review";
                request.Review.DateCreated = DateTime.Now;
                request.Review.CustomerReference = customer.TriperooCustomers.CustomerReference;

                _reviewService.InsertUpdateReview(reference, request.Review);

                if (location.Stats.ReviewCount > 0)
                {
                    location.Stats.ReviewCount += 1;
                    location.Stats.AverageReviewScore = (Convert.ToDouble(location.Stats.AverageReviewScore) + Convert.ToDouble(request.Review.StarRating)) / Convert.ToDouble(location.Stats.ReviewCount);
                }
				else
				{
					location.Stats.ReviewCount += 1;
                    location.Stats.AverageReviewScore = request.Review.StarRating;
                }

                _locationService.UpdateLocation(location, false);

                customer.TriperooCustomers.Stats.ReviewCount += 1;
                _customerService.InsertUpdateCustomer(customer.TriperooCustomers.CustomerReference, customer.TriperooCustomers);

                _visitedService.InsertNewVisitedLocation(token, new CustomerLocationDto(){
                    DateCreated = DateTime.Now,
                    Image = location.Image,
                    Latitude = location.LocationCoordinates.Latitude,
                    Longitude = location.LocationCoordinates.Longitude,
                    RegionID = location.RegionID,
                    RegionName = location.RegionName,
                    RegionNameLong = location.RegionNameLong,
                    SubClass = location.SubClass,
                    Url = location.Url
                });
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

			return new HttpResult(HttpStatusCode.OK);
        }

		#endregion

		#region Update Existing Review

		/// <summary>
		/// Update Existing Review
		/// </summary>
		public object Put(ReviewRequest request)
		{

			try
			{
				var token = Request.Headers["token"];

				if (string.IsNullOrEmpty(token))
				{
					return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
				}

				var customer = _customerService.ReturnCustomerByToken(token);

				if (customer == null)
				{
					return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
				}

                var review = _reviewService.ReturnReviewByReference(request.ReviewReference);

                if (review == null)
				{
					return new HttpResult("Review not found" + token, HttpStatusCode.NotFound);
                }

                review.Comment = request.Review.Comment;
                review.StarRating = request.Review.StarRating;
                review.Tags = request.Review.Tags;

                _reviewService.InsertUpdateReview(review.ReviewReference, review);

			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(HttpStatusCode.OK);
		}

		#endregion

		#region Remove Existing Review

		/// <summary>
		/// Remove Existing Review
		/// </summary>
		public object Delete(ReviewRequest request)
		{

			try
			{
				var token = Request.Headers["token"];

				if (string.IsNullOrEmpty(token))
				{
					return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
				}

				var customer = _customerService.ReturnCustomerByToken(token);

				if (customer == null)
				{
					return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
				}

                _reviewService.RemoveExistingReview(request.ReviewReference);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(HttpStatusCode.OK);
		}

		#endregion

		#region Like Review

		/// <summary>
		/// Like Review
		/// </summary>
		public object Put(LikeReviewRequest request)
        {
            try
            {
                var token = Request.Headers["token"];

                if (string.IsNullOrEmpty(token))
                {
                    return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
                }

                var customer = _customerService.ReturnCustomerByToken(token);

                if (customer == null)
                {
                    return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
                }

                _reviewService.LikeReview(request.ReviewReference);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
