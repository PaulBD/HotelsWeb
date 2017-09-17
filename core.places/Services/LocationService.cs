using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.foursquare.services;
using library.foursquare.dtos;
using library.wikipedia.services;
using System.IO;
using System;

namespace core.places.services
{
    public class LocationService : ILocationService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
        private readonly string _tempBucketName = "TriperooCommonStaging";
        private string _query;
        private IVenueService _venueService;
        private IContentService _contentService;

        public LocationService()
        {
            _venueService = new VenueService();
            _contentService = new ContentService();
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
            bool requiresUpdate = false;
            var q = _query + " WHERE regionID = " + locationId;

            var result = ProcessQuery(q)[0];

            // Wikepedia
            /*
            if (result.Summary != null)
            {
                if (string.IsNullOrEmpty(result.Summary.En))
                {
                    var wikipediaResult = _contentService.ReturnContentByLocation(result.RegionName);

                    if (!string.IsNullOrEmpty(wikipediaResult))
                    {
                        if (!wikipediaResult.Contains("From a page move") && !wikipediaResult.Contains("This is a redirect"))
                        {
                            result.Summary.En = wikipediaResult;
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
			*/

            if (requiresUpdate)
            {
                UpdateLocation("location:" + result.RegionID, result, false);
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
                locationDto.Photos.PhotoCount = foresquarePhotos.response.photos.count;

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
        public void UpdateLocation(string reference, LocationDto dto, bool isStaging)
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
        public void UploadPhoto(int locationId, Stream fileStream, string fileName, string contentType, string customerReference)
        {
            string containerName = "customerimages";
            var dateStamp = DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;

            var newFileName = customerReference.Replace("customer:", "") + "/" + dateStamp + "-" + fileName;
            // Store URL against Location with User Id / Name
            var storage = new library.azure.services.StorageService();
            storage.UploadToStorage(containerName, fileStream, newFileName, contentType);

            var location = ReturnLocationById(locationId);

            location.Photos.PhotoList.Add(new PhotoList()
            {
                customerReference = customerReference,
                prefix = "https://triperoostorage.blob.core.windows.net/",
                suffix = containerName + "/" + newFileName,
                height = 0,
                width = 0
            });

            UpdateLocation("location:" + location.RegionID, location, false);

        }

    }
}
