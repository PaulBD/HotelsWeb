using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Activity Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/trips/{tripId}/activity/{locationId}", "GET")]
    [Route("/v1/customer/trips/{tripId}/activity/{locationId}", "DELETE")]
    [Route("/v1/customer/trips/{tripId}/activity", "POST")]
    public class ActivityRequest
    {
		public int LocationId { get; set; }
		public int TripId { get; set; }
        public ActivityDto Activity { get; set; }
    }

    #endregion

    #region Activities Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/trips/{tripId}/activities", "GET")]
    public class CustomerActivitiesRequest
	{
		public int TripId { get; set; }
    }

    #endregion

    #region API logic

    public class ActivityApi : Service
    {
        private readonly IActivityService _activityService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityApi(IActivityService activityService)
        {
            _activityService = activityService;
        }

		#region Return Activities By Token

		/// <summary>
        /// Return Activities By Token
		/// </summary>
        public object Get(CustomerActivitiesRequest request)
        {
            var response = new List<ActivityDto>();

            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _activityService.ReturnActivitiesByToken(token, request.TripId);

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

		#region Return Activity By Location & Trip Id

		/// <summary>
		/// Return Activity By Location & Trip Id
		/// </summary>
		public object Get(ActivityRequest request)
        {
            var response = new ActivityDto();

            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _activityService.ReturnActivityByLocationId(request.LocationId, request.TripId, token);

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

        #region Insert Activity

		/// <summary>
        /// Insert New Activity
		/// </summary>
        public object Post(ActivityRequest request)
        {
            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _activityService.InsertNewActivity(token, request.TripId, request.Activity);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

		#endregion

		#region Archive Existing Activity

		/// <summary>
        /// Archive Existing Activity
		/// </summary>
		public object Delete(ActivityRequest request)
        {
            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _activityService.ArchiveActivityByLocationId(request.LocationId, request.TripId, token);
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
