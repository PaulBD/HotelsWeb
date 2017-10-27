using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using library.foursquare.services;
using System.Linq;
using System;
using library.caching;

namespace core.places.services
{
    public class NightlifeService : BaseService, INightlifeService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
		private string _query;
		private ILocationService _loctionService;
		private IVenueService _venueService;
		private readonly ICacheProvider _cache;

        public NightlifeService(ICacheProvider cache)
		{
			_cache = cache;

			_couchbaseHelper = new CouchBaseHelper();
			_loctionService = new LocationService();
			_venueService = new VenueService();
            _query = "SELECT photos, doctype, image, letterIndex, listingPriority, locationCoordinates as locationCoordinates, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, summary.en as summary, url FROM " + _bucketName;
        }

        /// <summary>
        /// Return nightlife by location Id
        /// </summary>
        public LocationListDto ReturnNightlifeByParentId(int parentLocationId)
		{
			var cacheKey = "Nightlife:" + parentLocationId;

			var nightlifeList = _cache.Get<LocationListDto>(cacheKey);

			if (nightlifeList != null)
			{
				return nightlifeList;
			}

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Nightlife Shadow'";

			var list = ProcessQuery(q);

			if (list == null)
			{
				list.Locations = PopulateNightlife(parentLocationId, null);
			}

			if (list != null)
			{
				_cache.AddOrUpdate(cacheKey, list);
			}

			return list;
        }

        /// <summary>
        /// Return nightlife by location Id and category
        /// </summary>
        public LocationListDto ReturnNightlifeByParentIdAndCategory(int parentLocationId, string category)
		{
			var cacheKey = "Nightlife:" + parentLocationId + category;

			var nightlifeList = _cache.Get<LocationListDto>(cacheKey);

			if (nightlifeList != null)
			{
				return nightlifeList;
			}

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Nightlife Shadow'";

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

			if (list == null)
			{
				list.Locations = PopulateNightlife(parentLocationId, null);
			}

			if (list != null)
			{
				_cache.AddOrUpdate(cacheKey, list);
			}

			return list;
        }

		private List<LocationDto> PopulateNightlife(int parentLocationId, string category)
		{
			List<LocationDto> result = new List<LocationDto>();

			var location = _loctionService.ReturnLocationById(parentLocationId);

			if (category == null)
			{
				category = "Nightlife";
			}

			if (location != null)
			{
				var venueResponse = _venueService.ReturnVenuesByLocation(category, location.RegionNameLong);

				foreach (var venue in venueResponse.Response.Venues)
				{
					string id = "777" + new string(venue.Id.Where(Char.IsDigit).ToArray()).Substring(0, 4);

					if (venue.Categories.Where(q => q.PluralName.ToLower().Contains("bar") || q.PluralName.ToLower().Contains("nightclub")).Count() > 0)
					{

						string vanueCategory = "unknown";

						if (venue.Categories.Count > 0)
						{
							vanueCategory = venue.Categories[0].ShortName.ToLower();
						}

						LocationDto dto = new LocationDto
						{
							Doctype = "nightlifeList",
							RegionID = int.Parse(id),
							RegionType = "Nightlife Shadow",
							SubClass = vanueCategory,
							RegionName = Regex.Replace(venue.Name, @"[^0-9a-zA-Z]'", " "),
							RegionNameLong = venue.Name + ", " + venue.Location.Country,
							ParentRegionID = parentLocationId,
							ParentRegionName = location.RegionName,
							ParentRegionType = location.RegionType,
							ParentRegionNameLong = location.RegionNameLong,
							LetterIndex = venue.Name.ToLower().Substring(0, 3),
							LocationCoordinates = new LocationCoordinatesDto { Latitude = venue.Location.Lat, Longitude = venue.Location.Lng },
							//Url = "/" + id + "/visit-location/" + StripValues(venue.Name),
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

						_loctionService.UpdateLocation(dto, false);

						if (dto.SubClass.ToLower() == category)
						{
							result.Add(dto);
						}
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
