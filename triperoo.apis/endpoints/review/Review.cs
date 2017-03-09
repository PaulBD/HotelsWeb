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

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewApi(IReviewService reviewService, ICustomerService customerService)
        {
            _reviewService = reviewService;
            _customerService = customerService;

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
                request.Review.ReviewReference = reference;
                request.Review.DateCreated = DateTime.Now;
                request.Review.CustomerReference = customer.TriperooCustomers.CustomerReference;

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
                request.Review.ReviewReference = reference;
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
