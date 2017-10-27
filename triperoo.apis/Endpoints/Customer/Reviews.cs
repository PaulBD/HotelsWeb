using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace triperoo.apis.endpoints.customer
{
	#region Review Endpoint

    /// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/{customerId}/reviews", "GET")]
	public class ReviewsRequest
	{
        public string customerId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
	}

	#endregion

	#region API logic

	public class ReviewApi : Service
	{
        private readonly IReviewService _reviewService;

		/// <summary>
		/// Constructor
		/// </summary>
		public ReviewApi(IReviewService reviewService)
		{
            _reviewService = reviewService;
		}

		#region Get all reviews by customer

		/// <summary>
		/// Get all reviews by customer
		/// </summary>
		public object Get(ReviewsRequest request)
		{
			var response = new List<ReviewDto>();

			try
			{
				response = _reviewService.ReturnReviewsByCustomer(request.customerId);

                if (request.PageNumber > 0)
                {
                    response = response.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
                else
                {
                    response = response.Take(request.PageSize).ToList();
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
