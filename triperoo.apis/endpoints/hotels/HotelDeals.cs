using core.deals.dtos;
using core.deals.Services;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace triperoo.apis.endpoints.hotels
{
    #region Return a list of deals by location

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/hotel/deals")]
    public class HotelDealRequest : Service
    {
        public string LocationName { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelDealRequestValidator : AbstractValidator<HotelDealRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelDealRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.LocationName).NotEmpty().WithMessage("Supply a valid location name parameter");
                RuleFor(r => r.PageSize).NotNull().WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).NotNull().WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

    #region API logic

    public class LocationsApi : Service
    {
        private readonly ITravelzooService _travelzooService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationsApi(ITravelzooService travelzooService)
        {
            _travelzooService = travelzooService;
        }

        #region List Deals by location

        /// <summary>
        /// Lists parent location by Id
        /// </summary>
        public object Get(HotelDealRequest request)
        {
            string cacheName = "deals:hotels:" + request.LocationName;
            List<TravelzooDto> response = null;

            try
            {
                response = Cache.Get<List<TravelzooDto>>(cacheName);

                if (response == null)
                {
                    response = _travelzooService.ReturnDeals(request.LocationName);
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
    }

    #endregion
}