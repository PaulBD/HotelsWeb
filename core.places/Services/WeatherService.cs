using core.places.dtos;

namespace core.places.services
{
    public class WeatherService : IWeatherService
    {
        /// <summary>
        /// Return a weather by Id
        /// </summary>
        public WeatherDto ReturnWeatherById(int locationId)
        {
            return new WeatherDto()
            {
                Currently = new CurrentlyDto()
                {
                    ApparentTemperature = 1,
                    CloudCover = 0.73,
                    DewPoint = 1,
                    Humidity = 1,
                    Icon = "sun",
                    Summary = "Sun",
                    NearestStormDistance = 0,
                    Ozone = 0,
                    PrecipIntensity = 0,
                    PrecipIntensityError = 0,
                    PrecipProbability = 0,
                    PrecipType = "rain",
                    Pressure = 0,
                    Temperature = 36,
                    Visibility = 0,
                    WindBearing = 0,
                    WindSpeed = 0,
                    Time = 1453402675
                },
                Latitude = 47.20296790272209,
                Longitude = -123.41670367098749,
                Timezone = "America/Los_Angeles"
            };
        }
    }
}
