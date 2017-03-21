using core.places.dtos;

namespace core.places.services
{
    public interface IWeatherService
    {
        WeatherDto ReturnWeatherById(int locationId);
    }
}
