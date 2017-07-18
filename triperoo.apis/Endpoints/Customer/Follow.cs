using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
	#region Follow Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/{customerId}/follow", "DELETE")]
    [Route("/v1/customer/{customerId}/follow", "POST")]
	public class FollowRequest
	{
        public string customerId { get; set; }
	}

	#endregion

	#region Following Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/{customerId}/following", "GET")]
	public class FollowingRequest
	{
		public string customerId { get; set; }
	}

	#endregion

	#region Followed By Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/{customerId}/followedBy", "GET")]
	public class FollowedByRequest
	{
		public string customerId { get; set; }
	}

	#endregion

	#region API logic

	public class FollowApi : Service
	{
		private readonly IFollowService _customerFollowService;

		/// <summary>
		/// Constructor
		/// </summary>
		public FollowApi(IFollowService customerFollowService)
		{
			_customerFollowService = customerFollowService;
		}

		#region Get all people following customer

		/// <summary>
		/// Get all people followed by customer
		/// </summary>
		public object Get(FollowingRequest request)
		{
            var response = new List<FollowerDto>();

			try
			{
				response = _customerFollowService.ListFriendsFollowingCustomer(request.customerId);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Get all people followed by customer

		/// <summary>
		/// Get all people followed by customer
		/// </summary>
		public object Get(FollowedByRequest request)
		{
			var response = new List<FollowerDto>();

			try
			{
				response = _customerFollowService.ListFriendsFollowedByCustomer(request.customerId);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Insert New Follow

		/// <summary>
		/// Insert New Follow
		/// </summary>
		public object Post(FollowRequest request)
		{
			try
			{
				var token = Request.Headers["token"];

				if (token != null)
				{
					_customerFollowService.FollowFriend(request.customerId, token);
				}
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(HttpStatusCode.OK);
		}

		#endregion

		#region Archive Existing Follow

		/// <summary>
		/// Archive Existing Fllow
		/// </summary>
		public object Delete(FollowRequest request)
		{
			try
			{
				var token = Request.Headers["token"];

				if (token != null)
				{
					_customerFollowService.UnfollowFriend(request.customerId, token);
				}
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
