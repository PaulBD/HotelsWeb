using core.places.dtos;
using System.Collections.Generic;

namespace core.places.services
{
    public interface ILocationService
    {
        List<LocationDto> ReturnLocationById(string locationId);

        List<AutocompleteDto> ReturnLocationsForAutocomplete(string searchValue);
    }
}
