using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.flights.services;
using core.flights.dtos;

namespace triperoo.apis.endpoints.flights
{
    #region Return a list of airports

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/airports")]
    public class AirportRequest
    {
        public string Term { get; set; }
        public string Locale { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class AirportRequestValidator : AbstractValidator<AirportRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AirportRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Term).NotNull().WithMessage("Supply a valid search term");
                RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
            });
        }
    }

    #endregion

    #region API logic

    public class AirportApi : Service
    {
        // Dependencies
        private readonly IFlightService _flightService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AirportApi(IFlightService flightService)
        {
            _flightService = flightService;
        }

        /// <summary>
        /// List all airports
        /// </summary>
        public object Get(AirportRequest request)
        {
            AirportLocationDto response = null;

            try
            {
                response = _flightService.ReturnAirports(request.Term, request.Locale);
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
