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
        public string Location { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
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
                RuleFor(r => r.Location).NotEmpty().WithMessage("Supply a valid location parameter");
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
            string cacheName = "deals:hotels:" + request.Location;
            List<TravelzooDto> response = null;

            try
            {
                response = Cache.Get<List<TravelzooDto>>(cacheName);

                if (response == null)
                {
                    response = _travelzooService.ReturnDeals(request.Location, request.Limit, request.Offset);
                    base.Cache.Add(cacheName, response);
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