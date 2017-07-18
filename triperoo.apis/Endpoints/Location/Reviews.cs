using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace triperoo.apis.endpoints.location
{
	#region Return Location Reviews By Id

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}/reviews", "GET")]
    public class ReviewsLocationRequest
    {
		public int Id { get; set; }
		public string Tags { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ReviewsLocationRequestValidator : AbstractValidator<ReviewsLocationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsLocationRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
				RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
				RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
            });
        }
	}

	#endregion

	#region Return Reviews By Type

	/// <summary>
	/// Request
	/// </summary>
    [Route("/v1/reviews/{type}", "GET")]
	public class ReviewsTypeRequest
	{
		public string Type { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}

	/// <summary>
	/// Validator
	/// </summary>
	public class ReviewsByTypeRequestValidator : AbstractValidator<ReviewsTypeRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ReviewsByTypeRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Type).NotNull().WithMessage("Invalid review type has been supplied");
				RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
				RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
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

		#region Return Location Reviews By Id

		/// <summary>
		/// Return Top Reviews By Type
		/// </summary>
		public object Get(ReviewsLocationRequest request)
        {
			var response = new ReviewListDto();
			string cacheName = "reviews:" + request.Id;

            try
			{
				response = Cache.Get<ReviewListDto>(cacheName);


				if (response == null)
				{
                    response = new ReviewListDto();
					response.ReviewDto = _reviewService.ReturnReviewsByLocationId(request.Id);
				}

				var tags = request.Tags;

                if (!string.IsNullOrEmpty(tags))
                {
                    var list = new List<ReviewDto>();

                    if (tags.Contains(","))
                    {
                        foreach (var s in tags.Split(','))
                        {
                            list.AddRange(response.ReviewDto.Where(q => q.Tags.Contains(s)).ToList());
                        }
                    }
					else
					{
						list.AddRange(response.ReviewDto.Where(q => q.Tags.Contains(tags)).ToList());
                    }

                    response.ReviewDto = list;
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

		#region Return Reviews By Type

		/// <summary>
		/// Return Top Reviews By Type
		/// </summary>
		public object Get(ReviewsTypeRequest request)
		{
			var response = new ReviewListDto();
			string cacheName = "reviews:" + request.Type;

			try
			{
				response = Cache.Get<ReviewListDto>(cacheName);

				if (response == null)
				{
                    response = new ReviewListDto();
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
