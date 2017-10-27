using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.foursquare.services;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using library.caching;

namespace core.places.services
{
    public class RestaurantService : BaseService, IRestaurantService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
		private string _query;
		private string _foodTypeQuery;
		private ILocationService _loctionService;
        private IVenueService _venueService;
        private library.zomato.services.IRestaurantService _restaurantService;
		private readonly ICacheProvider _cache;

		public RestaurantService(ICacheProvider cache)
		{
			_cache = cache;

            _couchbaseHelper = new CouchBaseHelper();
            _loctionService = new LocationService();
            _venueService = new VenueService();
            _restaurantService = new library.zomato.services.RestaurantService();
            _query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats, locationDetail FROM " + _bucketName;
            _foodTypeQuery = "SELECT subClass as CategoryNameFriendly FROM " + _bucketName;
		}

        public List<CategoryDto> ReturnCategoryList(int parentLocationId)
		{
			var q = _foodTypeQuery + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Restaurant' AND subClass != '' GROUP BY subClass ORDER BY subClass";
			
            var result = _couchbaseHelper.ReturnQuery<CategoryDto>(q, _bucketName);

			if (result.Count > 0)
			{
				return result;
			}

			return null;
        }

        /// <summary>
        /// Return restaurants by location Id
        /// </summary>
        public LocationListDto ReturnRestaurantsByParentId(int parentLocationId)
        {
            var cacheKey = "Restaurants:" + parentLocationId;

            var restaurantList = _cache.Get<LocationListDto>(cacheKey);

            if (restaurantList != null)
            {
                return restaurantList;
            }

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Restaurant'";

			var list =  ProcessQuery(q);


            if (list.Locations.Count == 0)
            {
                list.Locations = PopulateZomatoRestaurants(34.052235, -118.243683);
            }

			if (list != null)
			{
				_cache.AddOrUpdate(cacheKey, list);
			}

            return list;
        }

        /// <summary>
        /// Return restaurants by location Id and category
        /// </summary>
        public LocationListDto ReturnRestaurantsByParentIdAndCategory(int parentLocationId, string category)
		{
			var cacheKey = "Restaurants:" + parentLocationId + category;

			var restaurantList = _cache.Get<LocationListDto>(cacheKey);

			if (restaurantList != null)
			{
				return restaurantList;
			}

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Restaurant'";

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
            else {
                q += " AND LOWER(subClass) = '" + category.Replace("-and-", " & ").Replace("-", " ") + "'";
            }

			var list = ProcessQuery(q);

			if (list.Locations.Count == 0)
            {
                list.Locations = PopulateZomatoRestaurants(34.052235, -118.243683);
			}

			if (list != null)
			{
				_cache.AddOrUpdate(cacheKey, list);
			}

			return list;
        }

        private List<LocationDto> PopulateZomatoRestaurants(double latitude, double longitude)
        {
            var location = new List<LocationDto>();
            var result = _restaurantService.ReturnRestaurantByLocation(latitude, longitude);
            foreach(var restaurant in result.nearby_restaurants)
            {
                var reference = "Zom:" + restaurant.restaurant.id;
                var dto = new LocationDto{
                   
                };

                location.Add(dto);
                _couchbaseHelper.AddRecordToCouchbase(reference, dto, _bucketName);
            }

            return null;
        }

        private List<LocationDto> PopulateRestaurants(int parentLocationId, string category)
        {
            List<LocationDto> result = new List<LocationDto>();

            var location = _loctionService.ReturnLocationById(parentLocationId);

            if (category == null)
            {
                category = "Restaurants";
            }

            if (location != null)
            {
                var venueResponse = _venueService.ReturnVenuesByLocation(category, location.RegionNameLong);

                foreach (var venue in venueResponse.Response.Venues)
                {
                    string id = "666" + new string(venue.Id.Where(Char.IsDigit).ToArray()).Substring(0, 4);

                    if (venue.Categories.Where(q => q.PluralName.ToLower().Contains("restaurant") || q.PluralName.ToLower().Contains("steak") || q.PluralName.ToLower().Contains("diner") || q.PluralName.ToLower().Contains("pizza") || q.PluralName.ToLower().Contains("burger")).Count() > 0)
                    {

                        string vanueCategory = "unknown";

                        if (venue.Categories.Count > 0)
                        {
                            vanueCategory = venue.Categories[0].ShortName.ToLower();
                        }

                        LocationDto dto = new LocationDto
                        {
                            Doctype = "Foresquare",
                            RegionID = int.Parse(id),
                            RegionType = "Restaurant Shadow",
                            SubClass = vanueCategory,
                            RegionName = Regex.Replace(venue.Name, @"[^0-9a-zA-Z]'", " "),
                            RegionNameLong = venue.Name + ", " + venue.Location.Country,
                            ParentRegionID = parentLocationId,
                            ParentRegionName = location.RegionName,
                            ParentRegionType = location.RegionType,
                            ParentRegionNameLong = location.RegionNameLong,
                            LetterIndex = venue.Name.ToLower().Substring(0, 3),
                            LocationCoordinates = new LocationCoordinatesDto { Latitude = venue.Location.Lat, Longitude = venue.Location.Lng },
                            Url = "/" + id + "/visit-location/" + StripValues(venue.Name).ToLower(),
                            SourceData = new SourceData { ForesquareId = venue.Id },
                            ContactDetails = new ContactDetails
                            {
                                facebook = venue.Contact.Facebook,
                                facebookName = venue.Contact.FacebookName,
                                facebookUsername = venue.Contact.FacebookUsername,
                                formattedPhone = venue.Contact.FormattedPhone,
                                instagram = venue.Contact.Instagram,
                                phone = venue.Contact.Phone,
                                twitter = venue.Contact.Twitter
                            },
                            FormattedAddress = venue.Location.FormattedAddress,
                            Stats = new StatsDto() { AverageReviewScore = 0, LikeCount = 0, ReviewCount = 0 }
                        };

                        foreach (var cat in venue.Categories)
                        {
                            dto.Tags.Add(cat.ShortName);
                        }


						dto = _loctionService.AttachPhotos(venue.Id, dto);

                        //_couchbaseHelper.AddRecordToCouchbase("Rest-FQ-:" + id, dto, _bucketName);

                        result.Add(dto);
                    }
                }
            }

            return result;
        }

        private string StripValues(string name)
		{
			name = name.Replace(" ", "-");
			name = name.Replace("&", "and");
			name = name.Replace("'", "");
			name = Regex.Replace(name, @"[^0-9a-zA-Z]-", "");

            return name.ToLower();
        }
    }
}
