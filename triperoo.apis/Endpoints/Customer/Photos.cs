using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.customer
{

    #region Photos Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/{reference}/photos", "GET")]
    public class CustomerPhotosRequest
    {
        public string Reference { get; set; }
    }

    #endregion

    #region Photo Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/customer/{reference}/photos/{photoReference}", "DELETE")]
    public class CustomerPhotoRequest
    {
        public string Reference { get; set; }
        public string PhotoReference { get; set; }
    }

    #endregion

    #region API logic

    public class PhotoApi : Service
    {
        private readonly IPhotoService _customerPhotoService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoApi(IPhotoService customerPhotoService)
        {
            _customerPhotoService = customerPhotoService;
        }

        #region Return Photos By Customer Reference

        /// <summary>
        /// Return Photos By Token
        /// </summary>
        public object Get(CustomerPhotosRequest request)
        {
            var response = new CustomerPhotos();

            try
            {
                response = _customerPhotoService.ReturnPhotosByReference(request.Reference);

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

        #region Delete Photos By Id

        /// <summary>
        /// Delete Photos By Id
        /// </summary>
        public object Delete(CustomerPhotoRequest request)
        {
            try
            {
                var token = Request.Headers.Get("token");

                if (token == null)
                {
                    throw HttpError.Unauthorized("You are unauthorized to access this page");
                }

                _customerPhotoService.ArchivePhotoById(token, request.PhotoReference);
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
