using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface ILocationService
    {
        LocationDto ReturnLocationById(int locationId);
        List<LocationDto> ReturnLocationsForAutocomplete(string searchValue);
        List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type);
        void UpdateLocation(string reference, LocationDto dto, bool isStaging);
    }
}
