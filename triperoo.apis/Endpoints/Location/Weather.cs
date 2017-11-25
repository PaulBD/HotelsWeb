using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using library.weather.services;
using library.weather.dtos;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.location
{
	#region Return Weather By Location Id

	/// <summary>
	/// Request
	/// </summary>
    [Route("/v1/location/{id}/weather", "GET")]
    public class WeatherRequest
    {
        public int Id { get; set; }
        public bool IsCity { get; set; }
        public string Lang { get; set; }

    }

    /// <summary>
    /// Validator
    /// </summary>
    public class WeatherRequestValidator : AbstractValidator<WeatherRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WeatherRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).GreaterThan(0).WithMessage("Invalid location id have been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class WeatherApi : Service
    {
		private readonly IWeatherService _weatherService;
		private readonly ILocationService _locationService;

		/// <summary>
		/// Constructor
		/// </summary>
		public WeatherApi(IWeatherService weatherService, ILocationService locationService)
		{
			_locationService = locationService;
            _weatherService = weatherService;
        }

        #region List Weather by Id

        /// <summary>
        /// Lists Weather by Id
        /// </summary>
        public object Get(WeatherRequest request)
		{
			LocationDto locationResponse = new LocationDto();
            WeatherDto response = new WeatherDto();
			string cacheName = "weather:" + request.Id;
			string locationCacheName = "location:" + request.Id;

            try
			{
				locationResponse = _locationService.ReturnLocationById(request.Id, request.IsCity);

                response = Cache.Get<WeatherDto>(cacheName);

                if (response == null)
                {
                    response = _weatherService.ReturnWeatherByLocation(Convert.ToDouble(locationResponse.LocationCoordinates.Latitude), Convert.ToDouble(locationResponse.LocationCoordinates.Longitude), request.Lang);
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
