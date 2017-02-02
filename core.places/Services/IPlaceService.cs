using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface IPlaceService
    {
        List<FactualDto> ReturnPlacesByTownAndCountry(string town, string country, string type, int offset, int limit);

        FactualDto ReturnPlacesByProximity(double longitude, double latitude, int radius, string type, int offset, int limit);

        PlaceDto ReturnPlaceById(string id, string type);
    }
}
