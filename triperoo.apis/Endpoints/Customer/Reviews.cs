using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

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
				var token = Request.Headers.Get("token");

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				response = _reviewService.ReturnReviewsByCustomer(request.customerId);
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
