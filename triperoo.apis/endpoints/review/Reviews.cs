using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int LocationId { get; set; }
        public string Type { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ReviewsRequestValidator : AbstractValidator<ReviewsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.PageSize).NotNull().WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).NotNull().WithMessage("Invalid page number has been supplied");
            });
        }
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
            var response = new ReviewListDto();

            try
            {
                if (request.LocationId > 0)
                {
                    response.ReviewDto = _reviewService.ReturnReviewsByLocationId(request.LocationId);
                }
                else
                {
                    response.ReviewDto = _reviewService.ReturnReviewsByType(request.Type);
                }

                response.ReviewCount = response.ReviewDto.Count;

                if (request.PageNumber > 0)
                {
                    response.ReviewDto = response.ReviewDto.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
                else
                {
                    response.ReviewDto = response.ReviewDto.Take(request.PageSize).ToList();
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
