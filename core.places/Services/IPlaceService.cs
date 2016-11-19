using core.places.dtos;

namespace core.places.services
{
    public interface IPlaceService
    {
        PlaceDto ReturnPlacesByTownAndCountry(string town, string country, string type, int offset, int limit);

        PlaceDto ReturnPlacesByProximity(double longitude, double latitude, int radius, string type, int offset, int limit);

        DataDto ReturnPlaceById(string factualId);
    }
}
