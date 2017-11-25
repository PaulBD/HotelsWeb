using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;
using core.places.services;

namespace triperoo.apis.endpoints.customer
{
	#region Like Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/likes/{locationId}", "DELETE")]
	[Route("/v1/customer/likes", "POST")]
	public class LikeRequest
	{
        public int LocationId { get; set; }
		public CustomerLocationDto Location { get; set; }
	}

	#endregion

	#region Likes Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/likes", "GET")]
	public class CustomerLikesRequest
	{
	}

	#endregion

	#region API logic

	public class LikeApi : Service
	{
		private readonly ILikeService _customerLikeService;
		private readonly ILocationService _locationService;

		/// <summary>
		/// Constructor
		/// </summary>
		public LikeApi(ILikeService customerLikeService, ILocationService locationService)
		{
			_customerLikeService = customerLikeService;
            _locationService = locationService;
		}

		#region Return Likes By Token

		/// <summary>
		/// Return Likes By Token
		/// </summary>
		public object Get(CustomerLikesRequest request)
		{
			var response = new List<CustomerLocationDto>();

			try
			{
				var token = Request.Headers.Get("token");

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				response = _customerLikeService.ReturnLikesByToken(token);

				if (response == null)
				{
					throw HttpError.NotFound("Customer details cannot be found");
				}
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Insert New Like

		/// <summary>
		/// Insert New Like
		/// </summary>
		public object Post(LikeRequest request)
		{
			try
			{
				var token = Request.Headers["token"];

                if (token != null)
                {
                    _customerLikeService.InsertNewLike(token, request.Location);
                }

				var locationResponse = _locationService.ReturnLocationById(request.Location.Id, request.Location.IsCity);

				locationResponse.Stats.LikeCount += 1;
				_locationService.UpdateLocation(locationResponse, false);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(HttpStatusCode.OK);
		}

		#endregion

		#region Archive Existing Like

		/// <summary>
		/// Archive Existing Like
		/// </summary>
		public object Delete(LikeRequest request)
		{
			try
			{
				var token = Request.Headers["token"];

                if (token != null)
                {
                    _customerLikeService.ArchiveLikeByLocationId(request.LocationId, token);
                }

				var locationResponse = _locationService.ReturnLocationById(request.LocationId, request.Location.IsCity);

				locationResponse.Stats.LikeCount -= 1;
				_locationService.UpdateLocation(locationResponse, false);
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
