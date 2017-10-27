using System;
using core.places.dtos;
using library.caching;

namespace core.places.services
{
    public class ContentService : BaseService, IContentService
    {
        private string _query;
        private readonly ICacheProvider _cache;
        private readonly string _bucketName = "TriperooCommon";
        private readonly ILocationService _locationService;

        public ContentService(ICacheProvider cache, ILocationService locationService)
        {
            _cache = cache;
            _locationService = locationService;
            _query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats, locationDetail FROM " + _bucketName;
        }

        /// <summary>
        /// Returns the content by parent region identifier.
        /// </summary>
        public LocationListDto ReturnContentByParentRegionId(int parentLocationId, string contentType)
        {
            var cacheKey = contentType + ":" + parentLocationId;

            var attractionsList = _cache.Get<LocationListDto>(cacheKey);

            if (attractionsList != null)
            {
                return attractionsList;
            }

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND LOWER(regionType) = '" + contentType.ToLower().Replace("-", " ") + "' ORDER BY Rank";

            var list = ProcessQuery(q);

            if (list != null)
            {
                _cache.AddOrUpdate(cacheKey, list);
            }

            return list;
        }


        /// <summary>
        /// Return content by location Id and category
        /// </summary>
        public LocationListDto ReturnContentByParentIdAndCategory(int parentLocationId, string contentType, string category)
        {
            var cacheKey = "POI:" + parentLocationId + category;

            var attractionsList = _cache.Get<LocationListDto>(cacheKey);

            if (attractionsList != null)
            {
                return attractionsList;
            }

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = '" + contentType + "' ";

            if (category.Contains(","))
            {
                string[] cat = category.Split(',');

                q += " AND (";
                for (var i = 0; i < cat.Length; i++)
                {
                    q += "LOWER(subClass) = '" + cat[i].Replace("-and-", " & ").Replace("-", " ") + "'";

                    if (i < cat.Length - 1)
                    {
                        q += " OR ";
                    }
                }

                q += ")";
            }
            else
            {
                q += " AND LOWER(subClass) = '" + category.Replace("-and-", " & ").Replace("-", " ") + "'";
            }

            var list = ProcessQuery(q);

            if (list != null)
            {
                _cache.AddOrUpdate(cacheKey, list);
            }

            return list;
        }
    }
}
