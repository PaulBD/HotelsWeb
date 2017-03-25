using library.weather.dtos;

namespace library.weather.services
{
    public interface IWeatherService
    {
        WeatherDto ReturnWeatherByLocation(double latitude, double longitude, string language);
    }
}
