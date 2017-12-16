using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.foursquare.services;
using library.foursquare.dtos;
using library.wikipedia.services;
using System.IO;
using System;
using System.Configuration;
using library.caching;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
        private readonly string _tempBucketName = "TriperooCommonStaging";
        private string _query;
        private IVenueService _venueService;
        private IWikipediaService _contentService;
        private readonly ICacheProvider _cache;

        public LocationService(ICacheProvider cache)
        {
            _cache = cache;
            _contentService = new WikipediaService();
            _venueService = new VenueService();
            _couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats, locationDetail, suggestedActivity, airportCode, countryCode FROM " + _bucketName;
        }

        /// <summary>
        /// Return a list of places for autocomplete
        /// </summary>
        public List<LocationDto> ReturnLocationsForAutocomplete(string searchValue, string searchType)
        {
            var cacheKey = searchValue.Substring(0, 3);

            var contentList = _cache.Get<List<LocationDto>>(cacheKey);

            if (contentList != null)
            {
                return contentList;
            }

            var q = _query + " WHERE letterIndex = '" + searchValue.Substring(0, 3) + "' AND searchPriority < 999 ORDER BY searchPriority DESC";

            if (searchType == "airport")
            {
                q = _query + " WHERE (letterIndex = '" + searchValue.Substring(0, 3) + "' OR LOWER(airportCode) = '" + searchValue.Substring(0, 3) + "') AND regionType == 'airport' ORDER BY searchPriority DESC";

            }

            var list = ProcessQuery(q);

            if (list != null)
            {
                _cache.AddOrUpdate(cacheKey, list);
            }

            return list;
        }

        /// <summary>
        /// Return a location by Id
        /// </summary>
        public LocationDto ReturnLocationById(int locationId, bool isCity)
        {
            var cacheKey = locationId.ToString();

            var location = _cache.Get<LocationDto>(cacheKey);

            if (location != null)
            {
                return location;
            }

            bool requiresUpdate = false;
            var result = new LocationDto();
            var q = _query + " WHERE regionID = " + locationId;

            if (isCity)
            {
                q += " AND (regionType != 'Restaurants' AND regionType != 'Hotels' AND regionType != 'Attractions')";
            }

            var list = ProcessQuery(q);

            result = list[0];

            // Wikepedia
            if (result.Summary != null)
            {
                if (string.IsNullOrEmpty(result.Summary.en))
                {
                    var wikipediaResult = _contentService.ReturnContentByLocation(result.RegionName);

                    if (!string.IsNullOrEmpty(wikipediaResult))
                    {
                        if (!wikipediaResult.Contains("From a page move") && !wikipediaResult.Contains("This is a redirect"))
                        {
                            result.Summary.en = wikipediaResult;
                            requiresUpdate = true;
                        }
                    }
                }
            }

            // Foresquare
            if (result.FormattedAddress.Count == 0)
            {
                // Go to Foresquare and add this extra detail

                var foresquareResult = _venueService.ReturnVenuesByLocation(result.RegionName, result.ParentRegionName);

                if (foresquareResult != null){
                    result = FindForesquareLocation(result, foresquareResult);

                    if (result.SourceData.ForesquareId != null)
                    {
						result = AttachPhotos(result.SourceData.ForesquareId, result);
						requiresUpdate = true;
                    }

                }
			}
			

            if (requiresUpdate)
            {
                UpdateLocation(result, false);
            }

            if (result != null)
            {
                _cache.AddOrUpdate(cacheKey, result);
            }

            return result;
        }

        /// <summary>
        /// Attachs location photos
        /// </summary>
        public LocationDto AttachPhotos(string foreSquareId, LocationDto locationDto)
        {
            var foresquarePhotos = _venueService.UpdatePhotos(foreSquareId);

            if (foresquarePhotos != null)
            {
                
                foreach (var item in foresquarePhotos.response.photos.items)
                {
                    locationDto.Photos.PhotoList.Add(new PhotoList
                    {
                        height = item.height,
                        prefix = item.prefix,
                        suffix = item.suffix,
                        width = item.width
                    });
                }
            }

            return locationDto;
        }

        /// <summary>
        /// Finds Foresquare location.
        /// </summary>
        /// <returns>The location.</returns>
        /// <param name="locationDto">Location dto.</param>
        /// <param name="venueDto">Venue dto.</param>
        private LocationDto FindForesquareLocation(LocationDto locationDto, VenueDto venueDto)
        {
            Venue firstLocation = null;
            foreach (var v in venueDto.Response.Venues)
            {
                if (utilities.Common.DoesStringMatch(locationDto.RegionName, v.Name))
                {
                    firstLocation = v;
                    break;
                }
            }

            if (firstLocation != null)
            {
                locationDto.SourceData.ForesquareId = firstLocation.Id;

                if (firstLocation.Location != null)
                {
                    locationDto.LocationCoordinates.Latitude = firstLocation.Location.Lat;
                    locationDto.LocationCoordinates.Longitude = firstLocation.Location.Lng;
                    locationDto.FormattedAddress = firstLocation.Location.FormattedAddress;
                }

                if (firstLocation.Categories != null)
                {
                    foreach (var cat in firstLocation.Categories)
                    {
                        locationDto.Tags.Add(cat.ShortName);
                    }
                }

                if (firstLocation.Contact != null)
                {
                    locationDto.ContactDetails.facebook = firstLocation.Contact.Facebook;
                    locationDto.ContactDetails.facebookName = firstLocation.Contact.FacebookName;
                    locationDto.ContactDetails.facebookUsername = firstLocation.Contact.FacebookUsername;
                    locationDto.ContactDetails.formattedPhone = firstLocation.Contact.FormattedPhone;
                    locationDto.ContactDetails.instagram = firstLocation.Contact.Instagram;
                    locationDto.ContactDetails.phone = firstLocation.Contact.Phone;
                    locationDto.ContactDetails.twitter = firstLocation.Contact.Twitter;
                }
            }

            return locationDto;
        }

        /// <summary>
        /// Return a child locations by parent Id
        /// </summary>
        public List<LocationDto> ReturnLocationByParentId(int parentLocationId, string type)
        {
            var cacheKey = parentLocationId.ToString() + "_" + type;

            var contentList = _cache.Get<List<LocationDto>>(cacheKey);

            if (contentList != null)
            {
                return contentList;
            }

            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = '" + type + "'";

            var list = ProcessQuery(q);

            if (list != null)
            {
                _cache.AddOrUpdate(cacheKey, list);
            }

            return list;
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
        public void UpdateLocation(LocationDto dto, bool isStaging)
        { 
            UpdateLocation(dto, isStaging, "location:" + dto.RegionID);
        }

        /// <summary>
        /// Update Location
        /// </summary>
        public void UpdateLocation(LocationDto dto, bool isStaging, string reference)
        {
            if (isStaging)
            {
                _couchbaseHelper.AddRecordToCouchbase(reference, dto, _tempBucketName);
            }
            else
            {
                _couchbaseHelper.AddRecordToCouchbase(reference, dto, _bucketName);
            }
        }


        /// <summary>
        /// Add Location
        /// </summary>
        public void AddLocation(LocationDto dto, bool isStaging)
        {
            string reference = Guid.NewGuid().ToString();
            if (isStaging)
            {
                _couchbaseHelper.AddRecordToCouchbase(reference, dto, _tempBucketName);
            }
            else
            {
                _couchbaseHelper.AddRecordToCouchbase(reference, dto, _bucketName);
            }
        }

        /// <summary>
        /// Uploads the photo.
        /// </summary>
        public PhotoList UploadPhoto(int locationId, Stream fileStream, string fileName, string contentType, string customerReference)
        {
            string containerName = "customerimages";
            var dateStamp = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;

            var newFileName = customerReference.Replace("customer:", "") + "/" + dateStamp + "-" + fileName;
            // Store URL against Location with User Id / Name

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["UseAzure"]))
            {
                var storage = new library.azure.services.StorageService();
                storage.UploadToStorage(containerName, fileStream, newFileName, contentType);
            }

            var photo = new PhotoList()
            {
                customerReference = customerReference,
                prefix = "https://triperoostorage.blob.core.windows.net/",
                suffix = containerName + "/" + newFileName,
                height = 0,
                width = 0,
                photoReference = System.Guid.NewGuid().ToString()
            };

            if (locationId > 0)
            {
                var location = ReturnLocationById(locationId, false);

                location.Photos.PhotoList.Add(photo);

                UpdateLocation(location, false);
            }

            return photo;
        }

    }
}
