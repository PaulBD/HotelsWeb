using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using Couchbase;
using System.Collections.Generic;
using Couchbase.Configuration.Client;
using Couchbase.Authentication;

namespace triperoo.apis.endpoints.location
{
    #region Return Single Location By Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}", "GET")]
    [Route("/v1/location/{id}", "PUT")]
    [Route("/v1/location/", "POST")]
    public class LocationRequest
    {
        public int id { get; set; }
        public LocationDto Location { get; set; }

    }

    /// <summary>
    /// Validator
    /// </summary>
    public class LocationRequestValidator : AbstractValidator<LocationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocationRequestValidator()
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

    public class LocationApi : Service
    {
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationApi(ILocationService locationService)
        {
            _locationService = locationService;
        }

        #region Return Single Location By Id

        /// <summary>
        /// Return Single Location By Id
        /// </summary>
        public object Get(LocationRequest request)
        {
            LocationDto response = null;

            try
            {
                response = _locationService.ReturnLocationById(request.id);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region Update Location Address By Id

        /// <summary>
        /// Update Location Address By Id
        /// </summary>
        public object Put(LocationRequest request)
        {
            LocationDto response = null;

            try
            {
                response = _locationService.ReturnLocationById(request.id);

                response.FormattedAddress = request.Location.FormattedAddress;
                response.Summary = request.Location.Summary;
                response.LocationCoordinates = request.Location.LocationCoordinates;
                response.ContactDetails = request.Location.ContactDetails;
                response.Tags = request.Location.Tags;
                _locationService.UpdateLocation(response, true);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion


        #region Add Location Address By Id

        /// <summary>
        /// Add Location Address By Id
        /// </summary>
        public object Post(LocationRequest request)
        {
            try
            {
                _locationService.AddLocation(request.Location, true);
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
