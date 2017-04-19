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

    #region Return locations by parent id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/locations", "GET")]
    public class ParentLocationRequest
    {
        public int parentLocationId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParentLocationRequestValidator : AbstractValidator<ParentLocationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentLocationRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.parentLocationId).GreaterThan(0).WithMessage("Invalid parent location id have been supplied");
                RuleFor(r => r.PageSize).NotNull().WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).NotNull().WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class LocationsApi : Service
    {
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationsApi(ILocationService locationService)
        {
            _locationService = locationService;
        }

        #region List Parent Location by Id

        /// <summary>
        /// Lists parent location by Id
        /// </summary>
        public object Get(ParentLocationRequest request)
        {
            string cacheName = "parentLocations:" + request.parentLocationId;
            List<LocationDto> response = null;

            try
            {
                response = Cache.Get<List<LocationDto>>(cacheName);

                if (response == null)
                {
                    response = _locationService.ReturnLocationByParentId(request.parentLocationId, request.Type);
                    base.Cache.Add(cacheName, response);
                }

                if (request.PageNumber > 0)
                {
                    response = response.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
                else
                {
                    response = response.Take(request.PageSize).ToList();
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
            LocationListDto response = new LocationListDto();
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
                        locations = result.Where(q => q.RegionNameLong.ToLower().StartsWith(search)).OrderBy(q => q.SearchPriority).OrderBy(q => q.ListingPriority).ToList().Take(10).ToList();
                    }
                    else {
                        locations = result.Where(q => q.RegionType.ToLower() == request.SearchType.ToLower()).Where(q => q.RegionNameLong.ToLower().StartsWith(search)).OrderBy(q => q.SearchPriority).OrderBy(q => q.ListingPriority).Take(10).ToList();
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
