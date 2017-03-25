using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Customer Reviews Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/reviews", "GET")]
    public class ReviewsRequest
    {
        public int Id { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Type { get; set; }
    }

    #endregion

    #region API logic

    public class ReviewsApi : Service
    {
        private readonly IReviewService _reviewService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsApi(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        #region Return Top Reviews By Type

        /// <summary>
        /// Return Top Reviews By Type
        /// </summary>
        public object Get(ReviewsRequest request)
        {
            var response = new List<ReviewDto>();

            try
            {
                if (request.Id > 0)
                {
                    response = _reviewService.ReturnReviewsByLocationId(request.Id, request.Offset, request.Limit);
                }
                else
                {
                    response = _reviewService.ReturnReviewsByType(request.Type, request.Offset, request.Limit);
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
