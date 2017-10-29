using System;
using System.Net.Http;

namespace core.hotels.services
{
    public class RoomAvailabilityService
    {
        private string _baseUrl = "https://book.api.ean.com/ean-services/rs/hotel/v3/";
        private string _cid = "406390";
        private string _apiKey = "8a6ql3j1010cnihnpfbf6j1ad";
        private string _secret = "2s4g8jjsimsmv";
        //private Encryption _encryption;
        private string _hash = "";

        public RoomAvailabilityService()
        {
            //_encryption = new Encryption();
            //_hash = _encryption.CalculateMD5Hash(_apiKey + _secret + (Int32)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public void CheckRoomAvailability(string hotelId, string arrivalDate, string departDate, int nummberOfAdults, int numberOfChildren, string locale, string currency)
        {
            //TODO: Add Room &room=1,0
            var url = _baseUrl + "/avail?cid=" + _cid + "&minorRev=99&apiKey=" + _apiKey + "&locale=" + locale + "&currencyCode=" + currency + "&sig=" + _hash + "&hotelId=" + hotelId + "&arrivalDate=" + arrivalDate + "&departureDate=" + departDate + "&_type=json";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            var client = new HttpClient();
            var response = client.SendAsync(message).Result;

            if (response.IsSuccessStatusCode)
            {

            }
        }
    }
}
