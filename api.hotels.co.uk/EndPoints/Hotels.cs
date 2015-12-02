using System;
using System.Collections.Generic;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using api.laterooms.co.uk.Services;
using dtos.laterooms.co.uk;
using api.laterooms.co.uk.Enums;

namespace api.hotels.co.uk.EndPoints
{
    #region Hotels

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v3/hotels/")]
    public class HotelsRequest
    {
        public string Keyword { get; set; }
        public LateroomsCurrency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public int Nights { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelsRequestValidator : AbstractValidator<HotelsRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelsRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(r => r.Keyword).NotNull().WithMessage("Supply a valid keyword");
                                     RuleFor(r => r.Currency).NotEmpty().WithMessage("Invalid currency has been supplied");
                                     RuleFor(r => r.StartDate).NotEmpty().WithMessage("Invalid start date has been supplied");
                                     RuleFor(r => r.Nights).NotEmpty().WithMessage("Invalid number of nights has been supplied");
                                 });
        }
    }

    #endregion

    #region Hotel

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v3/hotel/", "GET")]
    public class HotelRequest
    {
        public List<int> Ids { get; set; }
        public LateroomsCurrency Currency { get; set; }
        public DateTime StartDate { get; set; }
        public int Nights { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelRequestValidator : AbstractValidator<HotelRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
                                 {

                                     RuleFor(r => r.Ids).NotEmpty().WithMessage("Invalid hotel id have been supplied");
                                     RuleFor(r => r.Currency).NotEmpty().WithMessage("Invalid currency has been supplied");
                                     RuleFor(r => r.StartDate).NotEmpty().WithMessage("Invalid start date has been supplied");
                                     RuleFor(r => r.Nights).NotEmpty().WithMessage("Invalid number of nights has been supplied");
                                 });

        }
    }

    #endregion

    public class HotelsApi : Service
    {
        // Dependencies
        private readonly ILateRoomsApiService _lateRoomsApiService;

        /// <summary>
        /// Constructor
        /// </summary>
        public HotelsApi(ILateRoomsApiService lateRoomsApiService)
        {
            _lateRoomsApiService = lateRoomsApiService;
        }

        /// <summary>
        /// Lists all hotels by keyword
        /// </summary>
        public object Get(HotelsRequest request)
        {
            LateRoomsDto response;

            try
            {
                response = _lateRoomsApiService.SearchHotelsByKeyword(request.Keyword, request.Currency, request.StartDate.ToShortDateString(), request.Nights);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Get a single hotel
        /// </summary>
        public object Get(HotelRequest request)
        {
            LateRoomsDto response;

            try
            {
                response = _lateRoomsApiService.SearchSelectedHotels(request.Ids, request.Currency, request.StartDate.ToShortDateString(), request.Nights);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message, ex.InnerException);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }
    }
}
