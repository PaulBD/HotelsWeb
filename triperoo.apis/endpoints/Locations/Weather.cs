using System;
using System.Net;
using ServiceStack;
using ServiceStack.FluentValidation;
using core.places.services;
using core.places.dtos;

namespace triperoo.apis.endpoints.locations
{

    #region Return a Weather by location id

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/weather", "GET")]
    public class WeatherRequest
    {
        public int id { get; set; }

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
                RuleFor(r => r.id).GreaterThan(0).WithMessage("Invalid location id have been supplied");
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
            string cacheName = "places:" + request.id + ":weather";

            try
            {
                response = Cache.Get<WeatherDto>(cacheName);

                if (response == null)
                {
                    response = _weatherService.ReturnWeatherById(request.id);
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
