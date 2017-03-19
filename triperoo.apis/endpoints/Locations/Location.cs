using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Collections.Generic;
using System.Linq;

namespace triperoo.apis.endpoints.locations
{
    #region Return a list of locations for autocomplete

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/autocomplete")]
    public class LocationSearchRequest : Service
    {
        public string SearchValue { get; set; }
        public string SearchType { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class LocationSearchRequestValidator : AbstractValidator<LocationSearchRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocationSearchRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.SearchValue).NotEmpty().WithMessage("Supply a valid search parameter");
                RuleFor(r => r.SearchValue).Length(3, 250).WithMessage("Supply a valid search parameter greater than 3 characters");
                RuleFor(r => r.SearchType).NotEmpty().WithMessage("Invalid type have been supplied");
            });
        }
    }

    #endregion

    #region Return a single location by id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location", "GET")]
    public class LocationRequest
    {
        public int id { get; set; }

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
                RuleFor(r => r.id).GreaterThan(0).WithMessage("Invalid location id have been supplied");
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

        #region List Location by Id

        /// <summary>
        /// Lists location by Id
        /// </summary>
        public object Get(LocationRequest request)
        {
            LocationDto response = new LocationDto();
            string cacheName = "places:" + request.id;
            List<LocationDto> result = null;

            try
            {
                result = Cache.Get<List<LocationDto>>(cacheName);

                if (result == null)
                {
                    result = _locationService.ReturnLocationById(request.id);
                    base.Cache.Add(cacheName, result);
                }

                if (result.Count > 0)
                {
                    response = result[0];
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion

        #region List all locations for autocomplete

        /// <summary>
        /// Lists all locations for autocomplete
        /// </summary>
        public object Get(LocationSearchRequest request)
        {
            AutocompleteDto response = new AutocompleteDto();
            List<LocationDto> result = null;
            List<LocationDto> locations = null;

            try
            {
                string search = request.SearchValue.ToLower();
                string cacheName = "autocomplete:" + search.Substring(0, 3);

                result = Cache.Get<List<LocationDto>>(cacheName);

                if (result == null)
                {
                    result = _locationService.ReturnLocationsForAutocomplete(search.Substring(0, 3));
                }

                if (result.Count > 0)
                {
                    base.Cache.Add(cacheName, result);

                    if (request.SearchType.ToLower() == "all")
                    {
                        locations = result.Where(q => q.Type.ToLower() == "city" || q.Type.ToLower() == "country" || q.Type.ToLower() == "Hotel").Where(q => q.Name.ToLower().StartsWith(search)).OrderBy(q => q.Priority).OrderBy(q => q.Name).Take(10).ToList();
                    }
                    else {
                        locations = result.Where(q => q.Type.ToLower() == request.SearchType.ToLower()).Where(q => q.Name.ToLower().StartsWith(search)).OrderBy(q => q.Priority).OrderBy(q => q.Name).Take(10).ToList();
                    }

                    response.Locations = locations;
                }
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
