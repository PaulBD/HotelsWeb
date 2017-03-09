using core.customers.dtos;
using core.customers.services;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsApi(IReviewService reviewService, ICustomerService customerService)
        {
            _reviewService = reviewService;
            _customerService = customerService;
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
