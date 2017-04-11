using core.extras.dtos;
using core.extras.services;
using ServiceStack.Text;
using System;
using System.Net.Http;

namespace core.extras.services
{
    public class AirportHotelService : BaseService, IAirportHotelService
    {
        public AirportHotelResponseDto AvailabilityAtHotel(string locationName, string arrivalDate, string departDate, string flightDate, int nights, string roomType, string secondRoomType, int parkingDays, string language)
        {
            var tokenResponse = GetUserToken();

            var url = ServerAddress + "/v1/hotel/" + locationName + ".js?key=" + Key;
            url += "&ArrivalDate=" + String.Format("{0:yyyy-MM-dd}", arrivalDate);
            url += "&DepartDate=" + String.Format("{0:yyyy-MM-dd}", departDate);
            url += "&Nights=" + nights;
            url += "&RoomType=" + roomType;
            url += "&ParkingDays=" + parkingDays;
            url += "&token=" + tokenResponse.API_Reply.Token;

            if (!string.IsNullOrEmpty(secondRoomType))
            {
                url += "&SecondRoomType=" + secondRoomType;
            }

            if (!string.IsNullOrEmpty(flightDate))
            {
                url += "&FlightDate=" + String.Format("{0:yyyy-MM-dd}", flightDate);
            }

            if (!string.IsNullOrEmpty(language))
            {
                url += "&Lang=" + language;
            }

            url += "&fields=images,address,latitude,longitude,sellingpoint,browser_description,features,tripappimages,sellpoint_location,sellpoint_terminal,sellpoint_transfers,sellpoint_parking,sellpoint_security,transfers,logo";

            var message = new HttpRequestMessage(HttpMethod.Get, url);

            using (var client = new HttpClient())
            {
                var result = client.SendAsync(message).Result;

                if (result.IsSuccessStatusCode)
                {
                    return JsonSerializer.DeserializeFromString<AirportHotelResponseDto>(result.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}
