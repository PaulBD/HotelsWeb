using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.foursquare.services;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
        private string _query;
        private VenueService _venueService;

        public LocationService()
        {
            _venueService = new VenueService();
            _couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats FROM " + _bucketName;
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            var q = _query + " WHERE letterIndex = '" + searchValue.Substring(0, 3) + "' AND regionType != 'Neighborhood'  AND regionType != 'Point of Interest' AND regionType != 'Point of Interest Shadow' ORDER BY searchPriority DESC";

            return ProcessQuery(q);
       }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public LocationDto ReturnLocationById(int locationId)
        {
            var q = _query + " WHERE regionID = " + locationId;

            return ProcessQuery(q)[0];
        }

        /// <summary>
        /// Return a child locations by parent Id
        /// </summary>
        public List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type)
        {
            var q = _query + " WHERE parentRegionID = " + parentLocationId;

            return ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<LocationDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<LocationDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Update Location
        /// </summary>
        public void UpdateLocation(string reference, LocationDto dto)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, dto, _bucketName);
        }
    }
}
