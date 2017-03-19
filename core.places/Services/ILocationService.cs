using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface ILocationService
    {
        List<LocationDto> ReturnLocationById(int locationId);

        List<LocationDto> ReturnLocationsForAutocomplete(string searchValue);
    }
}
