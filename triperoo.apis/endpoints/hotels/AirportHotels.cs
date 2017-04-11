using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.extras.services;
using core.extras.dtos;

namespace triperoo.apis.endpoints.hotels
{
    #region Airport Hotel Availability

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/airporthotels", "GET")]
    public class AirportHotelRequest
    {
        public string LocationName { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartDate { get; set; }
        public string FlightDate { get; set; }
        public int Nights { get; set; }
        public string RoomType { get; set; }
        public string SecondRoomType { get; set; }
        public int ParkingDays { get; set; }
        public string Language { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class AirportHotelRequestValidator : AbstractValidator<AirportHotelRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AirportHotelRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.LocationName).NotNull().WithMessage("Supply a valid from location");
                RuleFor(r => r.ArrivalDate).NotNull().WithMessage("Supply a valid arrival date");
                RuleFor(r => r.DepartDate).NotNull().WithMessage("Supply a valid depart date");
                RuleFor(r => r.Language).NotNull().WithMessage("Supply a valid language");
            });
        }
    }

    #endregion

    #region API logic

    public class AirportHotelRequestApi : Service
    {
        private readonly IAirportHotelService _airportHotelService;

        /// <summary>
        /// Constructor
        /// </summary>
        public AirportHotelRequestApi(IAirportHotelService airportHotelService)
        {
            _airportHotelService = airportHotelService;
        }

        #region Get Airport Hotel availability

        /// <summary>
        /// Get Airport Hotel availability
        /// </summary>
        public object Get(AirportHotelRequest request)
        {
            AirportHotelResponseDto response;

            try
            {
                response = _airportHotelService.AvailabilityAtHotel(request.LocationName, request.ArrivalDate, request.DepartDate, request.FlightDate, request.Nights, request.RoomType, request.SecondRoomType, request.ParkingDays, request.Language);
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
