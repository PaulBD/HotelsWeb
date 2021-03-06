﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using core.hotels.dtos;
using ServiceStack.Text;
using library.couchbase;
using System.Linq;
using core.places.dtos;
using core.places.services;

namespace core.hotels.services
{
    public class HotelService : IHotelService
    {
        private string _apiKey = "8a6ql3j1010cnihnpfbf6j1ad";
        private string _secret = "2s4g8jjsimsmv";
        private string _accountId = "406390";
        private string _url = "https://book.api.ean.com";
        private string _query;
        private ILocationService _locationService;

        public HotelService(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public string Authenticate()
        {
            return MD5GenerateHash(_apiKey + _secret + (Int32)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		#region Get Hotel By Id

		/// <summary>
        /// Return a single hotel By id
        /// </summary>
        public HotelDto ReturnHotelById(int hotelId, string locale, string currencyCode, int locationId)
        {
            var hotel = new HotelDto();
            var url = _url + "/ean-services/rs/hotel/v3/info?cid=" + _accountId + "&minorRev=99&apiKey=" + _apiKey + "&locale=" + locale + "&currencyCode=" + currencyCode + "&_type=json&sig=" + Authenticate() + "&xml=";

            var xml = "<HotelInformationRequest>";
            xml += "<hotelId>" + hotelId + "</hotelId>";
            xml += "<options>0</options>";
            xml += "</HotelInformationRequest>";

			url += xml;

			var message = new HttpRequestMessage(HttpMethod.Get, url);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
					var r = CleanUpResult(result.Content.ReadAsStringAsync().Result);
                    hotel = JsonSerializer.DeserializeFromString<HotelDto>(r);

                    CreateNewHotelRecord(hotel, locationId);
				}
			}

            return hotel;
		}

        #endregion

        #region Create Hotel Record

        private void CreateNewHotelRecord(HotelDto hotel, int locationId)
        {
            var location = new LocationDto();
            var hotelLocation = _locationService.ReturnLocationById(locationId, true);

            if (hotelLocation != null)
            {
                location.Doctype = "Expedia";
                location.SubClass = ReturnPropertyCategory(hotel.HotelInformationResponse.HotelSummary.propertyCategory);
                location.LetterIndex = hotel.HotelInformationResponse.HotelSummary.name.Substring(0, 3);
                location.RegionID = hotel.HotelInformationResponse.hotelId;
                location.RegionName = hotel.HotelInformationResponse.HotelSummary.name;
                location.FormattedAddress.Add(hotel.HotelInformationResponse.HotelSummary.address1);
                location.FormattedAddress.Add(hotel.HotelInformationResponse.HotelSummary.city);
                location.FormattedAddress.Add(hotel.HotelInformationResponse.HotelSummary.postalCode);
                location.CountryCode = hotel.HotelInformationResponse.HotelSummary.countryCode;
                location.Latitude = hotel.HotelInformationResponse.HotelSummary.latitude;
                location.Longitude = hotel.HotelInformationResponse.HotelSummary.longitude;
                location.LocationCoordinates = new places.dtos.LocationCoordinatesDto()
                {
                    Latitude = hotel.HotelInformationResponse.HotelSummary.latitude,
                    Longitude = hotel.HotelInformationResponse.HotelSummary.longitude
                };

                var imageUrl = "";
                var images = hotel.HotelInformationResponse.HotelImages.HotelImage.FirstOrDefault(q => q.heroImage == true);

                if (images != null)
                {
                    imageUrl = images.highResolutionUrl;
                }

                location.Image = imageUrl;
                location.Url = hotelLocation.Url + "/hotels/" + hotel.HotelInformationResponse.hotelId;

                location.RegionNameLong = hotel.HotelInformationResponse.HotelSummary.name + ", " + hotelLocation.RegionName;
                location.ParentRegionID = hotelLocation.RegionID;
                location.ParentRegionType = hotelLocation.RegionType;
                location.ParentRegionImage = hotelLocation.Image;
                location.ParentRegionName = hotelLocation.RegionName;
                location.ParentRegionNameLong = hotelLocation.RegionNameLong;

                _locationService.UpdateLocation(location, false, "hotel:" + hotel.HotelInformationResponse.hotelId);
            }
        }

        private string ReturnPropertyCategory(int propertyCategory)
        {
            switch (propertyCategory)
            {
                case 1:
                    return "Hotel";
                case 2:
                    return "Suite";
                case 3:
                    return "Resort";
                case 4:
                    return "Vacation Rental";
                case 5:
                    return "Bed & Breakfast";
                case 6:
                    return "All Inclusive";
            }

            return "Unknown";
        }

		#endregion

		#region Return room availability

		/// <summary>
		/// Return room availability
		/// </summary>
		public RoomAvailabilityDto ReturnRoomAvailability(int hotelId, string locale, string currencyCode, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3)
		{
			var url = _url + "/ean-services/rs/hotel/v3/avail?cid=" + _accountId + "&minorRev=99&apiKey=" + _apiKey + "&locale=" + locale + "&currencyCode=" + currencyCode + "&_type=json&sig=" + Authenticate() + "&xml=";

			var xml = "";

			var departureDate = arrivalDate.AddDays(nights);

            xml += "<HotelRoomAvailabilityRequest>";
			xml += " <hotelId>" + hotelId + "</hotelId>";
			xml += " <arrivalDate>" + arrivalDate.Month + "/" + arrivalDate.Day + "/" + arrivalDate.Year + "</arrivalDate>";
			xml += " <departureDate>" + departureDate.Month + "/" + departureDate.Day + "/" + departureDate.Year + "</departureDate>";
			xml += " <includeDetails>true</includeDetails>";
			xml += " <includeRoomImages>true</includeRoomImages>";
			xml += " <RoomGroup>";

			if (!string.IsNullOrEmpty(rooms1))
			{
				xml += "  <Room>";
				xml += "   <numberOfAdults>" + rooms1 + "</numberOfAdults>";
				xml += "  </Room>";
			}

			if (!string.IsNullOrEmpty(rooms2))
			{
				xml += "  <Room>";
				xml += "   <numberOfAdults>" + rooms2 + "</numberOfAdults>";
				xml += "  </Room>";
			}

			if (!string.IsNullOrEmpty(rooms3))
			{
				xml += "  <Room>";
				xml += "   <numberOfAdults>" + rooms3 + "</numberOfAdults>";
				xml += "  </Room>";
			}

			xml += " </RoomGroup>";
            xml += "</HotelRoomAvailabilityRequest>";
			url += xml;

			var message = new HttpRequestMessage(HttpMethod.Get, url);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
					var r = CleanUpResult(result.Content.ReadAsStringAsync().Result);
					return JsonSerializer.DeserializeFromString<RoomAvailabilityDto>(r);
				}
			}

