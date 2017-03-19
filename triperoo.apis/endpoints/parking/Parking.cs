using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.extras.services;
using core.extras.dtos;

namespace triperoo.apis.endpoints.parking
{
    #region Send customer password

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/parking", "GET")]
    public class ParkingRequest
    {
        public string LocationName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartDate { get; set; }
        public string Initials { get; set; }
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
            ParkingAvailabilityResponseDto response;

            try
            {
                var dto = new ParkingAvailabilityRequestDto();

                dto.Passenger.Initials = request.Initials;
                dto.Location.ArrivalDate = request.ArrivalDate;
                dto.Location.DepartDate = request.DepartDate;
                dto.Location.LocationName = request.LocationName;

                response = _parkingService.AvailabilityAtDestination(dto);
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
