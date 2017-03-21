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
            _query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, summary.en as summary, url FROM " + _bucketName;
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue)
        {
            var q = _query + " WHERE letterIndex = '" + searchValue.Substring(0, 3) + "' AND regionType != 'Neighborhood' ORDER BY SearchPriority ASC";

            return ProcessQuery(q, 0, 0);
       }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public LocationDto ReturnLocationById(int locationId)
        {
            var q = _query + " WHERE regionID = " + locationId;

            return ProcessQuery(q, 0, 0)[0];
        }

        /// <summary>
        /// Return a child locations by parent Id
        /// </summary>
        public List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type, int offset, int limit)
        {
            var q = _query + " WHERE parentRegionID = " + parentLocationId;

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

        /// <summary>
        /// Update Location
        /// </summary>
        public void UpdateLocation(string reference, LocationDto dto)
        {
            var updatedLocation = new AutocompleteDto()
            {
                Doctype = dto.Doctype,
                ListingPriority = dto.ListingPriority,
                ParentRegionID = dto.ParentRegionID,
                ParentRegionName = dto.ParentRegionName,
                ParentRegionNameLong = dto.ParentRegionNameLong,
                ParentRegionType = dto.ParentRegionType,
                RegionID = dto.RegionID,
                RegionName = dto.RegionName,
                RegionNameLong = dto.RegionNameLong,
                RegionType = dto.RegionType,
                RelativeSignificance = dto.RelativeSignificance,
                SubClass = dto.SubClass,
                Summary = new Summary
                {
                    En = dto.Summary
                },
                Stats = new Stats
                {
                    AverageReviewScore = dto.AverageReviewScore,
                    LikeCount = dto.LikeCount,
                    ReviewCount = dto.ReviewCount
                },
                LocationCoordinates = new LocationCoordinates
                {
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude
                }
            };

            _couchbaseHelper.AddRecordToCouchbase(reference, updatedLocation, _bucketName);
        }
    }
}
