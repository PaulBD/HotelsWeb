using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;

        public LocationService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            return _couchbaseHelper.ReturnQuery<LocationDto>("SELECT regionID as inventoryReference, autocompleteName as name, regionName as nameShort, autocompleteSearch as search, autocompleteUrl as url, autocompleteType as type, autocompletePriority as priority FROM TriperooCommon WHERE autocompleteSearch = '" + searchValue.Substring(0, 3) + "'", "TriperooCommon");
        }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public List<LocationDto> ReturnLocationById(int locationId)
        {
            return _couchbaseHelper.ReturnQuery<LocationDto>("SELECT regionID as inventoryReference, autocompleteName as name, regionName as nameShort, autocompleteUrl as url, autocompleteType as type, regionImage as image FROM TriperooCommon WHERE inventoryReference = " + locationId, "TriperooCommon");
        }
    }
}
