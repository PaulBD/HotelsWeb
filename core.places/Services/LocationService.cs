using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";

        public LocationService()
        {
            _couchbaseHelper = new CouchBaseHelper();
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            var q = "SELECT regionID as inventoryReference, autocompleteName as name, regionName as nameShort, autocompleteSearch as search, autocompleteUrl as url, autocompleteType as type, autocompletePriority as priority FROM TriperooCommon WHERE autocompleteSearch = '" + searchValue.Substring(0, 3) + "'";

            return ProcessQuery(q, 0, 0);
       }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public LocationDto ReturnLocationById(int locationId)
        {
            var q = "SELECT regionID as inventoryReference, autocompleteName as name, regionName as nameShort, autocompleteUrl as url, autocompleteType as type, regionImage as image FROM TriperooCommon WHERE inventoryReference = " + locationId;

            return ProcessQuery(q, 0, 0)[0];
        }

        /// <summary>
        /// Return a child locations by parent Id
        /// </summary>
        public List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type, int offset, int limit)
        {
            var q = "SELECT regionID as inventoryReference, autocompleteName as name, regionName as nameShort, autocompleteUrl as url, autocompleteType as type, regionImage as image FROM TriperooCommon WHERE parentRegionID = " + parentLocationId;

            return ProcessQuery(q, limit, offset);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<LocationDto> ProcessQuery(string q, int limit, int offset)
        {
            if (limit > 0)
            {
                q += " LIMIT " + limit + " OFFSET " + offset;
            }

            var result = _couchbaseHelper.ReturnQuery<LocationDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }
    }
}
