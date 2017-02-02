using System;
using System.Net;
using ServiceStack.FluentValidation;
using core.hotels.services;
using core.hotels.dtos;
using core.hotels.enums;
using ServiceStack;
using System.Collections.Generic;

namespace triperoo.apis.endpoints.hotels
{
    #region Return hotel by town & country

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/hotels/{Country}/{Town}")]
    [Route("/v1/hotels/{Country}/{Town}/{Offset}/{Limit}")]
    public class HotelsByTownRequest
    {
        public string Town { get; set; }
        public string Country { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelsRequestValidator : AbstractValidator<HotelsByTownRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelsRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.Town).NotNull().WithMessage("Please supply a valid town");
                RuleFor(r => r.Country).NotNull().WithMessage("Please supply a valid country");
            });
        }
    }

    #endregion

    #region Return hotel by town & country

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/hotels/{PlaceId}")]
    [Route("/v1/hotels/{PlaceId}/{Offset}/{Limit}")]
    public class HotelsByTownIdRequest
    {
        public int PlaceId { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelsByTownIdRequestValidator : AbstractValidator<HotelsByTownIdRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelsByTownIdRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
                RuleFor(r => r.PlaceId).NotNull().WithMessage("Please supply a valid place id");
            });
        }
    }

    #endregion

    #region Return a single hotel by id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/hotel/{id}", "GET")]
    public class HotelByIdRequest
    {
        public string Id { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelRequestValidator : AbstractValidator<HotelByIdRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>  { RuleFor(r => r.Id).NotEmpty().WithMessage("Invalid hotel id have been supplied"); });
        }
    }

    #endregion

    #region Return a hotels price by id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/hotel/{id}/prices/{Currency}/{Nights}/{StartDate}", "GET")]
    public class HotelPriceByIdRequest
    {
        public string Id { get; set; }
        public Currency Currency { get; set; }
        public string StartDate { get; set; }
        public int Nights { get; set; }
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelPriceRequestValidator : AbstractValidator<HotelPriceByIdRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelPriceRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () => { RuleFor(r => r.Id).NotEmpty().WithMessage("Invalid hotel id have been supplied"); });
        }
    }

    #endregion

    #region API logic

    public class HotelsApi : Service
    {
        // Dependencies
        private readonly IHotelService _hotelService;
        private readonly IHotelPriceService _hotelPriceService;

        /// <summary>
        /// Constructor
        /// </summary>
        public HotelsApi(IHotelService hotelService, IHotelPriceService hotelPriceService)
        {
            _hotelService = hotelService;
            _hotelPriceService = hotelPriceService;
        }

        /// <summary>
        /// Return all hotels by town & country
        /// </summary>
        public object Get(HotelsByTownRequest request)
        {
            List<HotelDto> response = null;
            string cacheName = request.Town + "-" + request.Country;

            try
            {
                response = Cache.Get<List<HotelDto>>(cacheName);

                if (response == null)
                {
                    response = _hotelService.ReturnHotelsByTown(request.Town, request.Country, request.Limit, request.Offset);
                    base.Cache.Add(cacheName, response);
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Return all hotels by town id
        /// </summary>
        public object Get(HotelsByTownIdRequest request)
        {
            List<HotelDto> response = null;
            string cacheName = "hotel:city:" + request.PlaceId;

            try
            {
                response = Cache.Get<List<HotelDto>>(cacheName);

                if (response == null)
                {
                    response = _hotelService.ReturnHotelsByPlaceId(request.PlaceId, request.Limit, request.Offset);
                    base.Cache.Add(cacheName, response);
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Return a single hotel
        /// </summary>
        public object Get(HotelByIdRequest request)
        {
            HotelDetailDto response = null;
            string cacheName = "hotel:" + request.Id;

            try
            {
                response = Cache.Get<HotelDetailDto>(cacheName);

                if (response == null)
                {
                    response = _hotelService.ReturnHotelById(request.Id);
                    base.Cache.Add(cacheName, response);
                }
            }

            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        /// <summary>
        /// Return a hotels price by id
        /// </summary>
        public object Get(HotelPriceByIdRequest request)
        {
            HotelPriceDto response = null;

            try
            {
                response = _hotelPriceService.ReturnHotelPrice(request.Id, request.Currency, request.StartDate, request.Nights);
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
