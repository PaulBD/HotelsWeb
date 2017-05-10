using System;
using core.extras.dtos;
using System.Net.Http;
using ServiceStack.Text;

namespace core.extras.services
{
    public class ParkingService : BaseService, IParkingService
    {
        #region Parking Availability

        /// <summary>
        /// Parking Availability
        /// </summary>
        public AirportParkingResponseDto AvailabilityAtDestination(string locationName, string dropoffDate, string dropoffTime, string pickupDate, string pickupTime, string initials, string language, int passengerCount)
        {
            var tokenResponse = GetUserToken();

            var url = ServerAddress + "/v1/carpark/" + locationName + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", dropoffDate);
            url += "&ArrivalTime=" + String.Format("{0:HHmm}", dropoffTime);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", pickupDate);
            url += "&DepartTime=" + String.Format("{0:HHmm}", pickupTime);
            url += "&token=" + tokenResponse.API_Reply.Token;
            
            if (!string.IsNullOrEmpty(language))
            {
                url += "&Lang=" + language;
            }

            if (passengerCount > 0)
            {
                url += "&NumberOfPax=" + passengerCount;
            }

            if (!string.IsNullOrEmpty(initials))
            {
                url += "&Initials=" + initials;
            }

            url += "&fields=tripappcarparksellpoint,tripapptransfertip,introduction,images,address,latitude,longitude,features,tripappimages,sellpoint_location,sellpoint_terminal,sellpoint_transfers,sellpoint_parking,sellpoint_security,transfers,logo";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<AirportParkingResponseDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }

        #endregion
    }
}
