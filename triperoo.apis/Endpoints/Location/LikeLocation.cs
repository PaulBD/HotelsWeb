using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.location
{
	#region Update Likes on Location By Id

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}/like", "PUT")]
	public class LikeLocationRequest
	{
        public int id { get; set; }
        public bool IsCity { get; set; }

	}

	/// <summary>
	/// Validator
	/// </summary>
	public class LikeLocationRequestValidator : AbstractValidator<LikeLocationRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LikeLocationRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
			});

		}
	}

	#endregion

	#region Update Likes on Location By Id

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/location/{id}/unlike", "PUT")]
	public class UnLikeLocationRequest
	{
        public int id { get; set; }
        public bool IsCity { get; set; }

	}

	/// <summary>
	/// Validator
	/// </summary>
	public class UnlikeLocationRequestValidator : AbstractValidator<UnLikeLocationRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public UnlikeLocationRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
			});

		}
	}

	#endregion

	#region API logic

	public class LikeLocationApi : Service
    {
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LikeLocationApi(ILocationService locationService)
        {
            _locationService = locationService;
        }

		#region Update Likes on Location By Id

		/// <summary>
		/// Update Likes on Location By Id
		/// </summary>
		public object Put(LikeLocationRequest request)
		{
			LocationDto response = null;

			try
			{
				response = _locationService.ReturnLocationById(request.id, request.IsCity);
                response.Stats.LikeCount += 1;
				_locationService.UpdateLocation(response, false);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

        #region Update Likes on Location By Id

		/// <summary>
		/// Update Likes on Location By Id
		/// </summary>
		public object Put(UnLikeLocationRequest request)
		{
			LocationDto response = null;

			try
			{
				response = _locationService.ReturnLocationById(request.id, request.IsCity);
				response.Stats.LikeCount -= 1;
				_locationService.UpdateLocation(response, false);
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
