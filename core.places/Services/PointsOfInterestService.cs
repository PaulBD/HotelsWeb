using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.caching;

namespace core.places.services
{
    public class PointOfInterestService : BaseService, IPointOfInterestService
    {
        private readonly string _bucketName = "TriperooCommon";
		private string _query;
		private readonly ICacheProvider _cache;

        public PointOfInterestService(ICacheProvider cache)
		{
			_cache = cache;
			_query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats FROM " + _bucketName;
		}

        /// <summary>
        /// Return point of interests by location Id
        /// </summary>
        public LocationListDto ReturnPointOfInterestsByParentId(int parentLocationId)
		{
			var cacheKey = "POI:" + parentLocationId;

			var attractionsList = _cache.Get<LocationListDto>(cacheKey);

			if (attractionsList != null)
			{
				return attractionsList;
			}

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Point of Interest Shadow'";

            var list = ProcessQuery(q);

			if (list != null)
			{
				_cache.AddOrUpdate(cacheKey, list);
			}

			return list;
		}

        /// <summary>
        /// Return point of interests by location Id and category
        /// </summary>
        public LocationListDto ReturnPointOfInterestsByParentIdAndCategory(int parentLocationId, string category)
		{
			var cacheKey = "POI:" + parentLocationId + category;

			var attractionsList = _cache.Get<LocationListDto>(cacheKey);

			if (attractionsList != null)
			{
				return attractionsList;
			}

			var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Point of Interest Shadow' ";

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
