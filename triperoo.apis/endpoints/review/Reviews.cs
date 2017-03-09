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
    #region Customer Reviews Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/reviews/{Type}", "GET")]
    public class TopReviewRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Type { get; set; }
    }

    #endregion

    #region Customer Reviews Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/reviews/customer/{Guid}", "GET")]
    public class CustomerReviewRequest
    {
        public string Guid { get; set; }
    }

    #endregion

    #region Place Reviews Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/reviews/place/{Type}/{Guid}", "GET")]
    public class PlaceReviewRequest
    {
        public string Guid { get; set; }
        public string Type { get; set; }
    }

    #endregion

    #region API logic

    public class ReviewsApi : Service
    {
        private readonly IReviewService _reviewService;
        private readonly ICustomerService _customerService;
        private readonly IHotelService _hotelService;
        private readonly IPlaceService _placeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsApi(IReviewService reviewService, ICustomerService customerService, IHotelService hotelService, IPlaceService placeService)
        {
            _reviewService = reviewService;
            _customerService = customerService;
            _hotelService = hotelService;
            _placeService = placeService;
        }

        #region Return Reviews By Customer Reference

        /// <summary>
        /// Return Reviews By Customer
        /// </summary>
        public object Get(CustomerReviewRequest request)
        {
            var response = new List<ReviewDto>();

            try
            {
                response = _reviewService.ReturnReviewsByCustomerReference("customer:" + request.Guid);

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

        #region Return Reviews By Place Reference

        /// <summary>
        /// Return Reviews By Place
        /// </summary>
        public object Get(PlaceReviewRequest request)
        {
            var response = new List<ReviewDto>();

            try
            {
                response = _reviewService.ReturnReviewsByPlaceReference(request.Type, request.Guid);

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

        #region Return Top Reviews By Type

        /// <summary>
        /// Return Top Reviews By Type
        /// </summary>
        public object Get(TopReviewRequest request)
        {
            var response = new List<ReviewDto>();

            try
            {
                response = _reviewService.ReturnReviewsByType(request.Type, request.Offset, request.Limit);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
                }

                // TODO: Fix this using query
                // TODO: Add Caching

                foreach (var r in response)
                {
                    var customer = _customerService.ReturnCustomerByReference(r.TriperooReviews.CustomerReference);

                    if (customer != null)
                    {
                        r.TriperooReviews.ProfileImage = customer.TriperooCustomers.Profile.ImageUrl;
                        r.TriperooReviews.ProfileUrl = customer.TriperooCustomers.Profile.ProfileUrl;
                        r.TriperooReviews.CustomerName = customer.TriperooCustomers.Profile.Name;
                    }

                    switch (r.TriperooReviews.PlaceType)
                    {
                        case "hotel":
                            var hotel = _hotelService.ReturnHotelById(r.TriperooReviews.Reference);
                            r.TriperooReviews.PlaceName = hotel.HotelName;
                            r.TriperooReviews.PlaceAddress = hotel.Address1;
                            r.TriperooReviews.PlaceCity = hotel.HotelCity;
                            r.TriperooReviews.PlaceCountry = hotel.HotelCountry;
                            r.TriperooReviews.PlaceImage = hotel.HotelImage;
                            r.TriperooReviews.PlaceUrl = hotel.HotelUrl;
                            break;
                        case "country":
                        case "city":
                            var place = _placeService.ReturnPlaceById(r.TriperooReviews.Reference, r.TriperooReviews.PlaceType);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
