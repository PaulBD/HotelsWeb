using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
        private string _query;

        public LocationService()
        {
            _couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT Doctype, Image, LetterIndex, ListingPriority, LocationCoordinates.Latitude, LocationCoordinates.Longitude, ParentRegionID, ParentRegionName, ParentRegionNameLong, ParentRegionType, RegionID, RegionName, RegionNameLong, RegionType, RelativeSignificance, SearchPriority, Stats.AverageReviewScore, Stats.LikeCount, Stats.ReviewCount, SubClass, Summary.En as Summary, Url  FROM " + _bucketName;
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            var q = _query + " WHERE LetterIndex = '" + searchValue.Substring(0, 3) + "' AND RegionType != 'Neighborhood' ORDER BY SearchPriority ASC";

            return ProcessQuery(q, 0, 0);
       }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public LocationDto ReturnLocationById(int locationId)
        {
            var q = _query + " WHERE RegionID = " + locationId;

            return ProcessQuery(q, 0, 0)[0];
        }

        /// <summary>
        /// Return a child locations by parent Id
        /// </summary>
        public List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type, int offset, int limit)
        {
            var q = _query + " WHERE ParentRegionID = " + parentLocationId;

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
