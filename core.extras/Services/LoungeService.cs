using System;
using core.extras.dtos;
using System.Net.Http;
using ServiceStack.Text;

namespace core.extras.services
{
    public class LoungeService : BaseService, ILoungeService
    {
		#region Lounge Availability

		/// <summary>
		/// Lounge Availability
		/// </summary>
		public LoungeAvailabilityResponseDto AvailabilityAtDestination(string locationName, string arrivalDate, string arrivalTime, string flightTime, int adults, int children, int infants, string language)
        {
            var tokenResponse = GetUserToken();

            var url = ServerAddress + "/v1/lounge/" + locationName + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&ArrivalTime=" + String.Format("{0:HHmm}", arrivalTime);
            url += "&Adults=" + adults;
            url += "&Children=" + children;
            url += "&token=" + tokenResponse.API_Reply.Token;

            if (!string.IsNullOrEmpty(language))
            {
                url += "&Lang=" + language;
            }

            url += "&fields=openingtime,closingtime,why_bookone,tripappcarparksellpoint,tripapptransfertip,introduction,images,address,latitude,longitude,features,tripappimages,sellpoint_location,sellpoint_terminal,sellpoint_transfers,sellpoint_parking,sellpoint_security,transfers,logo";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<LoungeAvailabilityResponseDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }

        #endregion
    }
}
