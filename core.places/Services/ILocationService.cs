using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface ILocationService
    {
        LocationDto ReturnLocationById(int locationId);
        List<LocationDto> ReturnLocationsForAutocomplete(string searchValue);
        List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type, int offset, int limit);
        void UpdateLocation(string reference, LocationDto dto);
    }
}
