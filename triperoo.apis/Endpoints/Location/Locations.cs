using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;
using System.Collections.Generic;
using System.Linq;

namespace triperoo.apis.endpoints.location
{
	#region Return a List of Locations By Search Value

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/locations/search")]
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

    #region Return Child Locations By Parent Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/locations/{parentLocationId}", "GET")]
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
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
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

		#region Return Child Locations By Parent Id

		/// <summary>
		/// Lists parent location by Id
		/// </summary>
		public object Get(ParentLocationRequest request)
        {
            string cacheName = "parentLocations:" + request.parentLocationId;
            List<LocationDto> response = new List<LocationDto>();

            try
            {
                response = _locationService.ReturnLocationByParentId(request.parentLocationId, request.Type);

                if (response != null)
                {
                    if (request.PageNumber > 0)
                    {
                        response = response.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                    }
                    else
                    {
                        response = response.Take(request.PageSize).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

		#endregion

		#region Return a List of Locations By Search Value

		/// <summary>
		/// Return a List of Locations By Search Value
		/// </summary>
		public object Get(LocationSearchRequest request)
        {
            LocationListDto response = new LocationListDto();
			List<LocationDto> result = null;

            try
            {
                string search = request.SearchValue.ToLower();

                result = _locationService.ReturnLocationsForAutocomplete(search.Substring(0, 3));

                if (result.Count > 0)
                {
                    if (request.SearchType.ToLower() == "all")
                    {
                        response.Locations = result.Where(q => q.RegionNameLong.ToLower().StartsWith(search)).OrderBy(q => q.SearchPriority).OrderBy(q => q.ListingPriority).ToList().Take(10).ToList();
                    }
                    else {
                        response.Locations = result.Where(q => q.RegionType.ToLower() == request.SearchType.ToLower()).Where(q => q.RegionNameLong.ToLower().StartsWith(search)).OrderBy(q => q.SearchPriority).OrderBy(q => q.ListingPriority).Take(10).ToList();
                    }
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
