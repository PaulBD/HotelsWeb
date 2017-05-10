using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.extras.services;
using core.extras.dtos;

namespace triperoo.apis.endpoints.airport
{
    #region Parking Availability

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/airport/{airportName}/parking", "GET")]
    public class ParkingRequest
    {
        public string AirportName { get; set; }
        public string DropoffDate { get; set; }
        public string DropoffTime { get; set; }
        public string PickupDate { get; set; }
        public string PickupTime { get; set; }
        public string Language { get; set; }
        public string Initials { get; set; }
        public int PassengerCount { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class ParkingRequestValidator : AbstractValidator<ParkingRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParkingRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.AirportName).NotNull().WithMessage("Supply a valid airport location");
                RuleFor(r => r.DropoffDate).NotNull().WithMessage("Supply a valid drop off date");
                RuleFor(r => r.DropoffTime).NotNull().WithMessage("Supply a valid drop off time");
                RuleFor(r => r.PickupDate).NotNull().WithMessage("Supply a valid pick up date");
                RuleFor(r => r.PickupTime).NotNull().WithMessage("Supply a valid pick up time");
                RuleFor(r => r.Language).NotNull().WithMessage("Supply a valid language");
            });
        }
    }

    #endregion

    #region API logic

    public class ParkingRequestApi : Service
    {
        private readonly IParkingService _parkingService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParkingRequestApi(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        #region Get Parking availability

        /// <summary>
        /// Get Parking availability
        /// </summary>
        public object Get(ParkingRequest request)
        {
            AirportParkingResponseDto response;

            try
            {
                response = _parkingService.AvailabilityAtDestination(request.AirportName, request.DropoffDate, request.DropoffTime, request.PickupDate, request.PickupTime, request.Initials, request.Language, request.PassengerCount);
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
