using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using core.hotels.dtos;
using ServiceStack.Text; using library.couchbase;

namespace core.hotels.services
{
    public class HotelService : IHotelService
    {
        private string _apiKey = "8a6ql3j1010cnihnpfbf6j1ad";
        private string _secret = "2s4g8jjsimsmv";
        private string _accountId = "406390";
        private string _url = "https://book.api.ean.com";
        private string _query;


		private CouchBaseHelper _couchbaseHelper;
		private readonly string _bucketName = "TriperooHotels";

		public HotelService()
		{
			_couchbaseHelper = new CouchBaseHelper();
            _query = "SELECT address, airportCode, amenitiesDescription, chain, chainCodeID, checkInTime, checkOutTime, confidence, diningDescription, doctype, eanHotelID, highRate, lastUpdatedDate, location,locationCoordinates, locationDescription, locationSummary, lowRate, mandatoryFeesDescription, name, nationalRatingsDescription, policyDescription, propertyCategory, propertyCurrency, propertyFeesDescription, recreationDescription, regionID, renovationDescription, roomDescription, sequenceNumber, spaDescription, supplierType, starRating, whatToExpect FROM " + _bucketName;
		}

        public string Authenticate()
        {
            return MD5GenerateHash(_apiKey + _secret + (Int32)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

		/// <summary>         /// Return a list of hotels by place id         /// </summary>         public List<HotelDto> ReturnHotelsByLocationId(int locationId)
		{
			var q = _query + " WHERE regionID = " + locationId + " ORDER BY sequenceNumber";

			return ProcessQuery(q);
		}

		/// <summary>         /// Return a list of hotels by proximity         /// </summary>         public List<HotelDto> ReturnHotelsByProximity(double longitude, double latitude, double radius)
		{
			var degLat = utility.Location.Deg2rad(latitude);
			var degLon = utility.Location.Deg2rad(longitude);

			var mapPoint = new utility.Location.MapPoint { Latitude = latitude, Longitude = longitude };

			var boundingBox = utility.Location.GetBoundingBox(mapPoint, radius);

			var meridian180condition = " AND ";

			if (boundingBox.MinPoint.Longitude > boundingBox.MaxPoint.Longitude)
			{
				meridian180condition = " OR ";
			}

			var q = _query + " WHERE (RADIANS(locationCoordinates.latitude) >= " + boundingBox.MinPoint.Latitude + " and RADIANS(locationCoordinates.latitude) <= " + boundingBox.MaxPoint.Latitude + ") and ";
			q += "(RADIANS(locationCoordinates.longitude) >= " + boundingBox.MinPoint.Longitude + meridian180condition + " RADIANS(locationCoordinates.longitude) <= " + boundingBox.MaxPoint.Longitude + ")";
			q += " AND acos(sin( RADIANS(" + degLat + ")) * sin (RADIANS(locationCoordinates.latitude)) + cos( RADIANS(" + degLat + " )) ";
			q += " * cos(RADIANS(locationCoordinates.latitude)) * cos (RADIANS(locationCoordinates.longitude) - RADIANS( " + degLon + "))) <= " + radius / 6371.0;

			return ProcessQuery(q);
		}

		/// <summary>         /// Return a single hotel By id         /// </summary>         public HotelDto ReturnHotelById(int id)         {             var q = _query + " WHERE eanHotelID = '" + id + "'";              var result = ProcessQuery(q);              if (result.Count > 0)             {                 return result[0];             }              return null;         }

		/// <summary>         /// Process Query         /// </summary>         private List<HotelDto> ProcessQuery(string q)
		{
			return _couchbaseHelper.ReturnQuery<HotelDto>(q, _bucketName);
		}

        /// <summary>
        /// Return Hotel List
        /// </summary>
        public HotelAPIListDto ReturnHotelsByLocationId(string sessionId, string locale, string currencyCode, int locationId, DateTime arrivalDate, int nights, string rooms1, string rooms2, string rooms3)
        {
            var city = "";
            var countryCode = "";


            var url = _url + "/ean-services/rs/hotel/v3/list?sig=" + Authenticate();
            url += "&apiKey=" + _apiKey + "&cid=" + _accountId;
            url += "&customerSessionId=" + sessionId;
            url += "&minorRev=30&locale=" + locale;
            url += "&currencyCode=" + currencyCode;
            url += "&city=" + city + "&countryCode=" + countryCode;
			url += "&arrivalDate=" + arrivalDate + "&departureDate=" + arrivalDate.AddDays(nights);
			url += "&room1=" + rooms1;

            if (!string.IsNullOrEmpty(rooms2))
            {
				url += "&room2=" + rooms2;
            }

            if (!string.IsNullOrEmpty(rooms3))
            {
                url += "&room3=" + rooms3;
            }

            url += "&apiExperience=PARTNER_AFFILIATE";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<HotelAPIListDto>(CleanUpResult(result.Content.ReadAsStringAsync().Result));
                }
            }

            return null;
        }

        #region Security

        private string CleanUpResult(string result)
        {
            return result.Replace("\"@", "");
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
