using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
    #region Favourite Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/favourite/{id}", "GET")]
    [Route("/v1/customer/favourite/{id}", "DELETE")]
    [Route("/v1/customer/favourite", "POST")]
    public class FavouriteRequest
    {
        public int Id { get; set; }
        public FavouriteDto Favourite { get; set; }
    }

    #endregion

    #region Favourites Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/favourites", "GET")]
    public class FavouritesRequest
    {
    }

    #endregion

    #region API logic

    public class FavouriteApi : Service
    {
        private readonly IFavouriteService _favouriteService;

        /// <summary>
        /// Constructor
        /// </summary>
        public FavouriteApi(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        #region Return Favorites By Token

        /// <summary>
        /// Return Favourite By Token
        /// </summary>
        public object Get(FavouritesRequest request)
        {
            var response = new List<FavouriteDto>();

            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                response = _favouriteService.ReturnFavouritesByToken(token);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region Return Favorite By Id

        /// <summary>
        /// Return Favourite By Token
        /// </summary>
        public object Get(FavouriteRequest request)
        {
            var response = new FavouriteDto();

            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                response = _favouriteService.ReturnFavouriteById(request.Id, token);

                if (response == null)
                {
                    throw new HttpError(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region Insert Favourite

        /// <summary>
        /// Insert New Favourite
        /// </summary>
        public object Post(FavouriteRequest request)
        {
            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                _favouriteService.InsertNewFavourite(token, request.Favourite);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion

        #region Archive Existing Favourite

        /// <summary>
        /// Archive Favourite
        /// </summary>
        public object Delete(FavouriteRequest request)
        {
            var response = new CustomerDto();

            try
            {
                var token = Request.Headers.Get("securityToken");

                if (token == null)
                {
                    throw new HttpError(HttpStatusCode.Unauthorized, "Unauthorized");
                }

                _favouriteService.ArchiveFavouriteById(request.Id, token);
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
