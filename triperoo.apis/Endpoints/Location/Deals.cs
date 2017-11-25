using core.deals.dtos;
using core.deals.Services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.location
{
    #region Return Deals By Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/deals")]
    public class DealRequest : Service
    {
        public int Id { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool Exclude { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class DealRequestValidator : AbstractValidator<DealRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DealRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id has been supplied");
				RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
				RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class DealsApi : Service
    {
        private readonly ITravelzooService _travelzooService;
		private readonly ILocationService _locationService;

		/// <summary>
		/// Constructor
		/// </summary>
		public DealsApi(ITravelzooService travelzooService, ILocationService locationService)
		{
			_locationService = locationService;
            _travelzooService = travelzooService;
        }

		#region Return Deals By Location Id

		/// <summary>
		/// Return Hotel Deals By Location Id
		/// </summary>
		public object Get(DealRequest request)
        {
			LocationDto locationResponse = new LocationDto();
            List<TravelzooDto> response = null;

            try
            {
                locationResponse = _locationService.ReturnLocationById(request.Id, true);

                if (locationResponse != null)
                {
                    if (request.Exclude)
                    {
                        response = _travelzooService.ReturnDealsExcludeLocation(locationResponse.RegionName);
                    }
                    else {
                        response = _travelzooService.ReturnDeals(locationResponse.RegionName);
                    }
                }

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
    }

    #endregion
}