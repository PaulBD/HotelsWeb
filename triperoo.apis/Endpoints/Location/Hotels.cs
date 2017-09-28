﻿using System;
using System.Collections.Generic;
using System.Net;
using core.hotels.dtos;
using core.hotels.services;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace triperoo.apis.endpoints.location
{
	#region Hotels By Location Id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/{id}/hotels", "POST")]
	public class HotelLocationRequest
	{
		public int Id { get; set; }
		public string Locale { get; set; }
		public string CurrencyCode { get; set; }
		public string Location { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Radius { get; set; }
		public List<int> PropertyCategory { get; set; }
		public float MinRate { get; set; }
		public float MaxRate { get; set; }
		public int MinStarRating { get; set; }
		public int MaxStarRating { get; set; }
		public int MinTripAdvisorRating { get; set; }
		public int MaxTripAdvisorRating { get; set; }

		public bool CheckDates { get; set; }
		public DateTime ArrivalDate { get; set; }
		public int Nights { get; set; }

		public string Rooms1 { get; set; }
		public string Rooms2 { get; set; }
		public string Rooms3 { get; set; }

		public List<string> FacilityList { get; set; }
		public string SortBy { get; set; }
		public int Exclude { get; set; }

		public int PageSize { get; set; }
		public int PageNumber { get; set; }

    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelLocationRequestValidator : AbstractValidator<HotelLocationRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelLocationRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
				RuleFor(r => r.Id).NotNull().WithMessage("Supply a valid location id");
				RuleFor(r => r.ArrivalDate).NotNull().WithMessage("Supply a valid arrival date");
				RuleFor(r => r.Nights).NotNull().WithMessage("Supply a valid number of nights");
				RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
				RuleFor(r => r.CurrencyCode).NotNull().WithMessage("Supply a valid currency code");
            });
        }
    }

	#endregion

	#region Hotel By id

	/// <summary>
	/// Request Hotel By id
	/// </summary>
	[Route("/v1/location/{id}/hotel/{hotelId}", "GET")]
	public class HotelDetailRequest
	{
		public int Id { get; set; }
		public int HotelId { get; set; }
		public string Locale { get; set; }
		public string CurrencyCode { get; set; }
	}

	/// <summary>
	/// Validator
	/// </summary>
	public class HotelDetailRequestValidator : AbstractValidator<HotelDetailRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public HotelDetailRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).NotNull().WithMessage("Supply a valid location id");
				RuleFor(r => r.HotelId).NotNull().WithMessage("Supply a valid hotel id");
				RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
				RuleFor(r => r.CurrencyCode).NotNull().WithMessage("Supply a valid currency code");
			});
		}
	}

	#endregion

	#region Hotel Room Availability By id

	/// <summary>
	/// Request Hotel By id
	/// </summary>
	[Route("/v1/location/{id}/hotel/{hotelId}/rooms/{arrivalDate}/{nights}", "GET")]
	public class HotelRoomAvailabilityRequest
	{
		public int Id { get; set; }
		public int HotelId { get; set; }
		public DateTime ArrivalDate { get; set; }
		public int Nights { get; set; }
		public string Locale { get; set; }
		public string CurrencyCode { get; set; }
		public string Room1 { get; set; }
		public string Room2 { get; set; }
		public string Room3 { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
	}

	/// <summary>
	/// Validator
	/// </summary>
	public class HotelRoomAvailabilityRequestValidator : AbstractValidator<HotelRoomAvailabilityRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public HotelRoomAvailabilityRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).NotNull().WithMessage("Supply a valid location id");
				RuleFor(r => r.HotelId).NotNull().WithMessage("Supply a valid hotel id");
				RuleFor(r => r.ArrivalDate).NotNull().WithMessage("Supply a valid arrival date");
				RuleFor(r => r.Nights).NotNull().WithMessage("Supply a valid number of nights");
				RuleFor(r => r.Locale).NotNull().WithMessage("Supply a valid locale");
				RuleFor(r => r.CurrencyCode).NotNull().WithMessage("Supply a valid currency code");
			});
		}
	}

	#endregion

	#region API logic

	public class HotelRequestApi : Service
	{
		private readonly IHotelService _hotelService;

		/// <summary>
		/// Constructor
		/// </summary>
		public HotelRequestApi(IHotelService hotelService)
		{
			_hotelService = hotelService;
		}

        #region Get Hotels By Location

        /// <summary>
        /// Get Hotels By location
        /// </summary>
        public object Post(HotelLocationRequest request)
		{
			var response = new HotelAPIListDto();

            try
            {
                if (request.Latitude == 0 && request.Longitude == 0 && request.Radius == 0)
                {
                    response = _hotelService.ReturnHotelsByLocationId(request.Locale, request.CurrencyCode, request.Location, request.PropertyCategory, request.MinRate, request.MaxRate, request.MinStarRating, request.MaxStarRating, request.MinTripAdvisorRating, request.MaxTripAdvisorRating, request.CheckDates, request.ArrivalDate, request.Nights, request.Rooms1, request.Rooms2, request.Rooms3, request.SortBy, request.Exclude);
                }
                else {
					response = _hotelService.ReturnHotelsByProximity(request.Locale, request.CurrencyCode, request.Longitude, request.Latitude, request.Radius, request.PropertyCategory, request.MinRate, request.MaxRate, request.MinStarRating, request.MaxStarRating, request.MinTripAdvisorRating, request.MaxTripAdvisorRating, request.CheckDates, request.ArrivalDate, request.Nights, request.Rooms1, request.Rooms2, request.Rooms3, request.SortBy, request.Exclude);
				}
            }
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Get Hotel Room Availability By Location

		/// <summary>
		/// Get Hotel Room Availability By Location
		/// </summary>
		public object Get(HotelRoomAvailabilityRequest request)
		{
			var response = new RoomAvailabilityDto();

			try
			{
				response = _hotelService.ReturnRoomAvailability(request.HotelId, request.Locale, request.CurrencyCode, request.ArrivalDate, request.Nights, request.Room1, request.Room2, request.Room3);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

        #region Get Hotel By Id

        /// <summary>
        /// Get Hotel By Id
        /// </summary>
        public object Get(HotelDetailRequest request)
		{
			string cacheName = "hotel:" + request.Id;
			HotelDto response;

			try
			{
				response = Cache.Get<HotelDto>(cacheName);

                if (response == null)
                {
                    response = new HotelDto();
                    response = _hotelService.ReturnHotelById(request.HotelId, request.Locale, request.CurrencyCode);
                    //TODO: Add to cache
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
