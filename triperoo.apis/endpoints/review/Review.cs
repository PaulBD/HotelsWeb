using core.customers.dtos;
using core.customers.services;
using core.hotels.services;
using core.places.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Review Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/review/{Guid}", "GET")]
    [Route("/v1/review/{Guid}", "DELETE")]
    [Route("/v1/review/{Guid}", "PUT")]
    [Route("/v1/review", "POST")]
    public class ReviewRequest
    {
        public string Guid { get; set; }
        public ReviewDetail Review { get; set; }
    }

    #endregion

    #region API logic

    public class ReviewApi : Service
    {
        private readonly IReviewService _reviewService;
        private readonly ICustomerService _customerService;
        private readonly IHotelService _hotelService;
        private readonly IPlaceService _placeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewApi(IReviewService reviewService, ICustomerService customerService, IHotelService hotelService, IPlaceService placeService)
        {
            _reviewService = reviewService;
            _customerService = customerService;
            _hotelService = hotelService;
            _placeService = placeService;

        }

        #region Return Review By Id

        /// <summary>
        /// Return Review
        /// </summary>
        public object Get(ReviewRequest request)
        {
            var response = new ReviewDetail();

            try
            {
                response = _reviewService.ReturnReviewByReference("review:" + request.Guid);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

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

                var reference = "review:" + Guid.NewGuid();
                request.Review.Reference = reference;
                request.Review.DateCreated = DateTime.Now;

                // Add the reviewer Details
                request.Review.Reviewer.Name = customer.TriperooCustomers.Profile.Name;
                request.Review.Reviewer.ImageUrl = customer.TriperooCustomers.Profile.ImageUrl;
                request.Review.Reviewer.ProfileUrl = customer.TriperooCustomers.Profile.ProfileUrl;

                // Add the location details
                switch (request.Review.PlaceType)
                {
                    case "city":
                    case "country":
                        var place = _placeService.ReturnPlaceById(request.Review.PlaceReference, request.Review.PlaceType);
                        break;
                    case "hotel":
                        var hotel = _hotelService.ReturnHotelById(request.Review.PlaceReference);
                        request.Review.Place.Address = hotel.Address1;
                        request.Review.Place.City = hotel.HotelCity;
                        request.Review.Place.Country = hotel.HotelCountry;
                        request.Review.Place.ImageUrl = hotel.HotelImage;
                        request.Review.Place.Url = hotel.HotelUrl;
                        request.Review.Place.Type = "hotel";
                        request.Review.Place.Name = hotel.HotelName;
                        break;

                }

                _reviewService.InsertNewReview(reference, request.Review);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion

        #region Update Review

        /// <summary>
        /// Update Review
        /// </summary>
        public object Put(ReviewRequest request)
        {
            try
            {
                var reference = "review:" + request.Guid;
                request.Review.Reference = reference;
                _reviewService.InsertNewReview(reference, request.Review);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion

        #region Archive Review

        /// <summary>
        /// Archive Review
        /// </summary>
        public object Delete(ReviewRequest request)
        {
            try
            {
                _reviewService.ArchiveReviewById("review:" + request.Guid);
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
