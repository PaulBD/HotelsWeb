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
        public List<AutocompleteDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            return _couchbaseHelper.ReturnQuery<AutocompleteDto>("SELECT * FROM TriperooCommon WHERE letterIndex = '" + searchValue.Substring(0, 3) + "'", "TriperooCommon");
        }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public List<LocationDto> ReturnLocationById(string locationId)
        {
            return _couchbaseHelper.ReturnQuery<LocationDto>("SELECT * FROM TriperooCommon WHERE reference = '" + locationId + "'", "TriperooCommon");
        }
    }
}
