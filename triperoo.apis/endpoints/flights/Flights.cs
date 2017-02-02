using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.flights.services;
using core.flights.dtos;

namespace triperoo.apis.endpoints.flights
{
    #region Return a list of cached flights

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/flights/{Market}/{Currency}/{Locale}/{OriginPlace}/{DestinationPlace}/{OutboundPartialDate}/{InboundPartialDate}")]
    public class FlightCacheRequest
    {
        public string Market { get; set; }
        public string Currency { get; set; }
        public string Locale { get; set; }
        public string OriginPlace { get; set; }
        public string DestinationPlace { get; set; }
        public string OutboundPartialDate { get; set; }
        public string InboundPartialDate { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class FlightCacheRequestValidator : AbstractValidator<FlightCacheRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FlightCacheRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.OriginPlace).NotNull().WithMessage("Supply a valid from location");
                RuleFor(r => r.DestinationPlace).NotNull().WithMessage("Supply a valid to location");
                RuleFor(r => r.OutboundPartialDate).NotNull().WithMessage("Supply a valid outbound date");
                RuleFor(r => r.Market).NotNull().WithMessage("Supply a valid market");
                RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
                RuleFor(r => r.Currency).NotNull().WithMessage("Supply a valid currency");
            });
        }
    }

    #endregion

    #region API logic

    public class FlightApi : Service
    {
        // Dependencies
        private readonly IFlightService _flightService;

        /// <summary>
        /// Constructor
        /// </summary>
        public FlightApi(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// List all cached flights
        /// </summary>
        public object Get(FlightCacheRequest request)
        {
            FlightListDto response = null;

            try
            {
                response = _flightService.ReturnCachedFlights(request.OriginPlace, request.DestinationPlace, request.OutboundPartialDate, request.InboundPartialDate, request.Market, request.Currency, request.Locale);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }
    }

    #endregion
}
