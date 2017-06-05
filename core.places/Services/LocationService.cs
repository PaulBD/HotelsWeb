using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;
using library.foursquare.services;
using library.foursquare.dtos;
using System.Net.Http;
using ServiceStack.Text;

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

            var result = ProcessQuery(q)[0];


            if (result.FormattedAddress.Count == 0)
            {
                // Go to Foresquare and add this extra detail

                var foresquareResult = _venueService.ReturnVenuesByLocation(result.RegionName, result.ParentRegionName);

                if (foresquareResult != null){
                    result = FindLocation(result, foresquareResult);

                    var foresquarePhotos = _venueService.UpdatePhotos(result.SourceData.ForesquareId);

                    result = AttachPhotos(result, foresquarePhotos);

                    UpdateLocation("location:" + result.RegionID, result);
                }
			}


            return result;
        }

        private LocationDto AttachPhotos(LocationDto locationDto, ForesquarePhotosDto photoDto)
        {
			locationDto.Photos.PhotoCount = photoDto.response.photos.count;

			foreach (var item in photoDto.response.photos.items)
			{
				locationDto.Photos.PhotoList.Add(new PhotoList
				{
					height = item.height,
					prefix = item.prefix,
					suffix = item.suffix,
					width = item.width
				});
			}

            return locationDto;
        }

        private LocationDto FindLocation(LocationDto locationDto, VenueDto venueDto)
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
        public void UpdateLocation(string reference, LocationDto dto)
        {
            _couchbaseHelper.AddRecordToCouchbase(reference, dto, _bucketName);
        }
    }
}
