using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using library.weather.services;
using library.weather.dtos;

namespace triperoo.apis.endpoints.locations
{

    #region Return a Weather by location id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/weather", "GET")]
    public class WeatherRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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
                RuleFor(r => r.Latitude).NotNull().WithMessage("Invalid latitude has been supplied");
                RuleFor(r => r.Longitude).NotNull().WithMessage("Invalid longitude has been supplied");
            });

        }
    }

    #endregion

    #region API logic

    public class WeatherApi : Service
    {
        private readonly IWeatherService _weatherService;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeatherApi(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        #region List Weather by Id

        /// <summary>
        /// Lists Weather by Id
        /// </summary>
        public object Get(WeatherRequest request)
        {
            WeatherDto response = new WeatherDto();
            string cacheName = "places:" + request.Latitude + "," + request.Longitude + ":weather";

            try
            {
                response = Cache.Get<WeatherDto>(cacheName);

                if (response == null)
                {
                    response = _weatherService.ReturnWeatherByLocation(request.Latitude, request.Longitude, request.Lang);
                    base.Cache.Add(cacheName, response);
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