			return null;
		}

		#endregion

		#region Return a list of hotels by proximity

		/// <summary>
		/// Return a list of hotels by proximity
		/// </summary>
		public HotelAPIListDto ReturnHotelsByProximity(string locale, string currencyCode, double longitude, double latitude, double radius, List<int> propertyCategory, float minRate, float maxRate, int minStarRating, int maxStarRating, int minTripAdvisorRating, int maxTripAdvisorRating, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3, string sortBy, int exclude)
		{
			var url = _url + "/ean-services/rs/hotel/v3/list?cid=" + _accountId + "&minorRev=99&apiKey=" + _apiKey + "&locale=" + locale + "&currencyCode=" + currencyCode + "&_type=json&sig=" + Authenticate() + "&xml=";

			var xml = "";

			xml += " <latitude>" + latitude + "</latitude>";
			xml += " <longitude>" + longitude + "</longitude>";
			xml += " <searchRadius>" + radius + "</searchRadius>";
			xml += " <searchRadiusUnit>MI</searchRadiusUnit>";
			xml += " <sort>" + sortBy + "</sort>";
			//xml += " <numberOfResults>25</numberOfResults>"; 
            xml += " <includeDetails>true</includeDetails>";
			xml += " <includeRoomImages>true</includeRoomImages>";

			return ReturnHotels(url, xml, propertyCategory, minRate, maxRate, minStarRating, maxStarRating, minTripAdvisorRating, maxTripAdvisorRating, checkDates, arrivalDate, nights, rooms1, rooms2, rooms3, exclude);
		}

		#endregion

		#region Get Hotel By Location Id

		/// <summary>
		/// Return Hotel List
		/// </summary>
		public HotelAPIListDto ReturnHotelsByLocationId(string locale, string currencyCode, string location, List<int> propertyCategory, float minRate, float maxRate, int minStarRating, int maxStarRating, int minTripAdvisorRating, int maxTripAdvisorRating, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3, string sortBy, int exclude)
        {
            var departureDate = arrivalDate.AddDays(nights);

            var url = _url + "/ean-services/rs/hotel/v3/list?cid=" + _accountId + "&minorRev=99&apiKey=" + _apiKey + "&locale=" + locale + "&currencyCode=" + currencyCode + "&_type=json&sig=" + Authenticate() + "&xml=";

            var xml = "";

            var locArray = location.Split(',');

            xml += " <city>" + locArray[0].Trim() + "</city>";
            xml += " <country>" + locArray[locArray.Length - 1].Trim() + "</country>";

            xml += " <arrivalDate>" + arrivalDate.Month + "/" + arrivalDate.Day + "/" + arrivalDate.Year + "</arrivalDate>";
            xml += " <departureDate>" + departureDate.Month + "/" + departureDate.Day + "/" + departureDate.Year + "</departureDate>";
			xml += " <sort>" + sortBy + "</sort>";
			//xml += " <numberOfResults>25</numberOfResults>";
			xml += " <includeDetails>true</includeDetails>";
			xml += " <includeRoomImages>true</includeRoomImages>";

			return ReturnHotels(url, xml, propertyCategory, minRate, maxRate, minStarRating, maxStarRating, minTripAdvisorRating, maxTripAdvisorRating, checkDates, arrivalDate, nights, rooms1, rooms2, rooms3, exclude);
		}

