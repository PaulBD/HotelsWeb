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
    [Route("/v1/customer/bookmark/{locationId}", "GET")]
    [Route("/v1/customer/bookmark/{locationId}", "DELETE")]
    [Route("/v1/customer/bookmark", "POST")]
    public class BookmarkRequest
    {
        public int LocationId { get; set; }
        public BookmarkDto Bookmark { get; set; }
    }

    #endregion

    #region Favourites Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/bookmarks", "GET")]
    public class CustomerBookmarksRequest
    {
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

        #region Return Customer Favorites By Token

        /// <summary>
        /// Return Favourite By Token
        /// </summary>
        public object Get(CustomerBookmarksRequest request)
        {
            var response = new List<BookmarkDto>();

            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _bookmarkService.ReturnBookmarksByToken(token);

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
		/// Return Bookmark By Token
		/// </summary>
		public object Get(BookmarkRequest request)
        {
            var response = new BookmarkDto();

            try
			{
				var token = Request.Headers["token"];

                if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                response = _bookmarkService.ReturnBookmarkByLocationId(request.LocationId, token);

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

                _bookmarkService.InsertNewBookmark(token, request.Bookmark);
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
		/// Archive Favourite
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

                _bookmarkService.ArchiveBookmarkByLocationId(request.LocationId, token);
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
