using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using api.laterooms.co.uk.Services;
using dtos.laterooms.co.uk;
using api.laterooms.co.uk.Enums;

namespace api.hotels.co.uk.EndPoints
{
    #region Hotel

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v3/hotel/live/{id}", "GET")]
    public class PriceRequest
    {
        public string Id { get; set; }
        public LateroomsCurrency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public int Nights { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class PriceRequestValidator : AbstractValidator<PriceRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PriceRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () => RuleFor(r => r.Id).NotEmpty().WithMessage("Invalid hotel id have been supplied"));

        }
    }

    #endregion

    public class PricesApi : Service
    {
        // Dependencies
        private readonly ILateRoomsApiService _lateRoomsApiService;

        /// <summary>
        /// Constructor
        /// </summary>
        public PricesApi(ILateRoomsApiService lateRoomsApiService)
        {
            _lateRoomsApiService = lateRoomsApiService;
        }

        /// <summary>
        /// Get prices for a single hotel
        /// </summary>
        public object Get(PriceRequest request)
        {
            LivePricesDto response;

            try
            {
                response = _lateRoomsApiService.SearchLivePrices(request.Id, request.Currency, request.StartDate.ToShortDateString(), request.Nights);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }
    }
}