        #endregion

		#region Security

		private string CleanUpResult(string result)
        {
			result = result.Replace("&gt;", ">");
			result = result.Replace("&lt;", "<");
            return result.Replace("\"@", "\"");
        }

		#endregion

		#region GetHotels

        /// <summary>
        /// Returns the hotels.
        /// </summary>
		private HotelAPIListDto ReturnHotels(string url, string xmlContent, List<int> propertyCategory, float minRate, float maxRate, int minStarRating, int maxStarRating, int minTripAdvisorRating, int maxTripAdvisorRating, bool checkDates, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3, int exclude)
		{
			var xml = "";

			xml += "<HotelListRequest>";
			xml += xmlContent;

			if (checkDates)
			{
				var departureDate = arrivalDate.AddDays(nights);

                if (propertyCategory != null)
                {
                    if (propertyCategory.Count > 0)
                    {
                        xml += "<propertyCategory>" + propertyCategory + "</propertyCategory>";
                    }
                }

				if (minRate > 0)
				{
					xml += "<minRate>" + minRate + "</minRate>";
				}

				if (maxRate > 0)
				{
					xml += "<maxRate>" + maxRate + "</maxRate>";
				}

				if (minStarRating > 0)
				{
					xml += "<minStarRating>" + minStarRating + "</minStarRating>";
				}

				if (maxStarRating > 0)
				{
					xml += "<maxStarRating>" + maxStarRating + "</maxStarRating>";
				}

				if (minTripAdvisorRating > 0)
				{
					xml += "<minTripAdvisorRating>" + minTripAdvisorRating + "</minTripAdvisorRating>";
				}

				if (maxTripAdvisorRating > 0)
				{
					xml += "<maxTripAdvisorRating>" + maxTripAdvisorRating + "</maxTripAdvisorRating>";
				}

				// TODO: Add exclude

				xml += " <arrivalDate>" + arrivalDate.Month + "/" + arrivalDate.Day + "/" + arrivalDate.Year + "</arrivalDate>";
				xml += " <departureDate>" + departureDate.Month + "/" + departureDate.Day + "/" + departureDate.Year + "</departureDate>";
				xml += " <RoomGroup>";

				if (!string.IsNullOrEmpty(rooms1))
				{
					xml += "  <Room>";
					xml += "   <numberOfAdults>" + rooms1 + "</numberOfAdults>";
					xml += "  </Room>";
				}

				if (!string.IsNullOrEmpty(rooms2))
				{
                    if (rooms2 != "0")
                    {
                        xml += "  <Room>";
                        xml += "   <numberOfAdults>" + rooms2 + "</numberOfAdults>";
                        xml += "  </Room>";
                    }
				}

				if (!string.IsNullOrEmpty(rooms3))
				{
                    if (rooms3 != "0")
                    {
                        xml += "  <Room>";
                        xml += "   <numberOfAdults>" + rooms3 + "</numberOfAdults>";
                        xml += "  </Room>";
                    }
				}

				xml += " </RoomGroup>";
			}

			xml += "<numberOfResults>200</numberOfResults>";

            if (exclude > 0)
            {
                xml += "<exclude>" + exclude + "</exclude>";
            }

			xml += "</HotelListRequest>";

			var message = new HttpRequestMessage(HttpMethod.Get, url + xml);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
					var r = CleanUpResult(result.Content.ReadAsStringAsync().Result);
					var jsonResult = JsonSerializer.DeserializeFromString<HotelAPIListDto>(r);

                    if (exclude > 0)
                    {
                        jsonResult.hotelListResponse.hotelList.hotelSummary = jsonResult.hotelListResponse.hotelList.hotelSummary.Where(q => q.hotelId != exclude).ToList();
                    }

                    jsonResult.MapLocations = jsonResult.hotelListResponse.hotelList.hotelSummary.Select(x => new dtos.MapLocationDto { RegionName = x.name, LocationCoordinates = new dtos.LocationCoordinatesDto() { Latitude = x.latitude, Longitude = x.longitude}, SubClass = "", Url = "", Image = x.imagelUrl }).ToList();

                    return jsonResult;
				}
			}

			return null;
		}

		#endregion

		#region Security

		/// <summary>
		/// Generate MD5 Hash
		/// </summary>
		private string MD5GenerateHash(string strInput)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            var md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strInput));

            // Create a new Stringbuilder to collect the bytes and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int nIndex = 0; nIndex < data.Length; ++nIndex)
            {
                sBuilder.Append(data[nIndex].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion
    }
}
