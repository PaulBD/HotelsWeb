using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

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
		private readonly ILikeService _likeService;

		/// <summary>
		/// Constructor
		/// </summary>
		public LikeApi(ILikeService likeService)
		{
			_likeService = likeService;
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

				response = _likeService.ReturnLikesByToken(token);

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

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				_likeService.InsertNewLike(token, request.Location);
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

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				_likeService.ArchiveLikeByLocationId(request.LocationId, token);
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
