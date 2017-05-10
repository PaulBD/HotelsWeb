using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.extras.services;
using core.extras.dtos;

namespace triperoo.apis.endpoints.airport
{
    #region Airport Lounge Availability

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/airport/{airportName}/lounges", "GET")]
    public class LoungeRequest
    {
		public string AirportName { get; set; }
		public string ArrivalDate { get; set; }
		public string ArrivalTime { get; set; }
		public string FlightTime { get; set; }
        public int AdultCount { get; set; }
		public int ChildCount { get; set; }
		public int InfantCount { get; set; }
		public string Language { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class LoungeRequestValidator : AbstractValidator<LoungeRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoungeRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.AirportName).NotNull().WithMessage("Supply a valid airport location");
                RuleFor(r => r.ArrivalDate).NotNull().WithMessage("Supply a valid arrival date");
				RuleFor(r => r.ArrivalTime).NotNull().WithMessage("Supply a valid arrival time");
				RuleFor(r => r.FlightTime).NotNull().WithMessage("Supply a valid flight time");
                RuleFor(r => r.AdultCount).GreaterThanOrEqualTo(0).WithMessage("Supply a valid adult count");
				RuleFor(r => r.ChildCount).GreaterThanOrEqualTo(0).WithMessage("Supply a valid children count");
				RuleFor(r => r.InfantCount).GreaterThanOrEqualTo(0).WithMessage("Supply a valid infant count");
            });
        }
    }

    #endregion

    #region API logic

    public class LoungeRequestApi : Service
    {
        private readonly ILoungeService _loungeService;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoungeRequestApi(ILoungeService loungeService)
        {
            _loungeService = loungeService;
        }

        #region Get Lounge availability

        /// <summary>
        /// Get Lounge availability
        /// </summary>
        public object Get(LoungeRequest request)
        {
            LoungeAvailabilityResponseDto response;

            try
            {
                response = _loungeService.AvailabilityAtDestination(request.AirportName, request.ArrivalDate, request.ArrivalTime, request.FlightTime, request.AdultCount, request.ChildCount, request.InfantCount, request.Language);
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
