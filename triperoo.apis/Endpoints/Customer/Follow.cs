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

	#region Follows Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/{customerId}/follows", "GET")]
	public class FollowsRequest
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

		#region Get all follows by customer

		/// <summary>
		/// Get all follows by customer
		/// </summary>
		public object Get(FollowsRequest request)
		{
            var response = new List<FriendDto>();

			try
			{
				response = _customerFollowService.ListFriends(request.customerId);
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
