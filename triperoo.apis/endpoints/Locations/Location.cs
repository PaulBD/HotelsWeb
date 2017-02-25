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
    [Route("/v1/locations/search")]
    public class LocationSearchRequest : Service
    {
        public string Q { get; set; }
        public string type { get; set; }
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
                RuleFor(r => r.Q).NotNull().WithMessage("Supply a valid search parameter");
                RuleFor(r => r.Q).Length(3, 250).WithMessage("Supply a valid search parameter greater than 3 characters");
            });
        }
    }

    #endregion

    #region Return a single location by id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{type}/{id}", "GET")]
    public class LocationRequest
    {
        public int id { get; set; }
        public string type { get; set; }

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
                RuleFor(r => r.type).NotEmpty().WithMessage("Invalid type have been supplied");
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

        #region List Location by Reference

        /// <summary>
        /// Lists location by reference (type:id)
        /// </summary>
        public object Get(LocationRequest request)
        {
            LocationDto response = new LocationDto();
            string cacheName = request.type + ":" + request.id;
            List<LocationDto> result = null;

            try
            {
                result = Cache.Get<List<LocationDto>>(cacheName);

                if (result == null)
                {
                    result = _locationService.ReturnLocationById(request.type + ":" + request.id);
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
            string search = request.Q.ToLower();
            string cacheName = "autocomplete:" + search.Substring(0, 3);
            List<AutocompleteDto> result = null;
            List<Place> place = null;

            try
            {
                result = Cache.Get<List<AutocompleteDto>>(cacheName);

                if (result == null)
                {
                    result = _locationService.ReturnLocationsForAutocomplete(search);
                }

                if (result.Count > 0)
                {
                    base.Cache.Add(cacheName, result);

                    if (request.type.ToLower() == "all")
                    {
                        place = result[0].TriperooCommon.places.Where(q => q.name.ToLower().StartsWith(search)).OrderBy(q => q.priority).Take(10).ToList();
                    }
                    else {
                        place = result[0].TriperooCommon.places.Where(q => q.type.ToLower() == request.type.ToLower()).Where(q => q.name.ToLower().StartsWith(search)).OrderBy(q => q.priority).Take(10).ToList();
                    }

                    response.TriperooCommon.count = place.Count;
                    response.TriperooCommon.places = place;
                    response.TriperooCommon.letterIndex = search;
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
