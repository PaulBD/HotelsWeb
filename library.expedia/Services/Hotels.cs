using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using library.expedia.dtos;
using ServiceStack.Text;

namespace library.expedia.services
{
    public class Hotels
    {
        private string _apiKey = "8a6ql3j1010cnihnpfbf6j1ad";
        private string _secret = "2s4g8jjsimsmv";
        private string _accountId = "406390";
        private string _url = "https://book.api.ean.com";

        public Hotels()
        {
        }

        public string Authenticate()
        {
            return MD5GenerateHash(_apiKey + _secret + (Int32)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        /// <summary>
        /// Return Hotel List
        /// </summary>
        public HotelListDto ReturnHotelList(string sessionId, string locale, string currencyCode, string city, string countryCode, DateTime arrivalDate, int nights, List<string> rooms)
        {
            var url = _url + "/ean-services/rs/hotel/v3/list?sig=" + Authenticate();
            url += "&apiKey=" + _apiKey + "&cid=" + _accountId;
            url += "&customerSessionId=" + sessionId;
            url += "&minorRev=30&locale=" + locale;
            url += "&currencyCode=" + currencyCode;
            url += "&city=" + city + "&countryCode=" + countryCode;
            url += "&arrivalDate=" + arrivalDate + "&departureDate=" + arrivalDate.AddDays(nights);

            for (var i = 1; i < rooms.Count + 1; i++)
			{
				url += "&room" + i + "=" + rooms[i - 1];
            }

            url += "&apiExperience=PARTNER_AFFILIATE";

			var message = new HttpRequestMessage(HttpMethod.Get, url);

			using (var client = new HttpClient())
			{
				var result = client.SendAsync(message).Result;

				if (result.IsSuccessStatusCode)
				{
                    return JsonSerializer.DeserializeFromString<HotelListDto>(CleanUpResult(result.Content.ReadAsStringAsync().Result));
				}
			}

			return null;
        }

        private string CleanUpResult(string result)
        {
            return result.Replace("\"@", ""); 
        }

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
