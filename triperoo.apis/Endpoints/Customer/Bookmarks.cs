using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Bookmark Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/trips/{tripId}/bookmark/{locationId}", "GET")]
    [Route("/v1/customer/trips/{tripId}/bookmark/{locationId}", "DELETE")]
    [Route("/v1/customer/trips/{tripId}/bookmark", "POST")]
    public class BookmarkRequest
    {
		public int LocationId { get; set; }
		public int TripId { get; set; }
        public CustomerLocationDto Location { get; set; }
    }

    #endregion

    #region Bookmarks Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/trips/{tripId}/bookmarks", "GET")]
    public class CustomerBookmarksRequest
	{
		public int TripId { get; set; }
    }

    #endregion

    #region API logic

    public class BookmarkApi : Service
    {
        private readonly IBookmarkService _bookmarkService;

        /// <summary>
        /// Constructor
        /// </summary>
        public BookmarkApi(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

		#region Return Bookmarks By Token

		/// <summary>
		/// Return Bookmarks By Token
		/// </summary>
		public object Get(CustomerBookmarksRequest request)
        {
            var response = new List<CustomerLocationDto>();

            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _bookmarkService.ReturnBookmarksByToken(token, request.TripId);

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

		#region Return Bookmark By Id

		/// <summary>
		/// Return Bookmark By Location & Trip Id
		/// </summary>
		public object Get(BookmarkRequest request)
        {
            var response = new CustomerLocationDto();

            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _bookmarkService.ReturnBookmarkByLocationId(request.LocationId, request.TripId, token);

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

		#region Insert Bookmark

		/// <summary>
		/// Insert New Bookmark
		/// </summary>
		public object Post(BookmarkRequest request)
        {
            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _bookmarkService.InsertNewBookmark(token, request.TripId, request.Location);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

		#endregion

		#region Archive Existing Bookmark

		/// <summary>
		/// Archive Existing Bookmark
		/// </summary>
		public object Delete(BookmarkRequest request)
        {
            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _bookmarkService.ArchiveBookmarkByLocationId(request.LocationId, request.TripId, token);
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
