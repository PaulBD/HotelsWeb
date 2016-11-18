using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.places
{
    #region Return a list of places by town & type

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/places/{Country}/{Town}/{Type}")]
    [Route("/v1/places/{Country}/{Town}/{Type}/{Offset}/{Limit}")]
    public class PlacesByTownRequest
    {
        public string Type { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class PlacesByTownRequestValidator : AbstractValidator<PlacesByTownRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlacesByTownRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Type).NotNull().WithMessage("Supply a valid type");
                RuleFor(r => r.Town).NotNull().WithMessage("Supply a valid town");
                RuleFor(r => r.Country).NotNull().WithMessage("Supply a valid country");
            });
        }
    }

    #endregion

    #region Return a list of places by longitude, latitude & type

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/places/{Latitude}/{Longitude}/{Radius}/{Type}")]
    [Route("/v1/places/{Latitude}/{Longitude}/{Radius}/{Type}/{Offset}/{Limit}")]
    public class PlacesByProximityRequest
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Radius { get; set; }
        public string Type { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class PlacesByProximityRequestValidator : AbstractValidator<PlacesByProximityRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlacesByProximityRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Longitude).NotNull().WithMessage("Supply a valid longitude");
                RuleFor(r => r.Latitude).NotNull().WithMessage("Supply a valid latitude");
                RuleFor(r => r.Radius).NotEmpty().WithMessage("Supply a valid radius");
                RuleFor(r => r.Type).NotNull().WithMessage("Supply a valid type");
            });
        }
    }

    #endregion

    #region Return a single place by id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/place/{FactualId}", "GET")]
    public class PlaceRequest
    {
        public string FactualId { get; set; }

    }

    /// <summary>
    /// Validator
    /// </summary>
    public class PlaceRequestValidator : AbstractValidator<PlaceRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlaceRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.FactualId).NotEmpty().WithMessage("Invalid place id have been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class PlacesApi : Service
    {
        // Dependencies
        private readonly IPlaceService _placeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlacesApi(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        /// <summary>
        /// Lists all places by town
        /// </summary>
        public object Get(PlacesByTownRequest request)
        {
            PlaceDto response;

            try
            {
                response = _placeService.ReturnPlacesByTownAndCountry(request.Town, request.Country, request.Type, request.Limit, request.Offset);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Lists all places by coordinates
        /// </summary>
        public object Get(PlacesByProximityRequest request)
        {
            PlaceDto response;

            try
            {
                response = _placeService.ReturnPlacesByProximity(request.Latitude, request.Longitude, request.Radius, request.Type, request.Offset, request.Limit);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Get a single place
        /// </summary>
        public object Get(PlaceRequest request)
        {
            PlaceDto response;
            try
            {
                response = _placeService.ReturnPlaceById(request.FactualId);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }
            
            return new HttpResult(response, HttpStatusCode.OK);
        }
    }

    #endregion
}
